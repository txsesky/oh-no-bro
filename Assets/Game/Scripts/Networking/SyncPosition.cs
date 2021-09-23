using System;
using Game.Components;
using Leopotam.EcsLite;
using Photon.Pun;
using UnityEngine;

namespace Game.Networking
{
    public class SyncPosition: MonoBehaviour, IPunObservable
    {
        private int _characterEntity = -1;

        private EcsPool<LocalToWorldData> _positionPool = default;
        private EcsPool<NetSyncPositionData> _netSyncPositionPool = default;
        private void Start()
        {
            PhotonNetwork.SendRate = 30;
            PhotonNetwork.SerializationRate = 30;
        }
        
        public void SetCharacterEntity(int entity)
        {
            _characterEntity = entity;
            _positionPool = RuntimeData.EcsWorld.GetPool<LocalToWorldData> ();
            _netSyncPositionPool = RuntimeData.EcsWorld.GetPool<NetSyncPositionData> ();
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if(_characterEntity == -1)
                return;
            
            ref var charLocalToWorld = ref _positionPool.Get(_characterEntity);
            ref var charNetSyncPos = ref _netSyncPositionPool.Get(_characterEntity);
            
            if (stream.IsWriting)
            {
                stream.SendNext(charLocalToWorld.Position.x);
                stream.SendNext(charLocalToWorld.Position.z);
            }
            else if (stream.IsReading)
            {
                charNetSyncPos.CurrentValue.x = (float)stream.ReceiveNext();
                charNetSyncPos.CurrentValue.z = (float)stream.ReceiveNext();

                var movement = charNetSyncPos.CurrentValue - charNetSyncPos.OldValue;
                var lag = Mathf.Abs((float) (PhotonNetwork.Time - info.SentServerTime));
                
                charNetSyncPos.CurrentValue += (movement * lag);
                charNetSyncPos.Speed = movement.magnitude;
            }
        }
    }
}