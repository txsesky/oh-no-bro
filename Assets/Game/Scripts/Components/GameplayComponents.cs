using UnityEngine;

namespace Game.Components {
	public struct HealthChangeData {
		public float Amount;
		public int By;
		public float Time;
	}

	public struct MovementDirectionData {
		public Vector2 Direction;
	}

	public struct ShopData { }

	public struct PlayerData {
		public int Id;
	}

	public struct TimerData {
		public float TimeLeft;
	}
	
	public struct ResourceData {
		public float Value;
	}
}