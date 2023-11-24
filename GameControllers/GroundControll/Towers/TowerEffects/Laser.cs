using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private LaserTower laserTower;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer != 8)
            return;
        Enemy componentEnemy = collision.gameObject.GetComponent<Enemy>();
        if (!componentEnemy.isDebuf)
            laserTower.setDebuf(ref componentEnemy);
    }
}
