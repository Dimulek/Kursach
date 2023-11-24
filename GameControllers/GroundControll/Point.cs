using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour
{

    private GameObject tower;
    private Tower towerComponent;
    private void OnMouseDown()
    {
        GameManager.InstanceManager.clickOnPlane(this);
    }

    public void setTower(in GameObject Tower, in int costTower)
    {
        if (gameObject.layer == 7)
            return;
        if (!GameManager.InstanceManager.moneyChange(-costTower))
            return;
        gameObject.layer = 7;
        tower = Instantiate(Tower, new Vector3(transform.position.x, transform.position.y, transform.position.z + 0.9f), new Quaternion(0, 0, 180, 0), gameObject.transform);
        tower.SetActive(true);

        towerComponent = tower.GetComponent<Tower>();
    }

    public void upgradeTower()
    {
        if (tower is null)
            return;
        towerComponent.updateTower(ref tower);
        towerComponent = tower.GetComponent<Tower>();
    }

    public void sellTower()
    {
        if (gameObject.layer == 6 || tower == null)
            return;
        gameObject.layer = 6;
        towerComponent.sellTower();
        Destroy(tower);
        tower = null;
        towerComponent = null;
    }

    public Tower getTower() =>
        towerComponent;
}
