using System;
using Game.Components;
using LeoEcsPhysics;
using Leopotam.EcsLite;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Game.Factory {
	public class BuildingFactory {

		public static void CreateBuilding(string shopPrefab, float colliderRadius, int entity, EcsWorld ecsWorld, Vector3 position, Quaternion rotation) {
			var transformPool = ecsWorld.GetPool<TransformRefData>();
			var setParentPool = ecsWorld.GetPool<SetParentData>();
			var colliderPool = ecsWorld.GetPool<SphereColliderRefData>();
			var rigidbodyPool = ecsWorld.GetPool<RigidbodyRefData>();
			
			var go = Object.Instantiate(Resources.Load(shopPrefab)) as GameObject;
			
			if (go == null)
				return;

			ref var transformData = ref transformPool.Add(entity);
			transformData.Value = go.transform;
			transformData.Value.position = position;
			transformData.Value.rotation = rotation;

			ref var rbData = ref rigidbodyPool.Add(entity);
			rbData.Value = go.AddComponent<Rigidbody>();
			rbData.Value.isKinematic = true;

			go.AddComponent<OnTriggerEnterChecker>();
			go.AddComponent<OnTriggerExitChecker>();

			var child = ecsWorld.NewEntity();

			var childGO = new GameObject("Collider");

			if (childGO == null)
				return;

			ref var tData = ref transformPool.Add(child);
			tData.Value = childGO.transform;

			ref var cData = ref colliderPool.Add(child);
			cData.Value = childGO.AddComponent<SphereCollider>();
			cData.Value.radius = colliderRadius;

			ref var setParentData = ref setParentPool.Add(child);
			setParentData.Entity = entity;
			setParentData.LocalTranslation = Vector3.zero;
		}
	}
}