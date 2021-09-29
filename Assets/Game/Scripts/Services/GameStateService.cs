using Game.Components;
using WebSocketSharp;

namespace Game.Services {
    sealed class GameStateService {
        public string GameMode;

        public GameStateService(string gameMode = null) {
            GameMode = !gameMode.IsNullOrEmpty() ? gameMode : Idents.GameModes.SinglePlayer;
        }
    }
}