using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridElement : MonoBehaviour {

	public Vector2Int GridPosition;

    private bool isDead;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		var health = ForestManager.instance.GetHealt(GridPosition.x, GridPosition.y);
		var childCount = gameObject.transform.childCount;
		
		if(health == 0.0f){			
			if(!gameObject.transform.GetChild(2).gameObject.activeInHierarchy){
				gameObject.transform.GetChild(0).gameObject.SetActive(false); // Idle
				gameObject.transform.GetChild(1).gameObject.SetActive(false); // Burned

				gameObject.transform.GetChild(2).gameObject.SetActive(true); // Burned
                if (!isDead)
                {
                    OnDeath();
                    isDead = true;
                }
			}
		}
		else if(health == 1.0f) {
			if(!gameObject.transform.GetChild(0).gameObject.activeInHierarchy){
				gameObject.transform.GetChild(1).gameObject.SetActive(false); // Burning
				gameObject.transform.GetChild(2).gameObject.SetActive(false); // Burned

				gameObject.transform.GetChild(0).gameObject.SetActive(true); // Idle
			}
		}
		else{			
			if(!gameObject.transform.GetChild(1).gameObject.activeInHierarchy){
				gameObject.transform.GetChild(0).gameObject.SetActive(false); // Idle
				gameObject.transform.GetChild(2).gameObject.SetActive(false); // Burned

				gameObject.transform.GetChild(1).gameObject.SetActive(true); // Burning
			}
		}

	}
    void OnDeath()
    {
        //GameManager.manager.FailedTrees++;
        //GameManager.manager.CheckState();
    }
}
