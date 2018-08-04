using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DeploymentManager : MonoBehaviour {

    public Transform deploymentPoint;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void DeployUnit(GameObject unit)
    {
        GameObject unitInstance = Instantiate(unit,deploymentPoint.position,deploymentPoint.rotation);
        NavMeshAgent agent = unitInstance.GetComponent<NavMeshAgent>();
        agent.destination = Vector3.forward;
    }
}
