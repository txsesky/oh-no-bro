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

    public struct InputData
    {
        public bool IsPerformedToMove;
        public Vector2 PointerPosition;
        public Vector2 Axis;
    }

    public struct LocalToParentData
    {
        public Matrix4x4 Value;

        public Vector3 Position
        {
            get => new Vector3(Value.m03, Value.m13, Value.m23);
            set
            {
                Value.m03 = value.x;
                Value.m13 = value.y;
                Value.m23 = value.z;
            }
        }

        public Vector3 Forward;
        public Vector3 Right;
        public Vector3 Up;
    }

    public struct LocalToWorldData
    {
        public Matrix4x4 Value;

        public Vector3 Position
        {
            get => new Vector3(Value.m03, Value.m13, Value.m23);
            set
            {
                Value.m03 = value.x;
                Value.m13 = value.y;
                Value.m23 = value.z;
            }
        }

        public Vector3 Forward;
        public Vector3 Right;
        public Vector3 Up;
    }

    public struct TransformRef
    {
        public Transform Value;
    }

    public struct Rigidbody2DRef
    {
        public Rigidbody2D Value;
    }

    public struct CircleCollider2DRef
    {
        public CircleCollider2D Value;
    }
}