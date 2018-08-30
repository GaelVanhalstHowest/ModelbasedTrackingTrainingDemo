using UnityEngine;

//This class creates a transform that is always at a certain normalized position of the screen.
public class AnchoredTransform : MonoBehaviour 
{
	public Vector2 AnchorPosition = new Vector2(0.5f, 0.5f);

	void Update () 
	{
		Camera camera = Camera.main;
		float depth = camera.WorldToViewportPoint(transform.position).z;

		transform.position = camera.ViewportToWorldPoint(new Vector3(AnchorPosition.x, AnchorPosition.y, depth));
	}
}
