using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public GameObject victoryScreen;
    public GameObject failScreen;
    public GameObject helpPanel;
    public GameObject restartConfirmPanel;

    public GameObject popUpPanel;

    public Text popUpText;

	// Use this for initialization
	void Start () {
		
	}

    void SetPanels()
    {
        victoryScreen.SetActive(false);
        failScreen.SetActive(false);
        helpPanel.SetActive(false);
        restartConfirmPanel.SetActive(false);
        popUpPanel.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public void OpenScreen(GameObject panel)
    {
        panel.SetActive(true);
    }
    public void CloseScreen(GameObject panel)
    {
        panel.SetActive(false);
    }

    public void SetPopup(GameObject panel, string message, float closeTime)
    {
        panel.SetActive(true);
        popUpText.text = message;
        StartCoroutine(AutoClosePanel(panel,closeTime));
    }

    IEnumerator AutoClosePanel(GameObject panel, float closeTime)
    {
        yield return new WaitForSeconds(closeTime);
        panel.SetActive(false);
    }
}
