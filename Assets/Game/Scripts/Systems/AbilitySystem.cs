using Game.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Game.Systems {
	public class AbilitySystem : IEcsRunSystem {

		//[EcsFilter(typeof(OnSpellStartData))]
		//readonly EcsFilter onSpellStartEventGroup;
		
		//readonly EcsPool<OnSpellStartEvent> _onSpellStartEventPool = default;
		//readonly EcsPool<OnSpellStartData> _onSpellStartEventDataPool = default;
		
		public void Run(EcsSystems systems) {
		}
	}
}