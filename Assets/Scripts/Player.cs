using Photon.Pun;
using Photon.Pun.Demo.Cockpit;
using System.Collections;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class Player : MonoBehaviourPun
{
    private Rigidbody2D playerRigidbody;
    private SpriteRenderer spriteRenderer;
    Vector3 currentPosition;
    
    public enum Move
    {
        None,
        Up,
        Left,
        Down,
        Right
    }
    public Move[] move = new Move[6];
    public int index = 0;
    public float speed = 3f;
    public bool isMoveReady;
    bool isMoving;

    private void Start()
    {
        PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable { { "isMoveReady", false } });
        GetComponent<PhotonTransformView>().m_SynchronizePosition= false;
        currentPosition = gameObject.transform.position;
        playerRigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if(photonView.IsMine)
        {
            spriteRenderer.color = Color.blue;
        }
        else
        {
            spriteRenderer.color = Color.red;
        }
    }

    private void Update()
    {
        if(!photonView.IsMine)
        {
            return;
        }
        if(isMoving) { return; }
        if(isMoveReady) { return; }
        //var input = InputButton.VerticalInput;
        //var distance = input * speed * Time.deltaTime;
        //var targetPosition = transform.position + Vector3.up * distance;

        //playerRigidbody.MovePosition(targetPosition);
        if(index == move.Length) {
            Debug.Log("no remain move cost");
            ReadyToMove();
            return;
        }

        if(Input.GetKeyDown(KeyCode.W))
        {
            move[index++] = Move.Up;
            transform.Translate(new Vector3( 0, 1, 0));
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            move[index++] = Move.Left;
            transform.Translate(new Vector3(-1, 0, 0));
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            move[index++] = Move.Down;
            transform.Translate(new Vector3(0, -1, 0));
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            move[index++] = Move.Right;
            transform.Translate(new Vector3(1, 0, 0));
        }

    }
    public void ReadyToMove()
    {
        isMoveReady = true;
        PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable { {"isMoveReady" , true} });
        transform.position = currentPosition;
    }
    public void MoveBoth()
    {
        Debug.Log("MoveBoth");
        isMoving = true;
        GetComponent<PhotonTransformView>().m_SynchronizePosition = true;
        StartCoroutine("MoveDelay");
    }

    IEnumerator MoveDelay()
    {
        if (!photonView.IsMine)
        {
            yield break;
        }
        isMoveReady = false;
        PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable { { "isMoveReady", false } });
        for (int i = 0; i < move.Length; i++)
        {
            if (move[i] == Move.None) { continue; }
            switch(move[i])
            {
                case Move.Up:
                    transform.Translate(new Vector3(0, 1, 0));
                    break;
                case Move.Left:
                    transform.Translate(new Vector3(-1, 0, 0));
                    break;
                case Move.Down:
                    transform.Translate(new Vector3(0, -1, 0));
                    break;
                case Move.Right:
                    transform.Translate(new Vector3(1, 0, 0));
                    break;
            }
            yield return new WaitForSeconds(0.5f);
        }
        index = 0;
        move = new Move[6];
        currentPosition = transform.position;
        isMoving = false;
        GetComponent<PhotonTransformView>().m_SynchronizePosition = false;
        GameManager.Instance.isPlayersMoving = false;
    }
}
