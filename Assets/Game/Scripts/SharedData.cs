using System.Collections.Generic;
using Game.Components;
using Game.Managers;
using Photon.Pun;
using UnityEngine;

namespace Game
{
    public class SharedData
    {
        public Camera Camera;
    
        public float DeltaTime;
        public float FixedDeltaTime;

        public GameState GameState;
    }
    
    public enum GameMode
    {
        Client,
        SinglePlayer
    }

    public struct GameState
    {
        public GameMode GameMode;
    }
}