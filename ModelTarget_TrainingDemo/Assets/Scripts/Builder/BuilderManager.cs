using System;
using System.Collections.Generic;
using UnityEngine;

public class BuilderManager: MonoBehaviour
{
	public BuilderUI BuilderUI = null;
	public PlaceableObject[] PlaceableObjects = null;
	public Step[] Steps = null;
	public GuideLine GuideLinePrefab = null;

	private int _currentStep;

	private void Awake()
	{
		foreach (var step in Steps)
		{
			foreach (var target in step.Targets)
			{
				target.gameObject.SetActive(false);
			}
		}

		BuilderUI.Initialize(PlaceableObjects, GuideLinePrefab);
		StartStep(0);

		Messenger.AddListener(PlaceableObject.ObjectPlacedMessage, OnObjectPlaced);
	}

	private void OnDestroy()
	{
		Messenger.RemoveListener(PlaceableObject.ObjectPlacedMessage, OnObjectPlaced);
	}

	private void OnObjectPlaced()
	{
		if (_currentStep < Steps.Length)
		{
			//1 Step might exist out of multiple objects, so we check if all objects are placed
			bool completed = true;
			foreach (var target in Steps[_currentStep].Targets)
			{
				if(!target.Completed)
				{
					completed = false;
					break;
				}
			}

			if(completed)
			{
				StartStep(_currentStep+1);
			}
			else
			{
				UpdateNeededObjects();
			}
		}
	}

	private void StartStep(int step)
	{
		_currentStep = step;

		if(step<Steps.Length)
		{
			foreach (var target in Steps[step].Targets)
			{
				target.gameObject.SetActive(true);
				target.SetVisible(true);
			}
		}

		UpdateNeededObjects();
	}

	private void UpdateNeededObjects()
	{
		//We check which objects need to placed for this step, so we can show hints.
		List<string> neededObjects = new List<string>();

		if (_currentStep < Steps.Length)
		{
			foreach (var target in Steps[_currentStep].Targets)
			{
				if (!target.Completed && !neededObjects.Contains(target.ModelName))
				{
					neededObjects.Add(target.ModelName);
				}
			}
		}

		BuilderUI.SetNeededObjects(neededObjects);
	}

	[Serializable]
	public struct Step
	{
		public TargetObject[] Targets;
	}
}