using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTruckUnit : BaseUnit {



    public override void ShootWater(Vector3 target)
    {
        if (Vector3.Distance(transform.position, target) < range)
        {
            Collider[] thingsOnFire = Physics.OverlapSphere(target,wateringRadius);
            Gizmos.DrawSphere(target,wateringRadius);
        }
    }
}
