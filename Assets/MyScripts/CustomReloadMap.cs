namespace Mapbox.Examples
{
	using Mapbox.Geocoding;
	using UnityEngine.UI;
	using Mapbox.Unity.Map;
	using UnityEngine;
	using System;
	using System.Collections;
    using Mapbox.Utils;

    public class CustomReloadMap : MonoBehaviour
	{
		Camera _camera;
		Vector3 _cameraStartPos;
		static AbstractMap _map;

		[SerializeField]
		ForwardGeocodeUserInput _forwardGeocoder;

		[SerializeField]
		Slider _zoomSlider;

		private HeroBuildingSelectionUserInput[] _heroBuildingSelectionUserInput;

		Coroutine _reloadRoutine;

		WaitForSeconds _wait;

		// My stuff
		[SerializeField] GameObject mapParentObject;
		InputEventTypes inEvents;
		private float initHandDistance;
		private float initMapZoom;

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
			if (_zoomSlider != null)
			{
				_map.OnUpdated += () => { _zoomSlider.value = _map.Zoom; };
				_zoomSlider.onValueChanged.AddListener(Reload);
				_zoomSlider.value = _map.Zoom;
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

		void Start()
		{
			inEvents = InputEventsInvoker.InputEventTypes;
			if(inEvents != null)
			{
				inEvents.HandDoubleInputStart += OnHandZoomStart;
				inEvents.HandDoubleInputCont += OnHandZoomCont;
			}
			initHandDistance = 1f;
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


		public void OnHandZoomStart(Vector3 pos0, Quaternion rot0, Vector3 pos1, Quaternion rot1, GameObject targetObj)
		{	
			if(targetObj.transform.IsChildOf(mapParentObject.transform))
			{
				initHandDistance = Vector3.Distance(pos0, pos1);
				initMapZoom = _map.Zoom;
			}
		}

		public void OnHandZoomCont(Vector3 pos0, Quaternion rot0, Vector3 pos1, Quaternion rot1, GameObject targetObj)
		{	
			if(targetObj.transform.IsChildOf(mapParentObject.transform))
			{
				float zoomFactor = Vector3.Distance(pos0, pos1) / initHandDistance;			// TODO: adjust zoom speed
				_map.UpdateMap(_map.CenterLatitudeLongitude, initMapZoom * zoomFactor);
			}
		}

		public static float GetReferenceDistance()
		{	
			Vector2d unit = new Vector2d(1f, 0f);
			return Vector3.Distance(_map.GeoToWorldPosition(unit, true), _map.GeoToWorldPosition(Vector2d.zero, true));
		}

	}
}