using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> towerTypes = new List<GameObject>();
    [SerializeField]
    private List<int> towerCost = new List<int>();
    [SerializeField]
    private List<Text> towerCostDisplay = new List<Text>();
    [SerializeField]
    private Border border;
    [SerializeField]
    private Transform enemyParent;
    [SerializeField]
    protected int moneyStart;
    [SerializeField]
    protected int healthStart;
    [SerializeField]
    private Text moneyDisplay;
    [SerializeField]
    private Text healthDisplay; 
    [SerializeField]
    protected Text waveCountDisplay;
    [SerializeField]
    protected Text moneyToUpgrade;
    [SerializeField]
    protected Text moneyToSale;
    [SerializeField]
    protected GameObject gameInterface;
    [SerializeField]
    private GameObject menuInterface;
    [SerializeField]
    private GameObject deathInterface;
    [SerializeField]
    private AStarAlgorithm aStarAlgorithm;
    [SerializeField]
    protected List<Enemy> enemyTypes = new List<Enemy>();
    [SerializeField]
    protected SQLiteControll sqliteControll;

    protected Action methodGetEnemies;

    private Point selectedPoint;
    private bool isDied = false;
    private int money = 0;
    private int health = 0;


    protected bool isCanSpawn = true;
    protected bool isWaveBegin = false;
    protected bool isEnemyCanGoToTarget = true;
    protected int waveCount = 0;
    protected int wavesPassed = 0;
    protected Action deathMethod;

    public List<GameObject> enemies;
    public List<int> enemyToSpawn;
    public List<Vector2> pathToTarget;

    public static GameManager InstanceManager;

    private void Start()
    {
        Begin();
        displayTowerCost();
        moneyChange(moneyStart);
        healthChange(healthStart);
        someChanges();

        methodGetEnemies();

#if !UNITY_EDITOR
Resolution resolution = Screen.currentResolution;
Application.targetFrameRate = Mathf.RoundToInt((float)resolution.refreshRate);
#endif
    }

    private void displayTowerCost()
    {
        for(int i = 0; i < 4; ++i)
        {
            towerCostDisplay.ElementAt(i).text = towerCost.ElementAt(i).ToString();
        }
    }

    protected void someChanges()
    {
        pathToTarget = aStarAlgorithm.getPathResult();
        pathToTarget.Reverse();
        if (pathToTarget.Count == 0)
            isEnemyCanGoToTarget = false;
        else
            isEnemyCanGoToTarget = true;
    }

    public void clickOnPlane(in Point point)
    {
        border.BorderApear(point.gameObject.transform.position);
        selectedPoint = point;
        Tower tower;
        if ((tower = selectedPoint.getTower()) != null)
            displayCost(tower.getSellCost(), tower.getUpgradeCost());
        else displayCost(0, 0);
    }

    private void displayCost(in int sellCost, in int upgradeCost)
    {
        moneyToSale.text = sellCost.ToString();
        moneyToUpgrade.text = upgradeCost.ToString();
    }

    public void startWave()
    {
        if (pathToTarget.Count() == 0)
            return;
        if(!isWaveBegin)
            ++waveCount;
        isWaveBegin = true;
    }

    public IEnumerator spawnEnemy()
    {
        isCanSpawn = false;
        Vector3 vec = new Vector3(transform.position.x, transform.position.y, -100);
        GameObject newEnemy = Instantiate(enemyTypes[enemyToSpawn[0]].gameObject, vec, transform.rotation, enemyParent);
        enemyToSpawn.RemoveAt(0);
        enemies.Add(newEnemy);
        newEnemy.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        isCanSpawn = true;
    }

    public void setTower(int towerIndex)
    {
        if(selectedPoint != null)
        {
            Vector2 pointPosition = selectedPoint.gameObject.transform.position;
            if (((pathToTarget.Where(p => p == pointPosition).Count() == 0 && isWaveBegin) || !isWaveBegin))
                selectedPoint.setTower(towerTypes[towerIndex], towerCost[towerIndex]);
            if (!isWaveBegin)
                someChanges();
            if (selectedPoint.getTower() != null)
                displayCost(selectedPoint.getTower().getSellCost(), selectedPoint.getTower().getUpgradeCost());
        }
    }

    public void updateTower()
    {
        if (selectedPoint != null)
        {
            selectedPoint.upgradeTower();
            Tower tower;
            if ((tower = selectedPoint.getTower()) != null)
                displayCost(tower.getSellCost(), tower.getUpgradeCost());
        }
    }

    public void deleteTower()
    {
        if (selectedPoint != null)
        {
            displayCost(0,0);
            selectedPoint.sellTower();
            if (!isWaveBegin)
                someChanges();
        }
    }

    public bool moneyChange(in int moneyChange)
    {
        if (money + moneyChange < 0)
            return false;
        money += moneyChange;
        if(moneyDisplay != null)
            moneyDisplay.text = money.ToString();
        return true;
    }

    public void healthChange(in int healthChange)
    {
        health += healthChange;
        if (health < 0)
            health = 0;
        healthDisplay.text = health.ToString();
        if (health == 0 && !isDied)
            Death();
    }

    private void Death()
    {
        isDied = true;
        deathMethod();
        waveCountDisplay.text += wavesPassed.ToString();
        openAndCloseInterfaceses(ref gameInterface, ref deathInterface, true);
    }

    public void Pause() =>
        Time.timeScale = 0;

    public void Begin() =>
        Time.timeScale = 1;

    public void openAndCloseInterfaceses(ref GameObject closingInterface, ref GameObject openingInterface, bool isFreesing)
    {
        closingInterface.SetActive(false);
        openingInterface.SetActive(true);
        if (isFreesing)
            Pause();
        else
            Begin();
    }

    public void openMenu()
    {
        openAndCloseInterfaceses(ref gameInterface, ref menuInterface, true);
    }

    public void closeMenu()
    {
        openAndCloseInterfaceses(ref menuInterface, ref gameInterface, false);
    }

    public void removeEnemy(in GameObject deadEnemy) =>
        enemies.Remove(deadEnemy);

    public void exit()
    {
        Begin();
        MainMenu.moveScene("Accaunt");
    }

    public void restart()
    {
        Begin();
        SceneManager.LoadScene("Load");
    }
    
    private void FixedUpdate()
    {
        if (isWaveBegin)
        {
            if (enemyToSpawn.Count > 0 && isCanSpawn)
            {
                StartCoroutine(spawnEnemy());
                return;
            }
            if (enemies.Count == 0 && enemyToSpawn.Count == 0)
            {
                ++wavesPassed;
                isWaveBegin = false;
                someChanges();
                methodGetEnemies();
            }
        }
    }
}