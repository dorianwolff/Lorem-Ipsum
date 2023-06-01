using System;
using TMPro;
using UnityEngine;


public class RoomItem : MonoBehaviour
{
    public TextMeshProUGUI roomName;
    private LobbyManager manager;

    private void Start()
    {
        manager = FindObjectOfType<LobbyManager>();
    }

    public void SetRoomName(string _roomName)
    {
        roomName.text = _roomName;
    }

    public void OnClickItem()
    {
        manager.JoinRoom(roomName.text);
    }
}
