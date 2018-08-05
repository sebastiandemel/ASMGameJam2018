using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirechopperUnit : BaseUnit {

    private Vector3 targetPoint;
    private Vector3 coordinates;

    public bool watered;

	
	// Update is called once per frame
	protected override void Update () {
        if (watered)
        {
            transform.Translate(transform.forward * unitSpeed * Time.deltaTime);
            return;
        }
        transform.position = Vector3.MoveTowards(transform.position,targetPoint,Time.deltaTime*unitSpeed);

        coordinates = new Vector3(transform.position.x, 8, transform.position.z);
        if (Vector3.Distance(coordinates, targetPoint) < 2)
        {
            Ray ray = new Ray(transform.position,-transform.up);
            RaycastHit hit;
            if (Physics.Raycast(ray,out hit, 50, targetMask))
            {
                ShootWater(hit.point);
            }            
        }
	}

    public override void Move(Vector3 destination)
    {        
        targetPoint = new Vector3(destination.x, 8, destination.z);
    }

    public override void ShootWater(Vector3 target)
    {
        
        Collider[] thingsOnFire = Physics.OverlapSphere(target, waterRadius, burnableMask);   

        SpawnWater(target);
        transform.rotation = Quaternion.LookRotation(target, transform.up);

        if (thingsOnFire != null)
        {
            int firesPutout = 0;
            for (int i = 0; i < thingsOnFire.Length; i++)
            {
                if (thingsOnFire[i].GetComponent<Burnable>() != null)
                {
                    if (thingsOnFire[i].GetComponent<Burnable>().isOnFire)
                        thingsOnFire[i].GetComponent<Burnable>().isOnFire = false;
                    else
                    {
                        thingsOnFire[i].GetComponent<Burnable>().moistureAmount = 100;
                    }

                    firesPutout++;
                }

            }
            Debug.Log("Fires put out " + firesPutout);
        }
        waterAmount--;
        watered = true;
    }
}
