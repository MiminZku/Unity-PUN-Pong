using Photon.Pun;
using UnityEngine;

public class Ball : MonoBehaviourPun    //Photon View 컴포턴트로 즉시 접근 가능
{
    public bool IsMasterClientLocal => PhotonNetwork.IsMasterClient && photonView.IsMine;

    private Vector2 direction = Vector2.right;
    private readonly float speed = 10f;
    private readonly float randomReflectionIntensity = 0.1f;
    
    private void FixedUpdate()
    {
        if (!IsMasterClientLocal || PhotonNetwork.PlayerList.Length < 2)
        {
            return;
        }

        var distance = speed * Time.deltaTime;
        var hit = Physics2D.Raycast(transform.position, direction, distance);

        if(hit.collider != null)
        {
            var goalPost = hit.collider.GetComponent<Goalpost>();
            if(goalPost != null)
            {
                GameManager.Instance.AddScore(3 - goalPost.playerNumber, 1);
            }

            direction = Vector2.Reflect(direction, hit.normal);
            direction += Random.insideUnitCircle * randomReflectionIntensity; // Random.insideUnitCircle : 반지름이 1짜리인 원 안에서의 랜덤한 벡터값
        }

        transform.position = (Vector2) transform.position + direction * distance;
    }
}