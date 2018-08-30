using UnityEngine;

//This is an animated line between two points
[RequireComponent(typeof(LineRenderer))]
public class GuideLine : MonoBehaviour
{	
	public float ScrollSpeed = -3.0f;
	public Transform Start;
	public Transform End;

	private LineRenderer _lineRenderer;
	private float _offset;

	void Awake () 
	{
		_lineRenderer = GetComponent<LineRenderer>();
		_lineRenderer.useWorldSpace = true;
	}
	
	void LateUpdate () 
	{
		_lineRenderer.enabled = Start != null && Start.gameObject.activeInHierarchy && 
			End != null && End.gameObject.activeInHierarchy;

		if(_lineRenderer.enabled)
		{
			Vector3 startPos = Start.position;
			Vector3 endPos = End.position;

			int pointCount = _lineRenderer.positionCount;
			float delta = 1.0f / (pointCount - 1);
			for (int i = 0; i < pointCount; i++)
			{
				_lineRenderer.SetPosition(i, Vector3.Lerp(startPos, endPos, i * delta));
			}

			_offset += Time.deltaTime * ScrollSpeed;
			_lineRenderer.material.mainTextureOffset = new Vector2(_offset % 1, 0);
			_lineRenderer.material.mainTextureScale = new Vector3(_lineRenderer.sharedMaterial.mainTextureScale.x, 1f);
		}
	}
}
