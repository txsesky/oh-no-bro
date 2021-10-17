using System;
using Game.Components;
using Leopotam.EcsLite;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Game.Factory {
	public class CharacterFactory {
		static string _dwarfPrefab = "Prefabs/Characters/Dwarf";
		static float _movementSpeed = 150f;
		static float _colliderRadius = 0.5f;

		public static void CreateCharacter(int entity, EcsWorld ecsWorld, Vector3 position, Quaternion rotation) {
			var transformPool = ecsWorld.GetPool<TransformRefData>();
			var movementSpeedPool = ecsWorld.GetPool<MovementSpeedData>();
			var movementDirectionPool = ecsWorld.GetPool<MovementDirectionData>();
			var characterControllerPool = ecsWorld.GetPool<CharacterControllerRefData>();
			var rigidbodyPool = ecsWorld.GetPool<RigidbodyRefData>();

			var go = Object.Instantiate(Resources.Load(_dwarfPrefab)) as GameObject;

			if (go == null)
				return;

			ref var transformData = ref transformPool.Add(entity);
			transformData.Value = go.transform;
			transformData.Value.position = position;
			transformData.Value.rotation = rotation;

			ref var charControllerData = ref characterControllerPool.Add(entity);
			charControllerData.Value = go.AddComponent<CharacterController>();
			charControllerData.Value.radius = _colliderRadius;

			ref var rbData = ref rigidbodyPool.Add(entity);
			rbData.Value = go.AddComponent<Rigidbody>();
			rbData.Value.isKinematic = true;

			ref var movementSpeedData = ref movementSpeedPool.Add(entity);
			movementSpeedData.BaseValue = _movementSpeed;
			movementSpeedData.FlatModifierValue = 0;
			movementSpeedData.PercentModifierValue = 1;

			movementDirectionPool.Add(entity);
		}
	}
}