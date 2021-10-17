using System.Collections.Generic;
using Game.Components;
using Game.Factory;
using Game.Networking;
using Game.UI.HUD;
using LeoEcsPhysics;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Photon.Pun;
using UnityEngine;

namespace Game.Systems {
	sealed class SinglePlayerSpawnSystem : IEcsInitSystem {
		float _spawnPointsDistance = 15f;

		readonly EcsWorld _world = default;
		readonly EcsPool<InputData> _inputPool = default;
		readonly EcsPool<PlayerData> _playerPool = default;

		public void Init(EcsSystems systems) {
			var entity = _world.NewEntity();

			CharacterFactory.CreateCharacter(entity, _world, new Vector3(_spawnPointsDistance, 0, 0),
				Quaternion.identity);

			_inputPool.Add(entity);
			_playerPool.Add(entity);
		}
	}
}