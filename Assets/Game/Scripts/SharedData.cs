using Game.Services;

namespace Game {
	sealed class SharedData {
		public TimeService Time;
		public GoogleDocsService GoogleDocs;
		public GameStateService GameState;
	}
}