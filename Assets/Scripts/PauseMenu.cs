﻿using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class PauseMenu : MonoBehaviour
{

    public static bool isOn = false;

    private NetworkManager networkManager;

    public void Start()
    {
        networkManager = NetworkManager.singleton;
    }

    public void LeaveRoomButton()
    {
        MatchInfo matchInfo = networkManager.matchInfo;
        networkManager.matchMaker.DropConnection(matchInfo.networkId, matchInfo.nodeId, 0, networkManager.OnDropConnection);
        networkManager.StopHost();
    }


    public void LeaveGameButton()
    {
        Application.Quit();
    }

}
