using System.Collections.Generic;
using UnityEngine;

//This object is the target to place the building blocks on.
public class TargetObject : MonoBehaviour 
{
	private static readonly List<TargetObject> ActiveTargets = new List<TargetObject>();

	public static readonly string ObjectUpdatedMessage = "A target object has been updated";

	public bool Completed { get; private set; }

	public string ModelName;
	private MeshRenderer[] _renderers = null;

	private ConnectionPointIndicator[] _indicators;

	/// <summary>
	/// Is the target currently active in the scene to be used to place an object on.
	/// </summary>
	/// <param name="modelName"></param>
	/// <returns></returns>
	public static TargetObject GetActiveTarget(string modelName)
	{
		foreach (var activeTarget in ActiveTargets)
		{
			if (activeTarget.ModelName == modelName)
			{
				return activeTarget;
			}
		}

		return null;
	}

	private void Awake()
	{
		_renderers = GetComponentsInChildren<MeshRenderer>();
		_indicators = GetComponentsInChildren<ConnectionPointIndicator>();

		UpdateIndicators();
	}

	private void OnEnable()
	{
		if(!Completed)
		{
			ActiveTargets.Add(this);
			Messenger.AddListener(PlaceableObject.ObjectUpdatedMessage, UpdateIndicators);
		}

		Messenger.Broadcast(ObjectUpdatedMessage);
	}

	private void OnDisable()
	{
		if (!Completed)
		{
			ActiveTargets.Remove(this);
			Messenger.RemoveListener(PlaceableObject.ObjectUpdatedMessage, UpdateIndicators);
		}

		Messenger.Broadcast(ObjectUpdatedMessage);
	}

	private void OnDestroy()
	{
		Messenger.RemoveListener(PlaceableObject.ObjectUpdatedMessage, UpdateIndicators);
	}

	private void UpdateIndicators()
	{
		//Show the indicators for the current viable targets.
		bool activateIndicators = !Completed && PlaceableObject.ActiveModelAvailable(ModelName);
		foreach (var indicator in _indicators)
		{
			indicator.gameObject.SetActive(activateIndicators);
		}
	}

	/// <summary>
	/// Used to mark that an object has been placed on it.
	/// </summary>
	public void Complete()
	{
		Completed = true;
		ActiveTargets.Remove(this);

		Messenger.RemoveListener(PlaceableObject.ObjectPlacedMessage, UpdateIndicators);
		Messenger.Broadcast(ObjectUpdatedMessage);
	}

	public void SetVisible(bool visible)
	{
		foreach (var renderer in _renderers)
		{
			renderer.enabled = visible;
		}
	}
}
