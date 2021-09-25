using System.Collections.Generic;
using UnityEngine;

namespace Game.Components
{
    public struct DisposeData
    {
    }

    public struct HierarchyData
    {
        public int Parent;
        public int FirstChild;
        public int NextSibling;
        public int PrevSibling;
        public int ChildrenCount;
    }

    public struct SetParentData
    {
        public int Entity;
        public Vector3 LocalTranslation;
    }


    public struct InputData
    {
        public bool IsPerformedToMove;
        public Vector2 PointerPosition;
        public Vector2 Axis;
    }

    public struct TransformRef
    {
        public Transform Value;
    }

    public struct RigidbodyRef
    {
        public Rigidbody Value;
    }

    public struct SphereColliderRef
    {
        public SphereCollider Value;
    }
    
    public struct CharacterControllerRef
    {
        public CharacterController Value;
    }
}