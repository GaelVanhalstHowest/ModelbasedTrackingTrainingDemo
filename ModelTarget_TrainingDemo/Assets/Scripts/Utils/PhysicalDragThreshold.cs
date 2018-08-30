using UnityEngine;
using UnityEngine.EventSystems;

//Makes that dragging is based on screen size and not pixel size.
[RequireComponent(typeof(EventSystem))]
public class PhysicalDragThreshold : MonoBehaviour
{
	private const float InchToCm = 2.54f;

	[SerializeField]
	private float _dragThresholdCM = 0.5f;

	void Awake()
	{
		EventSystem eventSystem = GetComponent<EventSystem>();
		eventSystem.pixelDragThreshold = (int)(_dragThresholdCM * Screen.dpi / InchToCm);
	}
}