using System.Collections.Generic;

namespace Game.Components{
	public struct NameComponent { public string Value; }
	public struct IDComponent { public string Value; }
	public struct ItemComponent { }
	public struct ItemGoldCostComponent { public string Value; }

	public struct AbilityComponent { }
	public struct AbilityTextureNameComponent { public string Value; }
	public struct AbilityBehaviorComponent { public byte Value; }
	public struct AbilityScriptFileComponent { public string Value; }
	public struct AbilityUseManaCostComponent { public string Value; }
	public struct AbilityUpgradeGoldCostComponent { public string Value; }
	public struct AbilitySpecialComponent { }

	public struct DynamicBufferElementComponent {
		public int OwnerEntity;
		public List<int> Entities;
	}
	public struct VarTypeComponent { public VarType Value; }
	public struct StringDataComponent { public string Value; }
	public struct DescriptionComponent { public string Value; }
	public struct AbilityLevelComponent { public int Value; }
	public struct AbilityMaxLevelComponent { public int Value; }

	public enum VarType {
		FIELD_INTEGER,
		FIELD_FLOAT
	}

	public enum Target {
		CASTER,
		TARGET,
		UNIT,
		ATTACKER
	}

	public enum ModifierProperty {
		HEALTH_FLAT_BONUS,
		HEALTH_PERCENT_BONUS,
		HEALTH_REGEN_FLAT_BONUS,
		HEALTH_REGEN_PERCENT_BONUS,
		MOVEMENT_SPEED_FLAT_BONUS,
		MOVEMENT_SPEED_PERCENT_BONUS,
		ATTACK_SPEED_FLAT_BONUS,
		ATTACK_SPEED_PERCENT_BONUS,
		ATTACK_DAMAGE_FLAT_BONUS,
		ATTACK_DAMAGE_PERCENT_BONUS
	}

	public enum AbilityBehavior {
		HIDDEN = 1 << 0, //Can be owned by a unit but can't be cast and won't show up on the HUD.
		PASSIVE = 1 << 1, //Cannot be cast like above but this one shows up on the ability HUD.
		NO_TARGET = 1 << 2, //Doesn't need a target to be cast, ability fires off as soon as the button is pressed.
		UNIT_TARGET = 1 << 3, //Needs a target to be cast on.

		POINT =
			1 << 4, //Can be cast anywhere the mouse cursor is (if a unit is clicked it will just be cast where the unit was standing).

		AOE =
			1 << 5, //Draws a radius where the ability will have effect. Kinda like POINT but with a an area of effect display.

		NOT_LEARNABLE =
			1 << 6, //Probably can be cast or have a casting scheme but cannot be learned (these are usually abilities that are temporary like techie's bomb detonate).
		CHANNELLED = 1 << 7, //Channeled ability. If the user moves or is silenced the ability is interrupted.
		ITEM = 1 << 8, //Ability is tied up to an item.
		TOGGLE = 1 << 9, //Can be insta-toggled.
		DIRECTIONAL = 1 << 10, //Has a direction from the hero, such as miranas arrow or pudge's hook.
		IMMEDIATE = 1 << 11, //Can be used instantly without going into the action queue.
		AUTOCAST = 1 << 12, //Can be cast automatically.
		NOASSIST = 1 << 13, //Ability has no reticle assist.
		AURA = 1 << 14, //Ability is an aura.  Not really used other than to tag the ability as such.
		ATTACK = 1 << 15, //Is an attack and cannot hit attack-immune targets.

		DONT_RESUME_MOVEMENT =
			1 << 16, //Should not resume movement when it completes. Only applicable to no-target, non-immediate abilities.
		ROOT_DISABLES = 1 << 17, //Cannot be used when rooted
		UNRESTRICTED = 1 << 18, //Ability is allowed when commands are restricted

		IGNORE_PSEUDO_QUEUE =
			1 << 19, //Can be executed while stunned, casting, or force-attacking. Only applicable to toggled abilities.
		IGNORE_CHANNEL = 1 << 20, //Can be executed without interrupting channels.
		DONT_CANCEL_MOVEMENT = 1 << 21, //Doesn't cause certain modifiers to end, used for courier and speed burst.
		DONT_ALERT_TARGET = 1 << 22, //Does not alert enemies when target-cast on them.

		DONT_RESUME_ATTACK =
			1 << 23, //Ability should not resume command-attacking the previous target when it completes. Only applicable to no-target, non-immediate abilities and unit-target abilities.
		NORMAL_WHEN_STOLEN = 1 << 24, //Ability still uses its normal cast point when stolen.
		IGNORE_BACKSWING = 1 << 25, //Ability ignores backswing pseudoqueue.
		RUNE_TARGET = 1 << 26, //Targets runes.
	}
}