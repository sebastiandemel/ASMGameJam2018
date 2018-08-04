using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// The base Script for Base Unit where other units are derived from
/// </summary>

public class BaseUnit : MonoBehaviour {

    public string unitName;
    public float unitSpeed;
    public float maxHealth;
    public float waterAmount;
    public float range;
    public float waterRadius;
    public GameObject waterShotPrefab;
    public bool isSelected;
    public bool sprayWater;
    public LayerMask targetMask;
    public LayerMask burnableMask;
    public GameObject selectionObject;
    public GameObject moveEffect;

    protected float currentHealth;
    protected NavMeshAgent navMeshAgent;

    protected virtual void Update()
    {
        selectionObject.SetActive(isSelected);
        if(moveEffect!=null)
            moveEffect.SetActive(navMeshAgent.isStopped);
    }
    private void Start() {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = unitSpeed;
    }

    public virtual void Move(Vector3 destination)
    {
        if (navMeshAgent != null)
        {
            navMeshAgent.destination = destination;
        }
    }

    public virtual void ShootWater(Vector3 target)
    {

    }

    public virtual void TakeDamage(float damageAmount) { }


    protected virtual void OnDeath() { }

    protected void SpawnWater(Vector3 waterPoint)
    {
        GameObject waterInstance = Instantiate(waterShotPrefab, waterPoint, Quaternion.identity);
        waterInstance.transform.localScale = new Vector3(waterRadius, waterRadius, waterRadius);
        StartCoroutine(DestroyWaterInstance(waterInstance));
    }

    protected IEnumerator DestroyWaterInstance(GameObject objectInstance)
    {
        yield return new WaitForSeconds(1f);
        Destroy(objectInstance);
    }

}
