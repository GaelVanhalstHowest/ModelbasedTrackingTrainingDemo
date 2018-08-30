using UnityEngine;

//This class copies the settings of one camera over to another
[RequireComponent(typeof(Camera))]
public class CameraCopier : MonoBehaviour 
{
	private Camera _camera = null;
	public Camera SourceCamera;

	private void Awake()
	{
		_camera = GetComponent<Camera>();	
	}

	void OnPreRender () 
	{
		Transform sourceTransform = SourceCamera.transform;
		transform.position = sourceTransform.position;
		transform.rotation = sourceTransform.rotation;

		_camera.nearClipPlane = SourceCamera.nearClipPlane;
		_camera.farClipPlane = SourceCamera.farClipPlane;
		_camera.orthographic = SourceCamera.orthographic;
		_camera.orthographicSize = SourceCamera.orthographicSize;
		_camera.worldToCameraMatrix = SourceCamera.worldToCameraMatrix;
		_camera.projectionMatrix = SourceCamera.projectionMatrix;
	}
}
