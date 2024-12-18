using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [Header("UI References")]
    public RectTransform playerListContainer; // Container for player buttons
    public GameObject playerButtonPrefab;     // Prefab for player buttons
    public TMP_Text lobbyStatusText;          // Text to show lobby status

    private Dictionary<string, GameObject> playerButtons = new Dictionary<string, GameObject>();

    void Start()
    {
        // Display initial lobby state
        UpdateLobbyStatus();
    }

    public void UpdateLobbyStatus()
    {
        if (PhotonNetwork.InRoom)
        {
            lobbyStatusText.text = $"In Lobby: {PhotonNetwork.CurrentRoom.Name} - {PhotonNetwork.CurrentRoom.PlayerCount}/{PhotonNetwork.CurrentRoom.MaxPlayers}";
        }
        else
        {
            lobbyStatusText.text = "Not currently in a lobby.";
        }
    }

    public override void OnJoinedRoom()
    {
        Debug.Log($"Joined Room: {PhotonNetwork.CurrentRoom.Name}");
        UpdateLobbyStatus();

        // Add existing players to the list
        foreach (var player in PhotonNetwork.PlayerList)
        {
            AddPlayer(player.NickName);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log($"Player joined: {newPlayer.NickName}");
        AddPlayer(newPlayer.NickName);
        UpdateLobbyStatus();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log($"Player left: {otherPlayer.NickName}");
        RemovePlayer(otherPlayer.NickName);
        UpdateLobbyStatus();
    }

    public void AddPlayer(string playerName)
    {
        if (playerButtons.ContainsKey(playerName))
        {
            Debug.LogWarning($"Player {playerName} is already in the lobby.");
            return;
        }

        // Instantiate a new player button
        GameObject newPlayerButton = Instantiate(playerButtonPrefab, playerListContainer);
        WlobbyPlayer lobbyPlayerScript = newPlayerButton.GetComponent<WlobbyPlayer>();

        if (lobbyPlayerScript != null)
        {
            lobbyPlayerScript.SetPlayerName(playerName);
        }

        // Track the button
        playerButtons.Add(playerName, newPlayerButton);
    }

    public void RemovePlayer(string playerName)
    {
        if (playerButtons.ContainsKey(playerName))
        {
            Destroy(playerButtons[playerName]);
            playerButtons.Remove(playerName);
        }
        else
        {
            Debug.LogWarning($"Player {playerName} not found in the lobby.");
        }
    }

    public void StartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Starting the game...");
            PhotonNetwork.LoadLevel("GameScene"); // Replace with your game scene name
        }
        else
        {
            Debug.LogWarning("Only the host can start the game.");
        }
    }

    public void LeaveLobby()
    {
        PhotonNetwork.LeaveRoom();
        lobbyStatusText.text = "Leaving lobby...";
    }

    public override void OnLeftRoom()
    {
        Debug.Log("Left the room.");
        UnityEngine.SceneManagement.SceneManager.LoadScene("FrontEnd"); // Replace with your main menu scene name
    }
}
