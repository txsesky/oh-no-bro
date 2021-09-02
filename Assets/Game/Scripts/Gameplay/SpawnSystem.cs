using System;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game
{
    public class SpawnSystem : MonoBehaviour
    {
        public Stack<GameObject> SpawnedObjects = new Stack<GameObject>();

        [SerializeField] private string _prefabToInstantiate = "Game/Prefabs/Characters/BasicKyle Variant";
        [SerializeField] private float _spawnPointsDistance = 15;
        [SerializeField] private float _spawnPointsHeight = 0;
        

        private List<Vector3> _spawnPointsList = new List<Vector3>();

        private void Start()
        {
            GenerateSpawnPoints();
            SpawnObjects();
        }

        private void SpawnObjects()
        {
            GetSpawnPoint(out var spawnPos, out var spawnRot);
            
            var newobj = PhotonNetwork.Instantiate(_prefabToInstantiate, spawnPos, spawnRot, 0);
            SpawnedObjects.Push(newobj);
        }

        private void GenerateSpawnPoints()
        {
            var playersCount = PhotonNetwork.PlayerList.Length;
            var baseAngle = (float) 360 / playersCount;

            _spawnPointsList.Clear();
            
            for (int i = 0; i < playersCount; i++)
            {
                var angle = i * baseAngle;
                var spawnPoint = new Vector3(_spawnPointsDistance * Mathf.Cos(angle), _spawnPointsHeight,
                    _spawnPointsDistance * Mathf.Sin(angle));
                _spawnPointsList.Add(spawnPoint);
            }
        }

        public void GetSpawnPoint(out Vector3 spawnPos, out Quaternion spawnRot)
        {
            var id = PhotonNetwork.LocalPlayer.ActorNumber;
            spawnPos = _spawnPointsList[(id == -1) ? 0 : id % _spawnPointsList.Count];
            spawnRot = Quaternion.identity;
        }
    }
}