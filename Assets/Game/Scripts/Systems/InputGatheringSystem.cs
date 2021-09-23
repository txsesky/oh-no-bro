using Game.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Systems
{
    public class InputGatheringSystem : IEcsPreInitSystem, IEcsRunSystem, IEcsDestroySystem, GameInput.IGameplayActions
    {
        [EcsFilter(typeof(InputData))]
        private readonly EcsFilter _inputGroup = default;
        
        [EcsPool] 
        private readonly EcsPool<InputData> _inputPool = default;
        
        private Vector2 _pointerPosition;
        private Vector2 _axis;
        private bool _isPerformedToMove;
        private GameInput _gameInput;
        
        public void PreInit(EcsSystems systems)
        {
            _gameInput = new GameInput();
            _gameInput.Gameplay.SetCallbacks(this);
            EnableGameplayInput();
        }

        public void Run(EcsSystems systems)
        {
            foreach (var inputEntity in _inputGroup)
            {
                ref var input = ref _inputPool.Get(inputEntity);
                input.IsPerformedToMove = _isPerformedToMove;
                input.PointerPosition = _pointerPosition;
                input.Axis = _axis;
            }

            //_isPerformedToMove = false; 
            _axis = Vector2.zero;
        }

        public void Destroy(EcsSystems systems)
        {
            DisableAllInput();
        }

        public void OnMove(InputAction.CallbackContext context) => 
            _axis = context.ReadValue<Vector2>();

        public void OnClickToMove(InputAction.CallbackContext context)
        {
            if(context.performed)
                _isPerformedToMove = true;
            if(context.canceled)
                _isPerformedToMove = false;
        }

        public void OnTrackPointerPosition(InputAction.CallbackContext context) =>
            _pointerPosition = context.ReadValue<Vector2>();
        
        public void EnableDialogueInput()
        {
            //gameInput.Menus.Enable();
            //gameInput.Dialogues.Enable();
            _gameInput.Gameplay.Disable();
        }

        public void EnableGameplayInput()
        {
            //gameInput.Menus.Disable();
            //gameInput.Dialogues.Disable();
            _gameInput.Gameplay.Enable();
        }

        public void EnableMenuInput()
        {
            //gameInput.Dialogues.Disable();
            //gameInput.Menus.Enable();
            _gameInput.Gameplay.Disable();
        }

        public void DisableAllInput()
        {
            //gameInput.Menus.Disable();
            _gameInput.Gameplay.Disable();
            //gameInput.Dialogues.Disable();
        }
    }
}