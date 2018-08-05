using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager manager;
        
    public int maxFailedTrees;
    private int failedTrees;  

    public int firesActive;

    public UIManager uiManager;
    public AudioManager audioManager;

    public GameObject forest;

    public int FailedTrees
    {
        get
        {
            return failedTrees;
        }

        set
        {
            failedTrees = value;
        }
    }

    private void Awake()
    {
        manager = this;
    }

    // Use this for initialization
    void Start() {
        firesActive = FindObjectsOfType(typeof(FireElement)).Length;
    }

    // Update is called once per frame
    void Update() {

    }
    public void CheckState()
    {
       
        if (FailedTrees >= maxFailedTrees)
        {
            LevelFail();
        }

        if (firesActive<=0)
        {
            Debug.Log("Level Check");
            LevelVictory();
        }

    }

    public void LevelVictory()
    {
        Debug.Log("Level Clear");
        uiManager.OpenScreen(uiManager.victoryScreen);
    }
    public void LevelFail()
    {
        uiManager.OpenScreen(uiManager.failScreen);
    }
}
