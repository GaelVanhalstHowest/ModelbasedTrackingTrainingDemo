using System;
using UnityEngine;
using UnityEngine.UI;

public class PlaceableObjectButton : MonoBehaviour 
{
	public Text Text;
	public Button Button;
	public Image HintImage;
	public RawImage PreviewImage;

	public Color UntoggledColor = Color.white;
	public Color ToggledColor = Color.gray;

	public bool Toggled { get; private set; }
	
	public PlaceableObject Prefab { get; private set; }
	private PlaceableObject _activeObject;
	private Transform _tray;
	private GuideLine _guideLinePrefab;
	private bool _showHint;

	public void Initialize(PlaceableObject placeableObject, Transform tray, GuideLine guideLinePrefab, Action onClicked)
	{
		_guideLinePrefab = guideLinePrefab;
		Prefab = placeableObject;
		_tray = tray;
		Text.text = placeableObject.ModelName;
		PreviewImage.texture = placeableObject.Preview;
		Button.onClick.AddListener(() => onClicked());
		Messenger.AddListener(PlaceableObject.ObjectPlacedMessage, OnObjectPlaced);
	}

	private void OnDestroy()
	{
		Messenger.RemoveListener(PlaceableObject.ObjectPlacedMessage, OnObjectPlaced);
	}

	private void OnObjectPlaced()
	{
		//If the selected object is placed, then make a new one
		if(Toggled && _activeObject.Placed)
		{
			CreatePlaceableObject();
		}
	}

	public void OnClicked()
	{
		Toggle(!Toggled);
	}

	/// <summary>
	/// Set if the button should be selected or not
	/// </summary>
	/// <param name="on"></param>
	public void Toggle(bool on)
	{
		Toggled = on;
		if (Toggled)
		{
			Button.targetGraphic.color = ToggledColor;
			CreatePlaceableObject();
			HintImage.gameObject.SetActive(false);
		}
		else
		{
			Button.targetGraphic.color = UntoggledColor;

			if (_activeObject != null)
			{
				Destroy(_activeObject.gameObject);
				_activeObject = null;
			}
		}

		UpdateHintIndicator();
	}

	private void CreatePlaceableObject()
	{
		if(_tray !=null)
		{
			_activeObject = Instantiate(Prefab, _tray, false);
			_activeObject.Initialize(_guideLinePrefab);
		}
	}

	/// <summary>
	/// Sets if there should be a hint for this button
	/// </summary>
	/// <param name="show"></param>
	public void ShowHint(bool show)
	{
		_showHint = show;
		UpdateHintIndicator();
	}

	private void UpdateHintIndicator()
	{
		//Show hint if needed and the object isn't currently selected.
		HintImage.gameObject.SetActive(_showHint && !Toggled);
	}
}
