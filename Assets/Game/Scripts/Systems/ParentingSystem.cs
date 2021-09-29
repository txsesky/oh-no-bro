using Game.Components;
using Game.Extensions;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Game.Systems {
	sealed class ParentingSystem : IEcsRunSystem {
		readonly EcsWorld _world = default;

		[EcsFilter(typeof(SetParentData))]
		readonly EcsFilter _pendingGroup = default;

		readonly EcsPool<SetParentData> _setParentPool = default;

		public void Run(EcsSystems systems) {
			foreach (var entity in _pendingGroup) {
				ref var setParentData = ref _setParentPool.Get(entity);
				entity.SetParent(_world, setParentData.Entity, setParentData.LocalTranslation);
				_setParentPool.Del(entity);
			}
		}
	}
}