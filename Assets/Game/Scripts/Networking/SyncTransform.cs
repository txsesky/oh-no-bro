using System;
using Game.Components;
using Game.Extensions;
using Leopotam.EcsLite;
using Photon.Pun;
using UnityEngine;

namespace Game.Networking
{
    public class SyncTransform: MonoBehaviour, IPunObservable
    {
        private int _characterEntity = -1;

        private EcsPool<TransformRefData> _positionPool = default;
        private EcsPool<NetSyncPositionData> _netSyncPositionPool = default;

        public void SetCharacterEntity(int entity, EcsWorld world)
        {
            _characterEntity = entity;
            _positionPool = world.GetPool<TransformRefData> ();
            _netSyncPositionPool = world.GetPool<NetSyncPositionData> ();
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if(_characterEntity.IsNull())
                return;
            
            ref var charLocalToWorld = ref _positionPool.Get(_characterEntity);
            ref var charNetSyncPos = ref _netSyncPositionPool.Get(_characterEntity);
            
            if (stream.IsWriting)
            {
                var position = charLocalToWorld.Value.position;
                stream.SendNext(position.x);
                stream.SendNext(position.z);
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