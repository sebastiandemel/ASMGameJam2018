﻿using System.Collections;
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
    public float wateringRadius;
    public GameObject waterShotPrefab;
    public bool isSelected;

    private NavMeshAgent navMeshAgent;

    private void Start() {
        navMeshAgent = GetComponent<NavMeshAgent>();
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


    protected void OnDeath() { }
	
}
