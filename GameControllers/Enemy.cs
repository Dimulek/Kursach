using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private GameObject debufIcon;

    [SerializeField]
    private GameObject HealthIcon;

    [SerializeField]
    private float Health;

    [SerializeField]
    private float MoveSpeed;

    [SerializeField]
    private float speedDebuf;

    [SerializeField]
    private int moneyEnemy;

    [SerializeField]
    private int damageEnemy;

    [SerializeField]
    private SpriteRenderer enemySprite;

    [SerializeField]
    private Animator animator;

    private GameManager _gameManager;

    private Vector3 NextNode;
    private int numberOfNode = 0;
    private int maxSizeWay = 0;

    private float nowHealth = 99999;
    private float nowMoveSpeed;

    private bool isSpawned = false;

    private bool isStun = false;
    private bool canBeStunned = true;
    public bool isDebuf = false;

    public int minWaveCount;
    public int waveScoreCost;

    // Start is called before the first frame update
    private void Awake()
    {
        _gameManager = GameManager.InstanceManager;
        nowHealth = Health;
        nowMoveSpeed = MoveSpeed;
    }
    void Start()
    {
        maxSizeWay = _gameManager.pathToTarget.Count;
        NextNode = _gameManager.pathToTarget[numberOfNode];
        StartCoroutine(spawnEnemy());
    }

    IEnumerator spawnEnemy()
    {
        yield return new WaitForSeconds(0.1f);
        isSpawned = true;
    }

    void ChooseNextNode()
    {
        if (numberOfNode + 1 == maxSizeWay)
        {
            Damage(Health);
            return;
        }
        Vector3 nowNode = NextNode;
        NextNode = _gameManager.pathToTarget[++numberOfNode];
        if(nowNode.y == NextNode.y)
        {
            animator.SetInteger("Corner", 0);
            if (nowNode.x < NextNode.x)
                enemySprite.flipX = false;
            else if (nowNode.x > NextNode.x)
                enemySprite.flipX = true;
        }
        else
        {
            enemySprite.flipX = true;
            if (nowNode.y > NextNode.y)
                animator.SetInteger("Corner",2);
            else
                animator.SetInteger("Corner", 1);

        }

    }

    void MoveNextNode()
    {
        if (!isStun)
            transform.position = Vector2.MoveTowards(transform.position, NextNode, nowMoveSpeed * Time.deltaTime);
    }

    public void Damage(in float damage)
    {
        if (!isSpawned)
            return;
        nowHealth -= damage;
        if (nowHealth <= 0)
        {
            Destroy(gameObject);
            return;
        }
        HealthIcon.transform.localScale = new Vector3(nowHealth / Health, 1, 1);
    }

    public void StunEnemy(in int damage, in float stunTime, in float stunDuration)
    {
        if (canBeStunned)
            StartCoroutine(GetStun(stunTime, stunDuration));

        Damage(damage);
    }

    public void beginDebuf(in int damage, in float damageDuration, ref int damageCount)
    {
        debufIcon.SetActive(true);
        nowMoveSpeed = speedDebuf;
        getDebuf(damage, damageDuration, ref damageCount);
    }

    private void getDebuf(in int damage, in float damageDuration, ref int damageCount)
    {
        if (isDebuf = damageCount > 0)
            StartCoroutine(debuffer(damage, damageDuration, damageCount));
        else
        {
            nowMoveSpeed = MoveSpeed;
            debufIcon.SetActive(false);
        }
    }

    private IEnumerator debuffer(int damage, float damageDuration, int damageCount)
    {
        Damage(damage);
        --damageCount;
        yield return new WaitForSeconds(damageDuration * 1f);
        getDebuf(damage, damageDuration, ref damageCount);
    }

    public IEnumerator GetStun(float stunTime, float stunDuration)
    {
        isStun = true;
        canBeStunned = false;
        yield return new WaitForSeconds(stunTime);
        StartCoroutine(removeStun(stunDuration));
        isStun = false;
    }

    public IEnumerator removeStun(float stunDuration)
    {
        yield return new WaitForSeconds(stunDuration);
        canBeStunned = true;
    }

    private void FixedUpdate()
    {
        if (transform.position == NextNode)
            ChooseNextNode();
        MoveNextNode();
    }

    public bool isCanSpawn(ref int scoreLeft)
    {
        if (scoreLeft - waveScoreCost > 0)
        {
            scoreLeft -= waveScoreCost;
            return true;
        }
        return false;
    }

    private void OnDestroy()
    {
        _gameManager.removeEnemy(gameObject);
        if (numberOfNode != GameManager.InstanceManager.pathToTarget.Count - 1)
            GameManager.InstanceManager.moneyChange(moneyEnemy);
        else
            GameManager.InstanceManager.healthChange(-damageEnemy);
    }
}
