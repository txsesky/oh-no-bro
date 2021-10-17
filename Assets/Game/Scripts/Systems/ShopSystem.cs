using System;
using System.Collections.Generic;
using System.Linq;
using Game.Components;
using Game.Events;
using Game.Extensions;
using LeoEcsPhysics;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite.Unity.Ugui;
using UnityEngine;

namespace Game.Systems {
	sealed class ShopSystem : IEcsRunSystem {
		[EcsFilter(typeof(TransformRefData))]
		readonly EcsFilter _transformGroup = default;

		[EcsFilter(typeof(OnTriggerEnterEvent))]
		readonly EcsFilter _playerOnEnterTriggerEntities = default;

		[EcsFilter(typeof(OnTriggerExitEvent))]
		readonly EcsFilter _playerOnExitTriggerEntities = default;

		[EcsFilter(typeof(UIShopPanelRef))]
		readonly EcsFilter _uiShopPanelEntities = default;

		[EcsFilter(typeof(EcsUguiClickEvent))]
		readonly EcsFilter _clickEventEntities = default;
		
		readonly EcsPool<PlayerData> _playerDataPool = default;
		readonly EcsPool<OnTriggerEnterEvent> _onEnterTriggerEventPool = default;
		readonly EcsPool<OnTriggerExitEvent> _onExitTriggerEventPool = default;
		readonly EcsPool<ShopData> _shopPool = default;
		readonly EcsPool<TransformRefData> _transformPool = default;
		readonly EcsPool<UIShopPanelRef> _uiShopPanelPool = default;
		readonly EcsPool<UItemElmntRef> _uiShopItemPool = default;
		readonly EcsPool<EcsUguiClickEvent> _clickEventPool = default;

		readonly Dictionary<Transform, int> _transforms = new Dictionary<Transform, int>();

		public void Run(EcsSystems systems) {
			_transforms.Clear();

			foreach (var entity in _transformGroup) {
				_transforms.Add(_transformPool.Get(entity).Value, entity);
			}

			foreach (var entity in _playerOnEnterTriggerEntities) {
				ref var eventData = ref _onEnterTriggerEventPool.Get(entity);

				var playerTransform = eventData.collider.transform;
				var shopTransform = eventData.senderGameObject.transform;

				var a = _transforms.ContainsKey(playerTransform) ? _transforms[playerTransform] : -1;
				var b = _transforms.ContainsKey(shopTransform) ? _transforms[shopTransform] : -1;

				if (a.IsNull() || b.IsNull())
					return;

				foreach (var uiShopPanelEntity in _uiShopPanelEntities) {
					Trigger(a, b, () => _uiShopPanelPool.Get(uiShopPanelEntity).Value.Show());
				}
			}

			foreach (var entity in _playerOnExitTriggerEntities) {
				ref var eventData = ref _onExitTriggerEventPool.Get(entity);

				var playerTransform = eventData.collider.transform;
				var shopTransform = eventData.senderGameObject.transform;

				var a = _transforms.ContainsKey(playerTransform) ? _transforms[playerTransform] : -1;
				var b = _transforms.ContainsKey(shopTransform) ? _transforms[shopTransform] : -1;

				if (a.IsNull() || b.IsNull())
					return;

				foreach (var uiShopPanelEntity in _uiShopPanelEntities) {
					Trigger(a, b, () => _uiShopPanelPool.Get(uiShopPanelEntity).Value.Hide());
				}
			}

			foreach (var clickEventEntity in _clickEventEntities) {
				Debug.Log("click event");

				var clickEventData = _clickEventPool.Get(clickEventEntity);

				var parent = clickEventData.Sender.transform.parent;
				var btnEntity = _transforms.ContainsKey(parent)
					? _transforms[parent]
					: -1;

				if (btnEntity.IsNull())
					return;

				if (_uiShopItemPool.Has(btnEntity)) {
					Debug.Log("+MovementSpeed");
				}
			}

			//gcd
		}

		void Trigger(int a, int b, Action action) {
			if (_playerDataPool.Has(a) && _shopPool.Has(b)) {
				ref var playerData = ref _playerDataPool.Get(a);
				ref var shopData = ref _shopPool.Get(b);

				action?.Invoke();
			}

			if (_playerDataPool.Has(b) && _shopPool.Has(a)) {
				ref var playerData = ref _playerDataPool.Get(b);
				ref var shopData = ref _shopPool.Get(a);

				action?.Invoke();
			}
		}
	}
}