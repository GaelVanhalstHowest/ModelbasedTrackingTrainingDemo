using System.IO;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlaceableObject))]
public class PlaceableObjectEditorScript : Editor 
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		if(GUILayout.Button("CreateIcon"))
		{
			Object[] targets = serializedObject.targetObjects;

			foreach (var target in targets)
			{
				PlaceableObject placeableObject = target as PlaceableObject;
	
				Texture2D thumbnail = AssetPreview.GetAssetPreview(placeableObject.gameObject);
				byte[] bytes = thumbnail.EncodeToPNG();
				Directory.CreateDirectory(Application.dataPath + "/PreviewImages");
				string filePath = string.Format("PreviewImages/{0}.png", placeableObject.name);
				File.WriteAllBytes(string.Format("{0}/{1}", Application.dataPath, filePath), bytes);

				Texture2D texture = AssetDatabase.LoadAssetAtPath("Assets/" + filePath, typeof(Texture2D)) as Texture2D;
				placeableObject.Preview = texture;
			}
			serializedObject.UpdateIfRequiredOrScript();
		}
	}
}
