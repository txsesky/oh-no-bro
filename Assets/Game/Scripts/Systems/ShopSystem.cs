using System;
using System.Collections.Generic;
using System.Linq;
using Game.Components;
using Game.Events;
using Game.Extensions;
using LeoEcsPhysics;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Game.Systems
{
    public class ShopSystem : IEcsRunSystem
    {
        [EcsWorld] private readonly EcsWorld _world = default;

        [EcsShared] private readonly SharedData _sharedData = default;

        [EcsFilter(typeof(TransformRef))] private readonly EcsFilter _transformGroup = default;

        [EcsFilter(typeof(OnTriggerEnterEvent))]
        private readonly EcsFilter _playerOnEnterTriggerEntities = default;

        [EcsFilter(typeof(OnTriggerExitEvent))]
        private readonly EcsFilter _playerOnExitTriggerEntities = default;

        [EcsPool] private readonly EcsPool<PlayerData> _playerDataPool = default;

        [EcsPool] private readonly EcsPool<OnTriggerEnterEvent> _onEnterTriggerEventPool = default;

        [EcsPool] private readonly EcsPool<OnTriggerExitEvent> _onExitTriggerEventPool = default;

        [EcsPool] private readonly EcsPool<ShopData> _shopPool = default;

        [EcsPool] private readonly EcsPool<TransformRef> _transformPool = default;

        private Dictionary<Transform, int> _transforms = new Dictionary<Transform, int>();

        public void Run(EcsSystems systems)
        {
            _transforms.Clear();

            foreach (var entity in _transformGroup)
            {
                _transforms.Add(_transformPool.Get(entity).Value, entity);
            }

            foreach (var entity in _playerOnEnterTriggerEntities)
            {
                ref var eventData = ref _onEnterTriggerEventPool.Get(entity);

                var playerTransform = eventData.collider.transform;
                var shopTransform = eventData.senderGameObject.transform;

                var a = _transforms.ContainsKey(playerTransform) ? _transforms[playerTransform] : -1;
                var b = _transforms.ContainsKey(shopTransform) ? _transforms[shopTransform] : -1;

                Trigger(a, b, () => MessageBus.ShopOpenUIEvent?.Invoke());
            }

            foreach (var entity in _playerOnExitTriggerEntities)
            {
                ref var eventData = ref _onExitTriggerEventPool.Get(entity);

                var playerTransform = eventData.collider.transform;
                var shopTransform = eventData.senderGameObject.transform;

                var a = _transforms.ContainsKey(playerTransform) ? _transforms[playerTransform] : -1;
                var b = _transforms.ContainsKey(shopTransform) ? _transforms[shopTransform] : -1;

                Trigger(a, b, () =>  MessageBus.ShopCloseUIEvent?.Invoke());
            }
        }

        private void Trigger(int a, int b, Action action)
        {
            if (a.IsNull() || b.IsNull())
                return;

            if (_playerDataPool.Has(a) && _shopPool.Has(b))
            {
                ref var playerData = ref _playerDataPool.Get(a);
                ref var shopData = ref _shopPool.Get(b);

                action?.Invoke();
            }

            if (_playerDataPool.Has(b) && _shopPool.Has(a))
            {
                ref var playerData = ref _playerDataPool.Get(b);
                ref var shopData = ref _shopPool.Get(a);
                
                action?.Invoke();
                
            }
        }
    }
}