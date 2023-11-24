using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketTower : MoovingTower
{
    [SerializeField]
    private GameObject rocket;
    [SerializeField]
    private Sprite emptyTower;
    [SerializeField]
    private Sprite fullTower;
    [SerializeField]
    private SpriteRenderer rocketRender;

    [SerializeField]
    private float zPositionRocket;

    void FixedUpdate()
    {
        if (getNearEnemy())
        {
            lookAt2D();
            if (isCanFire)
                StartCoroutine(shootEnemy());
        }
    }

    private IEnumerator shootEnemy()
    {
        isCanFire = false;
        rocketRender.sprite = emptyTower;
        Vector3 vec = transform.position;
        vec.z += zPositionRocket;
        GameObject rock = Instantiate(rocket, vec, transform.rotation);
        rock.GetComponent<Rocket>().targetVector(nearest, damage);
        rock.SetActive(true);
        yield return new WaitForSeconds(fireTransaction / 1f);
        rocketRender.sprite = fullTower;
        isCanFire = true;
    }
}
