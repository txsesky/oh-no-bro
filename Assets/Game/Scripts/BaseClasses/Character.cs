using System;

namespace Game.BaseClasses
{
    [Serializable]
    public class Character
    {
        public string Name;
        public string Prefab;
        public float ColliderRadius;
        public float MovementSpeed;
    }
}