using UnityEngine;
using UnityEngine.UI;

public class DemoManager : MonoBehaviour
{
    public Canvas MainCanvas;
    public GameObject[] Steps;

    public Button StartBtn;
    public Button NextStepBtn;
    public Button RestartBtn;

    private int _currentstep = 0;

	void Start()
    {
        ResetDemo();
	}

    public void StartDemo()
    {
		_currentstep = 0;
        StartBtn.interactable = false;
		NextStepBtn.interactable = true;
		RestartBtn.interactable = true;

        Steps[0].SetActive(true);
	}

    public void NextStep()
    {
        ++_currentstep;

        switch(_currentstep)
        {
            case 1:
            {
                Steps[1].SetActive(true);
            }break;
            case 2:
            {
                Steps[0].SetActive(false);
                Steps[1].SetActive(false);
                Steps[2].SetActive(true);

                StartBtn.interactable = false;
		        NextStepBtn.interactable = false;
		        RestartBtn.interactable = true;
            }break;
        }
	}

    public void ResetDemo()
    {
		_currentstep = 0;
        StartBtn.interactable = true;
		NextStepBtn.interactable = false;
		RestartBtn.interactable = false;

        foreach (var step in Steps)
            step.SetActive(false);
	}
}
