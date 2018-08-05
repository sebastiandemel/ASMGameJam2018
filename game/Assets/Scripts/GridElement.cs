using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridElement : MonoBehaviour {

	public Vector2Int GridPosition;

	public float Health = 1.0f;

    private bool isDead = false;

	private void Awake() {
		Health = 1.0f;	
	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
		var fireElement = gameObject.GetComponent<FireElement>();

		if(fireElement != null)
		{
			if(fireElement.isDead)
			{
				Destroy(fireElement);
				Debug.Log("Fire destroyed");
			}
		}
	}

    void OnDeath()
    {
		Health = 0;
		isDead = true;
        //GameManager.manager.FailedTrees++;
        //GameManager.manager.CheckState();s
    }
}
