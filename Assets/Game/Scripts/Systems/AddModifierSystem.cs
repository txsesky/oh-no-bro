using System;
using Game.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Game.Systems {
	sealed class AddModifierSystem : IEcsRunSystem {
		[EcsFilter(typeof(MovementSpeedData), typeof(PlayerData))]
		readonly EcsFilter _movementSpeedGroup = default;

		[EcsFilter(typeof(BuyItemEvent))]
		readonly EcsFilter _buyEventGroup = default;

		readonly EcsPool<MovementSpeedData> _movementSpeedPool = default;
		readonly EcsPool<ItemComponent> _itemPool = default;
		//readonly EcsPool<Modifier> _itemPool = default;
		readonly EcsPool<BuyItemEvent> _buyEventPool = default;
		//readonly EcsPool<AddModifier> 

		public void Run(EcsSystems systems) {
			foreach (var buyEventEntity in _buyEventGroup) {
				ref var buyEventData = ref _buyEventPool.Get(buyEventEntity);
				if (_itemPool.Has(buyEventData.SenderEntity)) {
					ref var itemData = ref _itemPool.Get(buyEventData.SenderEntity);
					/*foreach (var statModifier in itemData.) {
						switch (statModifier.Stat) {
							case Stat.HEALTH:
								break;
							case Stat.HEALTH_REGEN:
								break;
							case Stat.DAMAGE:
								break;
							case Stat.ATTACKSPEED:
								break;
							case Stat.MOVESPEED:
								foreach (var ent in _movementSpeedGroup) {
									ref var movementSpeedData = ref _movementSpeedPool.Get(ent);
									movementSpeedData.FlatModifierValue += statModifier.FlatValue;
									movementSpeedData.PercentModifierValue += statModifier.PercentValue;
								}

								break;
							case Stat.RES_EXTRACT:
								break;
							case Stat.RES_STEAL:
								break;
							default:
								throw new ArgumentOutOfRangeException();
						}
					}*/
				}
			}

			foreach (var ent in _movementSpeedGroup) {
				ref var movementSpeedData = ref _movementSpeedPool.Get(ent);
				movementSpeedData.Value = movementSpeedData.BaseValue + movementSpeedData.FlatModifierValue *
					movementSpeedData.PercentModifierValue;
				movementSpeedData.Value = Math.Max(movementSpeedData.Value, 100);
				movementSpeedData.Value = Math.Min(movementSpeedData.Value, 500);
				movementSpeedData.ValueMeterPerSec = movementSpeedData.Value / 50f;

				Debug.Log($"movementSpeedMS = {movementSpeedData.ValueMeterPerSec}");
				Debug.Log($"movementSpeed = {movementSpeedData.Value}");
				Debug.Log($"movementSpeedFlatModifier = {movementSpeedData.FlatModifierValue}");
				Debug.Log($"movementSpeedPercentModifier = {movementSpeedData.PercentModifierValue}");
			}
		}
	}
}

//AT = 100*BAT/(BAS+sumof(pos and neg modifiers)); clamp(50,700)
//DMG = BDMG * DMGmul(also reduction -value) + DMGflat(also reduction -value) 

//variant A choosen
//when statChangeEvent invoked
//change picked stat components
//example
//ms stat increased in shop
//statChangeEvent invoked
//ms.add, ms.mul changed
//item dropped
//statChangeEvent invoked
//ms.add ms.mul changed
//
//variant B
//when statChangeEvent invoked
//iterate over statModifier entities with owner entity
//and change stats respectively
//recalc ms.add ms.mul or directly current ms
//
//variant C
//when statChangeEvent invoked
//add statModifiers to picked stat components
//iterate over statModifiers in stat components
//recalc ms.add ms.mul or directly current ms
//
//if(!absolutevalue)
//if(!setvalue)
//foreach(mulValue in mulValues)
//	mulValuesSum += mulValue;(also reduction -value)
//msMul = 1 + mulValuesSum;
//foreach(flatValue in flatValues)
//	flatValuesSum += flatValue;(also reduction -value)
//msFlat = 0 + flatValuesSum;
//value = (base + msFlat) * msMul; clamp(100,500)