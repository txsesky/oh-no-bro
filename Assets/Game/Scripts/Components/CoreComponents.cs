using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Components {
	public struct DisposeData { }

	public struct HierarchyData {
		public int Parent;
		public int FirstChild;
		public int NextSibling;
		public int PrevSibling;
		public int ChildrenCount;
	}

	public struct SetParentData {
		public int Entity;
		public Vector3 LocalTranslation;
	}


	public struct InputData {
		public bool IsPerformedToMove;
		public Vector2 PointerPosition;
		public Vector2 Axis;
	}

	public struct TransformRefData {
		public Transform Value;
	}

	public struct RigidbodyRefData {
		public Rigidbody Value;
	}
	
	public struct SphereColliderRefData {
		public SphereCollider Value;
	}

	public struct CharacterControllerRefData {
		public CharacterController Value;
	}
	
	public struct CameraRefData {
		public Camera Value;
	}
	
	public struct WorldCameraTag { }
	
	public struct UICameraTag { }
}