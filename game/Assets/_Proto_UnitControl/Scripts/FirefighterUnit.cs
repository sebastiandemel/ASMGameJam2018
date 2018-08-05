using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirefighterUnit : BaseUnit {

    public GameObject deathEffect;
    public GameObject waterSprayEffect;
    [SerializeField]
    public Animator anim;
    public AudioClip deathClip;
    public AudioClip waterClip;
    private AudioSource source;

    private Vector3 oldTarget;
    private float coolDownWater = .5f;

    private void Awake()
    {
        source = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    protected override void Update () {
        base.Update();        

        if (isSelected && sprayWater && waterAmount >0 && Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100, targetMask))
            {              
                ShootWater(hit.point);
            }
        }
        if (isSelected && Input.GetKeyDown(KeyCode.X))
        {
            sprayWater = !sprayWater;

            if (sprayWater == false)
            {
                waterSprayEffect.SetActive(false);
            }
        }

        if (isSelected && navMeshAgent.remainingDistance<=navMeshAgent.stoppingDistance)
        {
            SetAnimationState(0);
        }

        if (takingDamage)
        {
            TakeDamage(Time.deltaTime * damegeOverTimeRate);
        }
       

    }

    public override void Move(Vector3 destination)
    {       
        base.Move(destination);
        sprayWater = false;
        waterSprayEffect.SetActive(sprayWater);

        SetAnimationState(1);
        Debug.Log(navMeshAgent);

    }

    public override void ShootWater(Vector3 target)
    {
        if (Vector3.Distance(transform.position, target) < range)
        {
            oldTarget = target;
            Debug.Log("In Range");
            Vector3 lookAt = target - transform.position;
            transform.rotation = Quaternion.LookRotation(lookAt);

            StartCoroutine(WaterCooldown(target));

          
        }
        else
        {

           // waterSprayEffect.SetActive(false);
           // sprayWater = false;
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
        //Instantiate(deathEffect, transform.position, Quaternion.identity);

        deathEffect.SetActive(true);
        SetAnimationState(2);
        StartCoroutine(Dying());
    }

    void SetAnimationState(int stateIndex)
    {
        if (anim != null)
        {
            anim.SetInteger("State",stateIndex);
        }
    }

    IEnumerator Dying()
    {
        yield return new WaitForSeconds(4f);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fire"))
        {
            takingDamage = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Fire"))
        {
            takingDamage = false;
        }
    }

    IEnumerator WaterCooldown(Vector3 target)
    {
        while (waterAmount>0)
        {
            waterSprayEffect.SetActive(sprayWater);

            // Old extinguishing system
            #region

             Collider[] thingsOnFire = Physics.OverlapSphere(target, waterRadius, burnableMask);
             waterSprayEffect.SetActive(sprayWater);
             //Vector3 lookAt = target - transform.position;
             SpawnWater(target);
            // transform.rotation = Quaternion.LookRotation(lookAt);

             source.PlayOneShot(waterClip);

             if (thingsOnFire != null)
             {
                
                 for (int i = 0; i < thingsOnFire.Length; i++)
                 {

                    var tree = thingsOnFire[i].GetComponent<FireElement>();
                     if (tree != null)
                     {
                        tree.Healt -= 0.1f;                        
                     }

                 }
        
             }
             //waterAmount--;
            #endregion 

            yield return new WaitForSeconds(coolDownWater);
            
        }

        sprayWater = false;
        waterSprayEffect.SetActive(false);
    }
}
