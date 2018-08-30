using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//This is the object is the building block that the user can drag around and place to build up the target. 
public class PlaceableObject : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
	private static readonly List<PlaceableObject> ActiveObjects = new List<PlaceableObject>();

	public static readonly string ObjectUpdatedMessage = "A placeable object has been updated";
	public static readonly string ObjectPlacedMessage = "A placeable object has been placed";

	public static readonly int DefaultLayer = 0;
	public static readonly int OverlayLayer = 8;

	private const float LerpTime = 0.25f;
	private static readonly RaycastHit[] NonAllocRaycastHits = new RaycastHit[100];

	public string ModelName;
	public Transform GuideLineTransform;
	public bool Placed { get; private set; }

	public Texture2D Preview;

	private GuideLine _guideLinePrefab;
	private Transform _rootTranform;
	private GuideLine _guideLinePreview;

	private TargetObject _currentGuideLineTarget;

	private Vector3 _wantedPosition;
	private Quaternion _wantedRotation;
	private Vector3 _wantedScale;

	public Vector3 DefaultScale = Vector3.one;

	private Coroutine _lerpingCoroutine = null;
	private TargetObject _currentTarget = null;

	public void Initialize(GuideLine guideLinePrefab)
	{
		_guideLinePrefab = guideLinePrefab;
		foreach(var collider in GetComponentsInChildren<Collider>())
		{
			collider.enabled = true;
		}
	}

	/// <summary>
	/// Is the model currently active in the scene to be placed.
	/// </summary>
	/// <param name="modelName"></param>
	/// <returns></returns>
	public static bool ActiveModelAvailable(string modelName)
	{
		foreach (var activeObject in ActiveObjects)
		{
			if (activeObject.ModelName == modelName)
			{
				return true;
			}
		}
		return false;
	}

	private void Start()
	{		
		if (GuideLineTransform == null)
		{
			GuideLineTransform = transform;
		}
		UpdateGuideLine();
	}

	private void OnEnable()
	{
		ActiveObjects.Add(this);
		transform.localScale = DefaultScale;
		_rootTranform = new GameObject("root").transform;
		_rootTranform.parent = transform.parent;
		_rootTranform.localPosition = transform.localPosition;
		_rootTranform.localRotation = transform.localRotation;
		_rootTranform.localScale = transform.localScale;

		transform.parent = _rootTranform;

		transform.localScale = Vector3.zero;
		SetWantedTransform(Vector3.zero, Quaternion.identity, Vector3.one);
		LerpTransform();

		Messenger.AddListener(TargetObject.ObjectUpdatedMessage, UpdateGuideLine);
		Messenger.Broadcast(ObjectUpdatedMessage);

		//We set the object to an overlay layer, so that it is always in front of target objects
		transform.SetLayerRecursive(OverlayLayer);
	}

	private void OnDisable()
	{
		ActiveObjects.Remove(this);
		Destroy(_rootTranform.gameObject);

		if (_guideLinePreview!=null)
		{
			Destroy(_guideLinePreview.gameObject);
		}

		Messenger.RemoveListener(TargetObject.ObjectUpdatedMessage, UpdateGuideLine);
		
		Messenger.Broadcast(ObjectUpdatedMessage);
	}

	private void UpdateGuideLine()
	{
		//We set a line between the object and a possible target
		if (_currentGuideLineTarget == null || !_currentGuideLineTarget.isActiveAndEnabled 
			|| _currentGuideLineTarget.Completed)
		{
			TargetObject target = TargetObject.GetActiveTarget(ModelName);

			if (_guideLinePreview == null)
			{
				_guideLinePreview = Instantiate(_guideLinePrefab);
				_guideLinePreview.transform.SetLayerRecursive(OverlayLayer);
			}

			if (target != null)
			{
				_guideLinePreview.gameObject.SetActive(true);
				_guideLinePreview.Start = transform;
				_guideLinePreview.End = target.transform;
			}
			else
			{
				_guideLinePreview.gameObject.SetActive(false);
			}
			_currentGuideLineTarget = target;
		}
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		transform.parent = null;
	}

	public void OnDrag(PointerEventData eventData)
	{
		Camera camera = Camera.main;
		float depth = camera.WorldToScreenPoint(_rootTranform.position).z;

		Vector3 pointerPosition = eventData.position;
		pointerPosition.z = depth;

		SetWantedTransform(camera.ScreenToWorldPoint(pointerPosition), _rootTranform.rotation, _rootTranform.lossyScale);

		Ray ray = camera.ScreenPointToRay(pointerPosition);
		TargettingRaycast(ray);
	}

	//We check if the object is over a possible target.
	//If so we move it to the target.
	private void TargettingRaycast(Ray ray)
	{
		TargetObject targetObject = TargetRaycast(ray);

		if (targetObject != null)
		{
			SetWantedTransform(targetObject.transform.position, targetObject.transform.rotation, targetObject.transform.lossyScale);
		}

		if (_currentTarget != targetObject)
		{
			TargetObject previousTarget = _currentTarget;
			_currentTarget = targetObject;
			LerpTransform(previousTarget, targetObject);
		}

		if (_lerpingCoroutine == null)
		{
			transform.rotation = _wantedRotation;
			transform.position = _wantedPosition;
			transform.localScale = _wantedScale;
		}
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		if (_currentTarget == null)
		{
			transform.parent = _rootTranform;
			SetWantedTransform(Vector3.zero, Quaternion.identity, Vector3.one);
			LerpTransform();
		}
		else
		{
			PlaceOnTarget();
		}
	}

	//We apply the object on the chosen target
	private void PlaceOnTarget()
	{
		transform.parent = _currentTarget.transform;
		SetWantedTransform(Vector3.zero, Quaternion.identity, Vector3.one);
		enabled = false;
		_currentTarget.Complete();
		Placed = true;

		LerpTransform(null, _currentTarget);
		Messenger.Broadcast(ObjectPlacedMessage);
	}

	private TargetObject TargetRaycast(Ray ray)
	{
		int amountOfHits = Physics.RaycastNonAlloc(ray, NonAllocRaycastHits);

		for (int i = 0; i < amountOfHits; i++)
		{
			RaycastHit hit = NonAllocRaycastHits[i];

			TargetObject targetObject = hit.transform.GetComponent<TargetObject>();
			if (targetObject != null && targetObject.ModelName == ModelName && !targetObject.Completed)
			{
				return targetObject;
			}
		}

		return null;
	}

	private void SetWantedTransform(Vector3 position, Quaternion rotation, Vector3 scale)
	{
		_wantedPosition = position;
		_wantedRotation = rotation;
		_wantedScale = scale;
	}

	//A lerp system to smoothly move objects to amd from a target
	private void LerpTransform(TargetObject oldTarget = null, TargetObject newTarget = null)
	{
		if (_lerpingCoroutine != null)
		{
			StopCoroutine(_lerpingCoroutine);
		}

		_lerpingCoroutine = StartCoroutine(DoLerpTransform(LerpTime, oldTarget, newTarget));
	}

	private IEnumerator DoLerpTransform(float lerpTime, TargetObject oldTarget, TargetObject newTarget)
	{
		if (oldTarget != null)
		{
			oldTarget.SetVisible(true);
			transform.SetLayerRecursive(OverlayLayer);
		}

		Vector3 startPosition = transform.localPosition;
		Vector3 startScale = transform.localScale;
		Quaternion startRotation = transform.localRotation;

		float time = 0;
		while(time<=lerpTime)
		{
			time += Time.deltaTime;
			float t = time / lerpTime;


			transform.localPosition = Vector3.Lerp(startPosition, _wantedPosition, t);
			transform.localRotation = Quaternion.Slerp(startRotation, _wantedRotation, t);
			transform.localScale = Vector3.Lerp(startScale, _wantedScale, t);
			yield return null;
		}

		if (newTarget != null)
		{
			newTarget.SetVisible(false);
			transform.SetLayerRecursive(DefaultLayer);
		}

		_lerpingCoroutine = null;
	}
}
