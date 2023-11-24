using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    private Vector3 target = new Vector3(10, 10, 10);
    private int explosionDamage;
    private float explosionRadius = 3;

    public void targetVector(in Vector3 targetVec, in int damage)
    {
        target = new Vector3(targetVec.x, targetVec.y, transform.position.z);
        explosionDamage = damage;
    }

    private void FixedUpdate()
    {
        if (transform.position != target)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, 5f * Time.deltaTime);
        }
        else
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius, 1 << LayerMask.NameToLayer("Enemy"));
            for (int i = colliders.Count() - 1; i >= 0; --i)
            {
                colliders.ElementAt(i).gameObject.GetComponent<Enemy>().Damage(explosionDamage);
            }
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
