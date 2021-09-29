using Game.Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace Game.Systems {
	sealed class CameraSpawnSystem : IEcsInitSystem {
		readonly EcsWorld _world = default;
		readonly EcsPool<TransformRefData> _transformPool = default;
		readonly EcsPool<CameraRefData> _cameraPool = default;
		readonly EcsPool<WorldCameraTag> _worldCameraPool = default;
		readonly EcsPool<UICameraTag> _uiCameraPool = default;

		public void Init(EcsSystems systems) {
			var wCamEnt = _world.NewEntity();
			var uCamEnt = _world.NewEntity();

			ref var wCamData = ref _cameraPool.Add(wCamEnt);
			wCamData.Value = Camera.main;

			ref var wTransData = ref _transformPool.Add(wCamEnt);
			wTransData.Value = wCamData.Value.transform;

			_worldCameraPool.Add(wCamEnt);

			ref var uCamData = ref _cameraPool.Add(uCamEnt);
			foreach (var camera in Camera.allCameras) {
				if (camera.gameObject.layer == Idents.Layers.Ui) {
					uCamData.Value = camera;
					break;
				}
			}

			ref var uTransData = ref _transformPool.Add(uCamEnt);
			uTransData.Value = uCamData.Value.transform;

			_uiCameraPool.Add(uCamEnt);
		}
	}
}