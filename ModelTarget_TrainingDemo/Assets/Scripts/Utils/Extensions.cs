using UnityEngine;

public static class Extensions
{
	/// <summary>
	/// Set the layers from this and transform and its children
	/// </summary>
	/// <param name="transform"></param>
	/// <param name="layer"></param>
	public static void SetLayerRecursive(this Transform transform, int layer)
	{
		transform.gameObject.layer = layer;

		int childCount = transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			transform.GetChild(i).SetLayerRecursive(layer);
		}
	}
}
