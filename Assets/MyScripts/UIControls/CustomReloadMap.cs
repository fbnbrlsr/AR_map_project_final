namespace Mapbox.Examples
{
	using Mapbox.Geocoding;
	using Mapbox.Unity.Map;
	using UnityEngine;
	using System.Collections;
    using Mapbox.Utils;

    public class CustomReloadMap : MonoBehaviour
	{	

		/*
		*	This class is a modified version of the original MapBox class for reloading the map.
		*	It is adjusted to support zooming using hand gestures.
		*/

		Camera _camera;
		Vector3 _cameraStartPos;
		static AbstractMap _map;

		ForwardGeocodeUserInput _forwardGeocoder;

		private HeroBuildingSelectionUserInput[] _heroBuildingSelectionUserInput;

		Coroutine _reloadRoutine;

		WaitForSeconds _wait;

		// Modifications
		[SerializeField] GameObject mapHolderObject;
		private float initHandDistance;
		float zoomSensitivity;

		void Awake()
		{	
			_camera = Camera.main;
			_cameraStartPos = _camera.transform.position;
			_map = FindObjectOfType<AbstractMap>();
			if(_map == null)
			{
				Debug.LogError("Error: No Abstract Map component found in scene.");
				return;
			}
			if(_forwardGeocoder != null)
			{
				_forwardGeocoder.OnGeocoderResponse += ForwardGeocoder_OnGeocoderResponse;
			}
			_heroBuildingSelectionUserInput = GetComponentsInChildren<HeroBuildingSelectionUserInput>();
			if(_heroBuildingSelectionUserInput != null)
			{
				for (int i = 0; i < _heroBuildingSelectionUserInput.Length; i++)
				{
					_heroBuildingSelectionUserInput[i].OnGeocoderResponse += ForwardGeocoder_OnGeocoderResponse;
				}
			}
			_wait = new WaitForSeconds(.0f);
		}

		// Added function
        void Start()
		{
			InputEventsInvoker.InputEventTypes.HandDoubleInputStart += OnHandZoomStart;
			InputEventsInvoker.InputEventTypes.HandDoubleInputCont += OnHandZoomCont;
			initHandDistance = 1f;

			zoomSensitivity = 0.05f;
		}

		void ForwardGeocoder_OnGeocoderResponse(ForwardGeocodeResponse response)
		{
			if (null != response.Features && response.Features.Count > 0)
			{
				int zoom = _map.AbsoluteZoom;
				_map.UpdateMap(response.Features[0].Center, zoom);
			}
		}

		void ForwardGeocoder_OnGeocoderResponse(ForwardGeocodeResponse response, bool resetCamera)
		{
			if (response == null)
			{
				return;
			}
			if (resetCamera)
			{
				_camera.transform.position = _cameraStartPos;
			}
			ForwardGeocoder_OnGeocoderResponse(response);
		}

		void Reload(float value)
		{
			if (_reloadRoutine != null)
			{
				StopCoroutine(_reloadRoutine);
				_reloadRoutine = null;
			}
			_reloadRoutine = StartCoroutine(ReloadAfterDelay(value));
		}

		IEnumerator ReloadAfterDelay(float zoom)
		{
			yield return _wait;
			_camera.transform.position = _cameraStartPos;
			_map.UpdateMap(_map.CenterLatitudeLongitude, zoom);
			_reloadRoutine = null;
		}

		// Added function
		public void OnHandZoomStart(Vector3 pos0, Quaternion rot0, Vector3 pos1, Quaternion rot1, GameObject targetObj)
		{	
			if(targetObj.transform.IsChildOf(mapHolderObject.transform))
			{
				initHandDistance = Vector3.Distance(pos0, pos1);
			}
		}

		// Added function
		public void OnHandZoomCont(Vector3 pos0, Quaternion rot0, Vector3 pos1, Quaternion rot1, GameObject targetObj)
		{	
			if(targetObj.transform.IsChildOf(mapHolderObject.transform))
			{
				float currDistance = Vector3.Distance(pos0, pos1);
				float deltaRatio = currDistance / initHandDistance;
				deltaRatio = 1f + (deltaRatio - 1f) * zoomSensitivity;
				initHandDistance = currDistance;

				if(Mathf.Abs(deltaRatio - 1f) > .1f) return;

				_map.UpdateMap(_map.CenterLatitudeLongitude, _map.Zoom * deltaRatio);
			}
		}

		// Added function
		public static float GetReferenceDistance()
		{	
			Vector2d unit = new Vector2d(1f, 0f);
			return Vector3.Distance(_map.GeoToWorldPosition(unit, true), _map.GeoToWorldPosition(Vector2d.zero, true));
		}

	}
}