using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootWater : MonoBehaviour {

    public float range;
    public float waterRadius;
    public float waterAmount;
    public float shootCooldown;
    public LayerMask fireMask;
    public GameObject waterPrefab;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {            
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 50, fireMask))
            {
                Debug.Log("MouseDown");
                WaterShoot(hit.point);
            }
        }
	}

    void WaterShoot(Vector3 target)
    {
        Debug.Log("Water splashed"+ target);
        
        if (Vector3.Distance(transform.position, target) < range && waterAmount > 0)
        {
            Debug.Log("Water splashed");
            Collider[] thingsOnFire = Physics.OverlapSphere(target,waterRadius,fireMask);
            SpawnWater(target);
            if (thingsOnFire != null)
            {
                int firesPutout=0;
                for (int i = 0; i < thingsOnFire.Length; i++)
                {
                    if (thingsOnFire[i].GetComponent<FireTree>() != null)
                    {
                        thingsOnFire[i].GetComponent<FireTree>().isOnFire = false;
                        firesPutout++;
                    }
                }
                Debug.Log("Fires put out " + firesPutout);
            }
            waterAmount --;
        }
    }

    void SpawnWater(Vector3 waterPoint)
    {
        GameObject waterInstance = Instantiate(waterPrefab, waterPoint, Quaternion.identity);
        waterInstance.transform.localScale = new Vector3(waterRadius,waterRadius,waterRadius);
        StartCoroutine(DestroyWaterInstance(waterInstance));
    }

    IEnumerator DestroyWaterInstance(GameObject objectInstance)
    {
        yield return new WaitForSeconds(1f);
        Destroy(objectInstance);
    }


}
