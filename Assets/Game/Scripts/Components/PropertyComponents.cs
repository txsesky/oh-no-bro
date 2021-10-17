namespace Game.Components {
	public struct HealthData {
		public float BaseValue;
		public float FlatModifierValue;
		public float PercentModifierValue;
		public float Value;
		public float Current;
	}

	public struct HealthRegenData {
		public float BaseValue;
		public float FlatModifierValue;
		public float PercentModifierValue;
		public float Value;
	}

	public struct MovementSpeedData {
		public float BaseValue;
		public float FlatModifierValue;
		public float PercentModifierValue;
		public float Value;
		public float ValueMeterPerSec;
	}

	public struct AttackSpeedData {
		public float BaseValue;
		public float FlatModifierValue;
		public float PercentModifierValue;
		public float Value;
		public float ValueSecPerAttack;
	}

	public struct AttackDamageData {
		public float BaseValue;
		public float FlatModifierValue;
		public float PercentModifierValue;
		public float Value;
		public int NumberOfDice;
		public int NumberOfSidesPerDie;
	}

	

	public struct ResourceExtractionData {
		public float BaseValue;
		public float FlatModifierValue;
		public float PercentModifierValue;
		public float Value;
	}

	public struct ResourceStealData {
		public float BaseValue;
		public float FlatModifierValue;
		public float PercentModifierValue;
		public float Value;
	}
}