using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burnable : MonoBehaviour {

    public GameObject burningEffect;
    public bool isOnFire;
    public float fireHealth;
    public float moistureAmount;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        burningEffect.SetActive(isOnFire);
	}

    void OnDeath()
    {
        GameManager.manager.FailedTrees++;
        GameManager.manager.CheckState();
    }
}
