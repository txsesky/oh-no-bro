using Game.Components;
using Game.Factory;
using LeoEcsPhysics;
using Leopotam.EcsLite;
using UnityEngine;

namespace Game.Systems {
	sealed class EnvironmentLoadSystem : IEcsInitSystem {
		string _shopPrefab = "Prefabs/Buildings/Shop";
		float _shopColliderRadius = 3f;
		float _shopTriggerRadius = 4f;

		readonly EcsWorld _world = default;
		readonly EcsPool<TransformRefData> _transformPool = default;
		readonly EcsPool<SphereColliderRefData> _colliderPool = default;
		readonly EcsPool<ShopData> _shopPool = default;

		public void Init(EcsSystems systems) {
			SpawnShop();
		}
		
		void SpawnShop() {
			var entity = _world.NewEntity();

			BuildingFactory.CreateBuilding(_shopPrefab, _shopColliderRadius, entity, _world, Vector3.zero, Quaternion.identity);

			ref var transformData = ref _transformPool.Get(entity);
			var go = transformData.Value.gameObject;
			
			ref var colliderData = ref _colliderPool.Add(entity);
			colliderData.Value = go.AddComponent<SphereCollider>();
			colliderData.Value.radius = _shopTriggerRadius;
			colliderData.Value.isTrigger = true;

			go.AddComponent<OnTriggerEnterChecker>();
			go.AddComponent<OnTriggerExitChecker>();

			_shopPool.Add(entity);
		}
	}
}