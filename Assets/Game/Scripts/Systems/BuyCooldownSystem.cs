using Game.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Game.Systems {
	sealed class BuyCooldownSystem : IEcsRunSystem {
		[EcsShared]
		readonly SharedData _sharedData;
		
		[EcsFilter(typeof(BuyItemEvent))]
		readonly EcsFilter _buyEventGroup;
		
		[EcsFilter(typeof(TimerData), typeof(UItemElmntRef))]
		readonly EcsFilter _timerGroup;

		readonly EcsPool<UItemElmntRef> _uiItemElmntPool = default;
		readonly EcsPool<BuyItemEvent> _buyEventPool = default;
		readonly EcsPool<TimerData> _timerPool = default;

		public void Run(EcsSystems systems) {
			foreach (var buyEventEntity in _buyEventGroup) {
				ref var buyEventData = ref _buyEventPool.Get(buyEventEntity);
				if (_uiItemElmntPool.Has(buyEventData.SenderEntity)) {
					ref var uiItemElmntData = ref _uiItemElmntPool.Get(buyEventData.SenderEntity);
					uiItemElmntData.Value.SetInteractable(false);
					ref var timerData = ref _timerPool.Add(buyEventData.SenderEntity);
					timerData.TimeLeft = 1f;
				}
			}

			foreach (var timerEntity in _timerGroup) {
				ref var timerData = ref _timerPool.Get(timerEntity);
				
				ref var uiItemElmntData = ref _uiItemElmntPool.Get(timerEntity);
				uiItemElmntData.Value.FillRadialProgressImage(timerData.TimeLeft);

				if (timerData.TimeLeft > 0) {
					timerData.TimeLeft -= _sharedData.Time.DeltaTime;
					continue;
				}
				
				uiItemElmntData.Value.SetInteractable(true);
				
				_timerPool.Del(timerEntity);
			}
		}
	}
}