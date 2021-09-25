using Photon.Pun;
using UnityEngine;

namespace Game.Components
{
    public struct NetSyncPositionData
    {
        public Vector3 CurrentValue;
        public Vector3 OldValue;
        public float Speed;
    }
    
    public struct PhotonViewRef
    {
        public PhotonView Value;
    }
}