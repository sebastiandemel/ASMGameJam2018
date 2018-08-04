using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTruckUnit : BaseUnit {
    
    public GameObject deathEffect;
    public GameObject waterSprayEffect;

    public GameObject deploymentUnit;
    public Transform deploymentPoint;
    public int deploymentAmount;

    public float deploymentCooldown;

    public AudioClip moveClip, deployClip;

    private float currCooldown;
    private AudioSource source;

    protected override void Update()
    {
        base.Update();
        if (currCooldown <= deploymentCooldown)
        {
            currCooldown += Time.deltaTime;
            return;
        }
        if (isSelected && deploymentUnit!=null && deploymentAmount > 0 && Input.GetKeyDown(KeyCode.C))
        {
            DeployUnit();
        }

        if (isSelected && Input.GetKeyDown(KeyCode.X))
        {
            sprayWater = !sprayWater;
            if (sprayWater==false)
            {
                waterSprayEffect.SetActive(false);
            }
        }
        if (sprayWater && Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 50, targetMask))
            {
                ShootWater(hit.point);
            }
        }
        
    }

    private void DeployUnit()
    {
        GameObject unitInstance = Instantiate(deploymentUnit,deploymentPoint.position,deploymentPoint.rotation);

        currCooldown = deploymentCooldown;
        if (deployClip != null)
        {
            source.PlayOneShot(deployClip);
        }
        deploymentAmount--;
    }

    public override void Move(Vector3 destination)
    {
        base.Move(destination);
        sprayWater = false;
        waterSprayEffect.SetActive(false);       
    }

    public override void ShootWater(Vector3 target)
    {
        
        if (Vector3.Distance(transform.position, target) < range)
        {
            Debug.Log("In Range");
            Collider[] thingsOnFire = Physics.OverlapSphere(target, waterRadius, burnableMask);
            waterSprayEffect.SetActive(sprayWater);
            Vector3 lookAt = target - transform.position;
            SpawnWater(target);
            transform.rotation = Quaternion.LookRotation(lookAt);
           
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
           // StartCoroutine(SprayCooldown(target));
        }
        else
        {
            StopCoroutine(SprayCooldown(target));
           // navMeshAgent.stoppingDistance = 0;
        }
    }

    IEnumerator SprayCooldown(Vector3 target)
    {
       
        if (!sprayWater)
            yield return null;

        yield return new WaitForSeconds(1f);
        ShootWater(target);
    }

    public override void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        if (currentHealth <= 0)
        {
            OnDeath();
        }
    }

    protected override void OnDeath()
    {
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
