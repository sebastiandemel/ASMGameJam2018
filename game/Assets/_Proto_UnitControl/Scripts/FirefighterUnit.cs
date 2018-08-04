using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirefighterUnit : BaseUnit {

    public GameObject deathEffect;
    public GameObject waterSprayEffect;

    private Animator anim;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void Move(Vector3 destination)
    {
        base.Move(destination);
        
    }

    public override void ShootWater(Vector3 target)
    {
        if (Vector3.Distance(transform.position, target) < range)
        {
            Collider[] thingsOnFire = Physics.OverlapSphere(target, wateringRadius);
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
        
    }

    void SetAnimationState(int stateIndex)
    {
        if (anim != null)
        {
            anim.SetInteger("StateIndex",stateIndex);
        }
    }

    IEnumerator Dying()
    {

        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
