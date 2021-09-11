using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace Game
{
    public class SharedData
    {
        public Camera Camera;
    
        public float DeltaTime;
        public float FixedDeltaTime;

        public List<ITriggerEventSystem> TriggerEventSystems;
    }
}