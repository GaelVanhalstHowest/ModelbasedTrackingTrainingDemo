using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuilderUI : MonoBehaviour 
{
	public Transform Tray;
	public PlaceableObjectButton PartButton;
	public RawImage PreviewImage;

	private readonly Dictionary<PlaceableObjectButton, string> _placeableObjectButtons = new Dictionary<PlaceableObjectButton, string>();

	private void Awake () 
	{
		PartButton.gameObject.SetActive(false);
		SetPreviewImage(null);
	}

	public void Initialize(PlaceableObject[] placeableObjects, GuideLine guideLinePrefab)
	{
		//We create all the buttons to select the different parts
		foreach (var placeableObject in placeableObjects)
		{
			PlaceableObjectButton button = Instantiate(PartButton, PartButton.transform.parent);
			button.gameObject.SetActive(true);
			button.Initialize(placeableObject, Tray, guideLinePrefab, ()=> OnButtonClicked(button));

			_placeableObjectButtons[button] = placeableObject.ModelName;
		}
	}

	private void OnButtonClicked(PlaceableObjectButton clickedButton)
	{
		//Makes that only the button of the selected object is shown as selected
		PlaceableObject previewedObject = null;
		foreach (var button in _placeableObjectButtons)
		{
			if(button.Key == clickedButton && button.Key.Toggled)
			{
				previewedObject = button.Key.Prefab;
			}

			//Deactivate other buttons if needed
			if (button.Key != clickedButton && button.Key.Toggled)
			{
				button.Key.Toggle(false);
			}
		}

		Texture2D previewTexture = null;
		if(previewedObject!=null)
		{
			previewTexture = previewedObject.Preview;
		}
		SetPreviewImage(previewTexture);
	}

	//An optional feature to show the selected object in the UI.
	//By example for when the placeable object isn't always on screen.
	private void SetPreviewImage(Texture2D previewImage)
	{
		if(PreviewImage!=null)
		{
			PreviewImage.enabled = previewImage != null;
			PreviewImage.texture = previewImage;
		}
	}

	/// <summary>
	/// This marks the objects that will have a hint arrow
	/// </summary>
	/// <param name="neededObjects"></param>
	public void SetNeededObjects(List<string> neededObjects)
	{
		foreach (var button in _placeableObjectButtons)
		{
			button.Key.ShowHint(neededObjects.Contains(button.Value));
		}
	}
}
