using Game.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Game.Systems
{
    public class ShopSystem : IEcsRunSystem
    {
        [EcsWorld] 
        private readonly EcsWorld _world = default;
        
        [EcsShared] 
        private readonly SharedData _sharedData = default;
        
        //[EcsFilter(typeof(PlayerData),typeof(OnEnterTriggerEventData))]
        //private readonly EcsFilter _playerOnEnterTriggerEntities = default;
        
        //[EcsFilter(typeof(PlayerData),typeof(OnExitTriggerEventData))]
        //private readonly EcsFilter _playerOnExitTriggerEntities = default;
        
        [EcsPool]
        private readonly EcsPool<PlayerData> _playerDataPool = default;

        //[EcsPool]
        //private readonly EcsPool<OnEnterTriggerEventData> _onEnterTriggerEventPool = default;
        
        //[EcsPool]
        //private readonly EcsPool<OnExitTriggerEventData> _onExitTriggerEventPool = default;

        [EcsPool]
        private readonly EcsPool<ShopData> _shopPool = default;

        public void Run(EcsSystems systems)
        {
            /*foreach (var playerEntity in _playerOnEnterTriggerEntities)
            {
                ref var playerData = ref _playerDataPool.Get(playerEntity);
                ref var triggerData = ref _onEnterTriggerEventPool.Get(playerEntity);

                if (!playerData.PhotonView.IsMine)
                    continue;

                if (!_shopPool.Has(triggerData.TriggerEntity))
                    continue;
                
                Debug.Log("ShopOpened");
            }

            foreach (var playerEntity in _playerOnExitTriggerEntities)
            {
                ref var playerData = ref _playerDataPool.Get(playerEntity);
                ref var triggerData = ref _onExitTriggerEventPool.Get(playerEntity);

                if (!playerData.PhotonView.IsMine)
                    continue;

                if (!_shopPool.Has(triggerData.TriggerEntity))
                    continue;
                
                Debug.Log("ShopClosed");
            }*/
        }

        public void Trigger(int a, int b)
        {
            if (_playerDataPool.Has(a) && _shopPool.Has(b))
            {
                ref var playerData = ref _playerDataPool.Get(a);
                ref var shopData = ref _shopPool.Get(b);

                if (playerData.PhotonView.IsMine)
                {
                    Debug.Log("ShopOpened");
                }
            }

            if (_playerDataPool.Has(b) && _shopPool.Has(a))
            {
                ref var playerData = ref _playerDataPool.Get(b);
                ref var shopData = ref _shopPool.Get(a);
                
                if (playerData.PhotonView.IsMine)
                {
                    Debug.Log("ShopOpened");
                }
            }
        }
    }
}