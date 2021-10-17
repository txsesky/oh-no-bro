namespace Game {
	public static class Idents {
		public static class Worlds {
			public const string Events = "events";
			public const string UiEvents = "ui-events";
		}

		public static class Layers {
			public const int Ui = 5;
		}

		public static class GameModes {
			public const string SinglePlayer = "single-player";
			public const string Client = "client";
		}
		
		public static class Paths {
			public const string AbilityDatabaseJsonPath = "config/data/ability_database.json";
			public const string ItemDatabaseJsonPath = "config/data/item_database.json";
		}
		
		public static class Urls {
			public const string AbilityDatabaseJsonUrl = "https://raw.githubusercontent.com/txsesky/gold-rush-database/master/ability_database.json";
			public const string ItemDatabaseJsonUrl = "https://raw.githubusercontent.com/txsesky/gold-rush-database/master/item_database.json";
		}
	}
}