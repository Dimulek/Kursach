using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretTower : MoovingTower
{
    [SerializeField]
    private GameObject fireEffect;

    private void FixedUpdate()
    {
        if (getNearEnemy())
        {
            lookAt2D();
            if (isCanFire)
            {
                StartCoroutine(shootEffect());
                StartCoroutine(shootEnemy());
            }
        }
    }

    private IEnumerator shootEnemy()
    {
        isCanFire = false;
        enemyComponent.Damage(damage);
        yield return new WaitForSeconds(fireTransaction / 1f);
        isCanFire = true;
    }

    private IEnumerator shootEffect()
    {
        fireEffect.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        fireEffect.SetActive(false);
    }
}
