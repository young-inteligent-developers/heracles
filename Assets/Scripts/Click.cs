using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Click : MonoBehaviour
{
    public int minAttack;
    public int maxAttack;

    string[] enemyName = { "Goblin", "Wolf", "Skeleton" };
    int[] enemyHp = { 20, 30, 40 };
    string currentEnemy;
    int currentEnemyHp = -1;

    int repeatEnemy = 0;
    int randEnemy = 0;

    public Slider slider;
    public float FillSpeed = 0.5f;
    private float hpProgress = 1;
    float hp = 0;
    void Start()
    {
        
    }

    void Update()
    {
        if (slider.value > hpProgress)
            slider.value -= FillSpeed * Time.deltaTime;
    }

    public void AttackClick()
    {
        
        if (currentEnemyHp <= 0)
        {
            RandEnemy();
            hp = currentEnemyHp;
        }

        int r = Random.Range(minAttack, maxAttack);

        currentEnemyHp -= r;
 
        IncrementHpProgress(currentEnemyHp/hp);

        Debug.Log(r);
        Debug.Log(currentEnemy);
        Debug.Log(currentEnemyHp);
    }

    public void RandEnemy()
    {
        while (randEnemy == repeatEnemy)
            randEnemy = Random.Range(0, enemyName.Length);

        repeatEnemy = randEnemy;

        currentEnemy = enemyName[randEnemy];
        currentEnemyHp = enemyHp[randEnemy];
    }

    public void IncrementHpProgress(float newProgress)
    {
        hpProgress = newProgress;

        if (hpProgress <= 0)
        {
            hpProgress = 1;
            slider.value = 1;
        }
    }
}
