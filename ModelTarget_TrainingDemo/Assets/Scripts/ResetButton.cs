using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetButton : MonoBehaviour {

	private const float AnimationTime = 0.25f;

	public CanvasGroup LoadingPanel;

	private bool _triggered = false;

	private void Awake()
	{
		LoadingPanel.gameObject.SetActive(false);
	}

	public void ResetScene()
	{
		if(!_triggered)
		{
			_triggered = true;
			StartCoroutine(DoResetScene());
		}
	}

	public IEnumerator DoResetScene()
	{
		LoadingPanel.gameObject.SetActive(true);

		float time = 0;

		while (time< AnimationTime)
		{
			time += Time.deltaTime;
			LoadingPanel.alpha = time / AnimationTime;
			yield return null;
		}

		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
}
