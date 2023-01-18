using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<GameManager>();

            return instance;
        }
    }

    private static GameManager instance;

    public Text scoreText;
    public Transform[] spawnPositions;
    public GameObject playerPrefab;
    public GameObject ballPrefab;
    private int[] playerScores;
    public bool isPlayersMoving;

    private void Start()
    {
        playerScores = new[] {0,0};
        SpawnPlayer();

        //if(PhotonNetwork.IsMasterClient)
        //{
        //    SpawnBall();
        //}
    }
    private void Update()
    {
        if (PhotonNetwork.PlayerList.Length < 2) return;
        foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
        {
            Hashtable cp = player.CustomProperties;
            Debug.Log(cp["isMoveReady"]);
            if (!(bool)cp["isMoveReady"]) return;
        }
        if (isPlayersMoving) return;
        isPlayersMoving = true;
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            player.GetComponent<Player>().MoveBoth();
        }
    }

    private void SpawnPlayer()
    {
        var localPlayerIndex = PhotonNetwork.LocalPlayer.ActorNumber - 1;
        var spawnPosition = spawnPositions[localPlayerIndex % spawnPositions.Length];
        PhotonNetwork.Instantiate(playerPrefab.name, spawnPosition.position, spawnPosition.rotation);
    }

    private void SpawnBall()
    {
        PhotonNetwork.Instantiate(ballPrefab.name, Vector2.zero, Quaternion.identity);
    }

    public override void OnLeftRoom() // 방 나가질 때 자동 실행(내가 나갈 경우에만)
    {
        SceneManager.LoadScene("Lobby");
    }

    public void AddScore(int playerNumber, int score)
    {
        if (!PhotonNetwork.IsMasterClient) return;
        playerScores[playerNumber - 1] += score;

        photonView.RPC("RPCUpdateScoreText", RpcTarget.All,
            playerScores[0].ToString(), playerScores[1].ToString());
    }

    
    [PunRPC]
    private void RPCUpdateScoreText(string player1ScoreText, string player2ScoreText)
    {
        scoreText.text = $"{player1ScoreText} : {player2ScoreText}";
    }
}