using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.PlayerLoop.PreLateUpdate;

public class Tower : MonoBehaviour
{
    [SerializeField]
    private int sellCost;
    [SerializeField]
    private int upgradeCost;
    [SerializeField]
    private GameObject nextUpgrade;
    [SerializeField]
    protected float fireRadius;

    public int damage;
    public float fireTransaction;

    public void updateTower(ref GameObject tower)
    {
        if (upgradeCost <= 0)
            return;
        if (!GameManager.InstanceManager.moneyChange(-upgradeCost))
            return;
        tower = Instantiate(nextUpgrade, transform.position, transform.rotation, transform.parent);
        Destroy(gameObject);
    }

    public void sellTower() =>
        GameManager.InstanceManager.moneyChange(sellCost);

    public int getSellCost() =>
        sellCost;

    public int getUpgradeCost() =>
        upgradeCost;
}
