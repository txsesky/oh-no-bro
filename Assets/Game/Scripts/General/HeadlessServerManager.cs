using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Bolt;
using Photon.Bolt.Matchmaking;
using Photon.Bolt.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HeadlessServerManager : GlobalEventListener
{
    [SerializeField] private string map = "";
    private static string s_map;

    [SerializeField] private string roomID = "Test";
    private static string s_roomID;
    
    [SerializeField] private bool isServer = false;
    
    public bool IsServer
    {
        get => isServer;
        set => isServer = value;
    }

    public static string RoomID()
    {
        return s_roomID;
    }

    public static string Map()
    {
        return s_map;
    }

    public override void BoltStartBegin()
    {
        BoltNetwork.RegisterTokenClass<PhotonRoomProperties>();
    }

    public override void BoltStartDone()
    {
        if (BoltNetwork.IsServer)
        {
            var roomProperties = new PhotonRoomProperties();

            roomProperties.AddRoomProperty("m", map);
            
            roomProperties.IsOpen = true;
            roomProperties.IsVisible = true;

            if (s_roomID.Length == 0)
            {
                s_roomID = Guid.NewGuid().ToString();
            }
            
            BoltMatchmaking.CreateSession(
                sessionID:s_roomID,
                token: roomProperties,
                sceneToLoad: map);
        }
    }

    private void Awake()
    {
        isServer = "true" == (GetArgs("-s", "-isServer") ?? (isServer ? "true" : "false"));
        s_map = GetArgs("-m", "-map") ?? map;
        s_roomID = GetArgs("-r", "-room") ?? roomID;

        if (IsServer)
        {
            var validMap = false;

            foreach (var value in BoltScenes.AllScenes)
            {
                if (SceneManager.GetActiveScene().name != value)
                {
                    if (s_map == value)
                    {
                        validMap = true;
                        break;
                    }
                }
            }

            if (!validMap)
            {
                BoltLog.Error("Invalid configuration: please verify level name");
                Application.Quit();
            }
            
            BoltLauncher.StartServer();
            DontDestroyOnLoad(this);
        }
    }

    private static string GetArgs(params string[] names)
    {
        var args = Environment.GetCommandLineArgs();
        for (int i = 0; i < args.Length; i++)
        {
            foreach (var name in names)
            {
                if (args[i] == name && args.Length > 1 + 1)
                {
                    return args[i + 1];
                }
            }
        }
        return null;
    }
}
