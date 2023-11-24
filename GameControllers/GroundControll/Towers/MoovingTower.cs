using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class MoovingTower : ShootingTower
{
    protected Vector3 nearest;
    protected GameObject nearestEnemy;
    protected Enemy enemyComponent;

    private void Start() =>
        nearest = new Vector3(transform.position.x, transform.position.y - 1);

    protected void GetNearPoint()
    {
        if(nearEnemy.Count > 0)
        {
            GameObject newEnemy = nearEnemy.OrderBy(p => Vector3.Distance(transform.position, p.transform.position)).First();
            if (newEnemy == nearestEnemy)
                return;
            nearestEnemy = newEnemy;
            enemyComponent = nearestEnemy.GetComponent<Enemy>();
        }
    }

    protected void lookAt2D()
    {
        GetNearPoint();
        nearest = nearestEnemy.transform.position;
        Vector3 diff = nearest - transform.position;
        diff.Normalize();
        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
    }
}
