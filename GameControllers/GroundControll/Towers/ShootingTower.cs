using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingTower : Tower
{
    protected bool isCanFire = true;
    protected List<GameObject> nearEnemy = new List<GameObject>();

    protected bool getNearEnemy()
    {
        nearEnemy.Clear();
        foreach (Collider2D collider in Physics2D.OverlapCircleAll(transform.position, fireRadius, 1 << LayerMask.NameToLayer("Enemy")))
            nearEnemy.Add(collider.gameObject);
        return nearEnemy.Count > 0;
    }
}
