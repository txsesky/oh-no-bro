using Game.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Unity.Ugui;
using UnityEngine.UI;

namespace Game.Systems
{
    public class MainMenuSystem : IEcsInitSystem, IEcsDestroySystem
    {
        
        [EcsUguiNamed("HostButton")] private Button _hostButton;
        [EcsUguiNamed("JoinButton")] private Button _joinButton;
        
        public void Init(EcsSystems systems)
        {
            var world = systems.GetWorld();
            var filter = world.Filter<NetworkManagerComponent>().End();
            var weapons = world.GetPool<NetworkManagerComponent>();
            
            foreach (var entity in filter)
            {
                ref var networkManagerComponent = ref weapons.Get(entity);
                _hostButton.onClick.AddListener(networkManagerComponent.NetworkManager.StartServer);
                _joinButton.onClick.AddListener(networkManagerComponent.NetworkManager.StartClient);
            }
        }
        
        public void Destroy(EcsSystems systems)
        {
            _hostButton.onClick.RemoveAllListeners();
            _joinButton.onClick.RemoveAllListeners();
        }
    }
}