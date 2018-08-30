using UnityEngine;

public class WorldUIToCamera : MonoBehaviour
{
    public bool FreezeUI = false;

    public void ToggleFreezeUI()
    {
        if(FreezeUI)
            FreezeUI = false;
        else
            FreezeUI = true;
    }

    void Update()
    {
        if (!FreezeUI)
            transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward, Camera.main.transform.up);
    }
}
