using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CycleTutorial : MonoBehaviour
{
    private Button _previousButton;
    private TextMeshProUGUI _nextButtonText;

    private int _tutorialStep;
    
    void Start()
    {
        _previousButton = transform.GetChild(0).GetChild(4).GetComponent<Button>();
        _nextButtonText = transform.GetChild(0).GetChild(5).GetChild(0).GetComponent<TextMeshProUGUI>();
        
        _tutorialStep = 0;
        _previousButton.interactable = false;
        
        SwitchStep();
    }

    public void ClickNext()
    {
        _tutorialStep += 1;
        CheckOptions();
    }

    public void ClickPrevious()
    {
        _tutorialStep -= 1;
        CheckOptions();
    }

    void CheckOptions()
    {
        _previousButton.interactable = _tutorialStep >= 1;

        _nextButtonText.text = _tutorialStep >= 3 ? "Finished" : "Next";

        if (_tutorialStep >= 4)
        {
            gameObject.SetActive(false);
            return;
        }
        
        SwitchStep();
    }

    void SwitchStep()
    {
        for (int i = 0; i < 4; i++)
        {
            transform.GetChild(0).GetChild(i).gameObject.SetActive(i == _tutorialStep);
        }
    }
}
