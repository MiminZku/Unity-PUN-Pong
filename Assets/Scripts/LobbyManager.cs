using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    private readonly string gameVersion = "1";

    public Text connectionInfoText;
    public Button joinButton;
    
    private void Start()
    {
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.ConnectUsingSettings();

        joinButton.interactable = false;
        connectionInfoText.text = "Connecting to master server...";
    }
    
    public override void OnConnectedToMaster()
    {
        joinButton.interactable = true;
        connectionInfoText.text = "Online : Connected to master server";
    }
    
    public override void OnDisconnected(DisconnectCause cause)
    {
        joinButton.interactable = false;
        connectionInfoText.text = $"Offline: Connection disabled {cause.ToString()} - Try reconnecting...";

        PhotonNetwork.ConnectUsingSettings();
    }
    
    public void Connect()
    {
        joinButton.interactable = false;
        if(PhotonNetwork.IsConnected)
        {
            connectionInfoText.text = "Connecting to random room...";
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            connectionInfoText.text = "Offline: Connection disabled - Try reconnecting...";

            PhotonNetwork.ConnectUsingSettings();
        }
    }
    
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        connectionInfoText.text = "There is no empty room, creating new room...";
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 2 });
    }
    
    public override void OnJoinedRoom()
    {
        connectionInfoText.text = "Connected with room.";
        // SceneManager.LoadScene(); -> 나만 넘어가고 다른 플레이어는 안넘어가기 때문에 안됨
        PhotonNetwork.LoadLevel("Main");
    }
}