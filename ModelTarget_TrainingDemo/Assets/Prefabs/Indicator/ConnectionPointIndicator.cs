using UnityEngine;

public class ConnectionPointIndicator : MonoBehaviour
{
    public GameObject normalContainer;
    public GameObject correctContainer;

	private void Start()
	{
		LineRenderer[] lines = GetComponentsInChildren<LineRenderer>();

		foreach(var line in lines)
		{
			float scale = Mathf.Abs(line.transform.lossyScale.x);
			line.widthMultiplier *= scale;
		}
	}

	public void SetCorrect(bool correct)
    {
        if(normalContainer != null)
            normalContainer.SetActive(!correct);

        if(correctContainer != null)
            correctContainer.SetActive(correct);
    }
}
