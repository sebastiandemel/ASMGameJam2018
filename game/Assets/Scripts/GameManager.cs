using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager manager;
        
    public int maxFailedTrees;
    private int failedTrees;

    private int firesActive;

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
        firesActive = forest.GetComponentsInChildren<Burnable>().Length;
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
        else if(firesActive>0)
        {
            LevelVictory();
        }

    }

    public void LevelVictory()
    {

    }
    public void LevelFail()
    {

    }
}
