using System;
using System.Collections.Generic;
using Game.Components;
using Game.UI.Common;
using Leopotam.EcsLite;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.HUD {
	public class UITooltipPanel : UIPanel {
		[SerializeField]
		TMP_Text _name;
		[SerializeField]
		TMP_Text _level;
		[SerializeField]
		TMP_Text _itemGoldCost;
		[SerializeField]
		TMP_Text _abilityUpgradeGoldCost;
		[SerializeField]
		TMP_Text _abilityManaCost;
		[SerializeField]
		TMP_Text _description;
		[SerializeField]
		TMP_Text _special;

		public void Setup(int entity, EcsWorld ecsWorld) {
			_name.text = GetComponent<NameComponent>(ecsWorld, entity).Value;
			_level.text = GetComponent<AbilityLevelComponent>(ecsWorld, entity).Value.ToString();
			_itemGoldCost.text = GetComponent<ItemGoldCostComponent>(ecsWorld, entity).Value;
			_abilityUpgradeGoldCost.text = GetComponent<AbilityUpgradeGoldCostComponent>(ecsWorld, entity).Value;
			_abilityManaCost.text = GetComponent<AbilityUseManaCostComponent>(ecsWorld, entity).Value;
			_description.text = GetComponent<DescriptionComponent>(ecsWorld, entity).Value;

			var dynamicFilter = ecsWorld.Filter<DynamicBufferElementComponent>().Inc<AbilitySpecialComponent>().End();
			var dynamicPool = ecsWorld.GetPool<DynamicBufferElementComponent>();
			
			var namePool = ecsWorld.GetPool<NameComponent>();
			var stringPool = ecsWorld.GetPool<StringDataComponent>();

			foreach (var dynamicElmnt in dynamicFilter) {
				ref var dynElmntC = ref dynamicPool.Get(dynamicElmnt);
				if (dynElmntC.OwnerEntity == entity) {
					foreach (var dataEntity in dynElmntC.Entities) {
						ref var nameData = ref namePool.Get(dataEntity);
						_special.text += nameData.Value;
						ref var decs = ref stringPool.Get(dataEntity);
						_special.text += $": {decs.Value.Replace(' ', '/')}\n";
					}
					break;
				}
			}
		}

		T GetComponent<T>(EcsWorld ecsWorld, int entity) where T : struct {
			var compPool = ecsWorld.GetPool<T>();

			if (!compPool.Has(entity)) return new T();
			
			ref var comp = ref compPool.Get(entity);
			return comp;
		}
	}
}