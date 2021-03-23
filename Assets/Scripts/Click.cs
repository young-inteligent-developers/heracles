using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class Click : MonoBehaviour
{
    public Button ClickButton;
    public int minAttack;
    public int maxAttack;

    string[] enemyName = { "Goblin", "Wolf", "Skeleton" };
    int[] enemyHp = { 50, 75, 100 };
    string currentEnemy;
    int currentEnemyHp = 0;

    int repeatEnemy = 0;
    int randEnemy = 0;

    public Slider slider;
    public float FillSpeed = 0.5f;
    private float hpProgress = 1;
    float hp = 0;

    void Start()
    {
        RandEnemy();
    }

    void Update()
    {
        if (slider.value > hpProgress)
            slider.value -= FillSpeed * Time.deltaTime;
    }

    public void AttackClick()
    {
        
        if (currentEnemyHp <= 0 && slider.value <= 0)
        {
            RandEnemy();
            Debug.Log("work");
        }
        else
        {
            int r = Random.Range(minAttack, maxAttack);

            currentEnemyHp -= r;

            IncrementHpProgress(currentEnemyHp / hp);

            //Debug.Log(r);
            Debug.Log(currentEnemy);
            Debug.Log(currentEnemyHp);
        }
    }

    public void RandEnemy()
    {
        while (randEnemy == repeatEnemy)
            randEnemy = Random.Range(0, enemyName.Length);

        repeatEnemy = randEnemy;

        currentEnemy = enemyName[randEnemy];
        currentEnemyHp = enemyHp[randEnemy];

        hp = currentEnemyHp;

        hpProgress = 1;
        slider.value = 1;
    }

    public void DieEnemy()
    {

    }

    public void IncrementHpProgress(float newProgress)
    {
        hpProgress = newProgress;
    }
}
