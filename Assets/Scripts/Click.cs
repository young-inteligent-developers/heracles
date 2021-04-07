using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Click : MonoBehaviour
{

    [Header("Attack Configuration Points")]
    public Button ClickButton;
    public int minAttack;
    public int maxAttack;

    [Header("HP Slider")]
    public Slider slider;
    public TextMeshProUGUI actuallyHP;
    public float FillSpeed = 0.5f;
    private float hpProgress = 1;
    private float hp = 0;
    private bool attack = true;

    [Header("Attack Text Points")]
    public GameObject GeneratePointsCointener;
    public GameObject AttackPointText;
    public GameObject AttackPointBlocker;

    [Header("Enemy")]
    public GameObject Enemy;
    public Sprite[] EnemyNameString = new Sprite[3];
    int[] enemyHp = { 100, 125, 150 };
    int currentEnemyHp = 0;
    int repeatEnemy = 0;
    int randEnemy = 0;

    [Header("Stage")]
    public Button StagesButton;
    public TextMeshProUGUI FightWithBoss;
    public TextMeshProUGUI StageText;
    int stage = 14;
    int maxStage = 15;
    int level = 0;

    [Header("Gold")]
    public int gold;
    public TextMeshProUGUI GoldText;

    [Header("Boss")]
    public Sprite[] Boss = new Sprite[1];
    int[] BoosHp = { 500 };
    bool boss = false;


    void Awake()
    {
       
    }

    void Start()
    {
        RandEnemy();
        ActuallyHP();
        ActuallyStage();
        ActuallyGold();
    }

    void Update()
    {
        if (slider.value > hpProgress)
            slider.value -= FillSpeed * Time.deltaTime;

        if (currentEnemyHp <= 0 && slider.value <= 0 && attack == true)
            AttackClick();

        DelatePoint();
    }

    public void AttackClick()
    {

        if (currentEnemyHp <= 0 && slider.value <= 0)
        {
            DieEnemy();
            Invoke("RandEnemy", 1);
        }
        else if (currentEnemyHp >= 0)
        {
            int r = Random.Range(minAttack, maxAttack);

            currentEnemyHp -= r;
            ActuallyHP();

            IncrementHpProgress(currentEnemyHp / hp);
            ShowRandAttackEnemy(r);

            //Debug.Log(r);
            Debug.Log("Actualy HP : " + currentEnemyHp);
            Debug.Log("Actualy Lvl : " + level);
        }
    }

    void RandEnemy()
    {
        Enemy.SetActive(true);
        ClickButton.enabled = true;
        attack = true;

        while (randEnemy == repeatEnemy)
            randEnemy = Random.Range(0, EnemyNameString.Length);

        repeatEnemy = randEnemy;

        //currentEnemy = enemyNameString[randEnemy];
        currentEnemyHp = enemyHp[randEnemy];

        hp = currentEnemyHp;
        ActuallyHP();

        hpProgress = 1;
        slider.value = 1;


        Enemy.GetComponent<SpriteRenderer>().sprite = EnemyNameString[randEnemy];
    }

    void DieEnemy()
    {
        Enemy.SetActive(false);
        ClickButton.enabled = false;
        attack = false;

        stage++;
        ActuallyStage();

        gold += 10;
        ActuallyGold();

        Debug.Log("Die");
    }

    void ShowRandAttackEnemy(int r)
    {
        GameObject att = Instantiate(AttackPointText, GeneratePointsCointener.transform);

        att.GetComponent<TextMeshProUGUI>().text = r.ToString();

    }

    void IncrementHpProgress(float newProgress)
    {
        hpProgress = newProgress;
    }

    void ActuallyHP()
    {
        if(currentEnemyHp > 0)
            actuallyHP.text = currentEnemyHp.ToString() + " HP";
        else
            actuallyHP.text = "Dead";
    }

    void ActuallyStage()
    {
        if (stage == maxStage)
        {
            FightWithBoss.enabled = true;
            StageText.enabled = false;
            StagesButton.enabled = true;

            boss = true;
        }
        else if (stage > maxStage && boss == false)
        {
            FightWithBoss.enabled = false;
            StageText.enabled = true;
            StagesButton.enabled = false;

            stage = 0;
            gold += 40;
            //level++;
        }

        StageText.text = stage.ToString() + "/" + maxStage.ToString();
    }

    void ActuallyGold()
    {
        GoldText.text = gold.ToString();
    }

    public void FightBoss()
    {
        //FightWithBoss.enabled = false;
        boss = false;

        currentEnemyHp = BoosHp[level];

        hp = currentEnemyHp;
        ActuallyHP();

        hpProgress = 1;
        slider.value = 1;

        Enemy.GetComponent<SpriteRenderer>().sprite = Boss[level];

        Debug.Log("Boss");
    }

    void DelatePoint()
    {
        foreach (Transform child in GeneratePointsCointener.GetComponent<Transform>())
        {
            if (child.GetComponent<Collider2D>().IsTouching(AttackPointBlocker.GetComponent<Collider2D>()))
            {
                Destroy(child.gameObject);
            }
        }   
    }
}
