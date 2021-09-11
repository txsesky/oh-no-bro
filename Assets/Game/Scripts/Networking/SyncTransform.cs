using System;
using Photon.Pun;
using UnityEngine;

namespace Game.Networking
{
    [RequireComponent(typeof(PhotonView))]
    public class SyncTransform : MonoBehaviour, IPunObservable
    {
        private PhotonView _photonView;
        private Vector3 _networkPosition = Vector3.zero;
        private Vector3 _oldPosition = Vector3.zero;
        private float _speed = 1f;

        private void Start()
        {
            _photonView = GetComponent<PhotonView>();

            PhotonNetwork.SendRate = 20;
            PhotonNetwork.SerializationRate = 40;
        }
        
        void Update ()
        {
            if (_photonView.IsMine)
                return;

            var position = transform.position;
            _oldPosition = position;
            
            position = Vector3.MoveTowards(position, _networkPosition, Time.deltaTime * _speed * 10);
            transform.position = position;
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(transform.position);
            }
            else
            {
                _networkPosition = (Vector3)stream.ReceiveNext();
                
                var movement = _networkPosition - _oldPosition;
                var lag = Mathf.Abs((float) (PhotonNetwork.Time - info.SentServerTime));
                
                _networkPosition += (movement * lag);
                _speed = movement.magnitude;
            }
        }
    }
}