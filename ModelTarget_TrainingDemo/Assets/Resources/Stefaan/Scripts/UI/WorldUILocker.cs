using System;
using UnityEngine;
using UnityEngine.UI; 

public class WorldUILocker : MonoBehaviour
{
    public const string LockMsg = "Lock obj";
    public const string LockedSpritePath = "Stefaan/Images/UI/LockClosed";
    public const string UnlockMsg = "Unlock obj";
    public const string UnlockedSpritePath = "Stefaan/Images/UI/LockOpen";

    public static Sprite LockedSprite
    {
        get;
        private set;
    }
    public static Sprite UnlockedSprite
    {
        get;
        private set;
    }

    public Text LockStatusTxt;
    public Image LockStatusSprite;

    public bool IsLocked
    {
        get
        {
            return _isLocked;
        }
        private set{}
    }
    private bool  _isLocked;

    public event EventHandler LockStatusChanged;
    protected virtual void OnLockStatusChanged()
    {
        EventHandler handler = LockStatusChanged;
        if (handler != null)
            handler(this, new EventArgs());
    }

    void Start()
    {
        if (LockedSprite == null)
            LockedSprite = Resources.Load<Sprite>(LockedSpritePath);

        if (UnlockedSprite == null)
            UnlockedSprite = Resources.Load<Sprite>(UnlockedSpritePath);

        _isLocked = false;
        if(LockStatusTxt != null)
            LockStatusTxt.text = LockMsg;
        if(LockStatusSprite != null)
            LockStatusSprite.sprite = LockedSprite;    
    }

    public void ToggleLock()
    {
        _isLocked = !_isLocked;

        if(_isLocked)
        {
            if (LockStatusTxt != null)
                LockStatusTxt.text = UnlockMsg;
            if(LockStatusSprite != null)
                LockStatusSprite.sprite = UnlockedSprite;
        }else{
            if(LockStatusTxt != null)
                LockStatusTxt.text = LockMsg;
            if(LockStatusSprite != null)
                LockStatusSprite.sprite = LockedSprite;     
        }

        OnLockStatusChanged();
    }
}
