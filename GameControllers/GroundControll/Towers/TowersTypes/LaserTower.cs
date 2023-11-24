using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTower : ShootingTower
{
    private Animator animator;
    public int damageCount;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (getNearEnemy())
        {
            if (animator.GetInteger("RollingState") == 0)
            {
                animator.SetInteger("RollingState", 1);
            }
        }
        else
        if (animator.GetInteger("RollingState") == 1)
        {
            animator.SetInteger("RollingState", 0);

        }
    }

    public void setDebuf(ref Enemy enemyControll)
    {
        enemyControll.beginDebuf(damage, fireTransaction, ref damageCount);
    }
}
