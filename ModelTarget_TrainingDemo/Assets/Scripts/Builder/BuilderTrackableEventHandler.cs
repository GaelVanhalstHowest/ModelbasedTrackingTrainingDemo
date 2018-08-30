using UnityEngine;

//A custom trackable event handler.
//It activates certain gameobjects if the modeltarget has been found.
public class BuilderTrackableEventHandler : DefaultTrackableEventHandler
{
	public GameObject[] HideableObjects;

	protected override void OnTrackingFound()
	{
		foreach (var hideableObject in HideableObjects)
		{
			hideableObject.SetActive(true);
		}
	}

	protected override void OnTrackingLost()
	{
		foreach (var hideableObject in HideableObjects)
		{
			hideableObject.SetActive(false);
		}
	}
}