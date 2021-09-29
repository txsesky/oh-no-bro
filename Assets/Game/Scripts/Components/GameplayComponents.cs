using System;
using Photon.Pun;
using UnityEngine;

namespace Game.Components {
    public struct HealthPointsData {
        public float BaseMax;
        public float ModifiedMax;
        public float Current;
        public StatModifier StatModifier;
    }

    public struct HealthRegenerationData {
        public float Base;
        public float Modified;
        public StatModifier StatModifier;
    }

    public struct MovementSpeedData {
        public float Base;
        public float Modified;
        public StatModifier StatModifier;
    }

    public struct AttackCooldownData {
        public float Base;
        public float Modified;
        public StatModifier StatModifier;
    }

    public struct DamageData {
        public float Base;
        public float Modified;
        public StatModifier StatModifier;
    }

    public struct GoldAmountData {
        public float Base;
        public float Modified;
        public StatModifier StatModifier;
    }

    public struct GoldMiningData {
        public float Base;
        public float Modified;
        public StatModifier StatModifier;
    }

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

    public struct ShopItemData {
        public string Name;
        public string Image;
        public int Level;
        public float Cost;
        public Stat[] Stats;
        public StatModifier[] StatsModifiers;
    }

    public struct StatModifier {
        public float AddVal;
        public float MultVal;
    }

    public enum Stat {
        HP,
        HPR,
        DMG,
        AC,
        MS,
        GM
    }
}