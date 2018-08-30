using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldUIHider : MonoBehaviour
{
    public GameObject[] UIGameObjects;

    public const string HideUISpritePath = "Stefaan/Images/UI/EyeOpen";
    public const string UnhideUISpritePath = "Stefaan/Images/UI/EyeClosed";

    public static Sprite HideUISprite{get;private set;}
    public static Sprite UnhideUISprite{get;private set;}

    public Image HideStatusSprite;

    private void Start()
    {
        if (HideUISprite == null)
            HideUISprite = Resources.Load<Sprite>(HideUISpritePath);

        if (UnhideUISprite == null)
            UnhideUISprite = Resources.Load<Sprite>(UnhideUISpritePath);

        foreach (var uiObj in UIGameObjects)
            uiObj.SetActive(true);
    }

    public void ToggleUIVisibility()
    {
        if (UIGameObjects == null || UIGameObjects.Length == 0)
            return;

        bool currentVisible = UIGameObjects[0].activeSelf;

        if(currentVisible)
            HideStatusSprite.sprite = UnhideUISprite;
        else
            HideStatusSprite.sprite = HideUISprite;

        foreach (var uiObj in UIGameObjects)
            uiObj.SetActive(!currentVisible);
    }
}
