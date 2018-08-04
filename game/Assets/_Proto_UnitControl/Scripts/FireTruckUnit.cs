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
        if (isSelected && deploymentUnit!=null && deploymentAmount > 0 && Input.GetKeyDown(KeyCode.D))
        {
            DeployUnit();
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
    }

    public override void ShootWater(Vector3 target)
    {
        if (Vector3.Distance(transform.position, target) < range)
        {
            Collider[] thingsOnFire = Physics.OverlapSphere(target,wateringRadius,burnableMask); 
            

        }
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
