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
    float hpProgress = 1;
    float hp = 0;
    bool attack = true;

    [Header("Attack Text Points")]
    public GameObject GeneratePointsCointener;
    public GameObject AttackPointText;
    public GameObject AttackPointBlocker;

    [Header("Stage")]
    public Button StagesButton;
    public TextMeshProUGUI FightWithBoss;
    public Animator FightWithBossAnimation;
    public TextMeshProUGUI StageText;
    public int stage = 14; // Public is test, change to private later
    int maxStage = 15;

    [Header("Gold")]
    public TextMeshProUGUI GoldText;

    [Header("Enemys")]
    public GameObject EnemySprite;
    public Sprite[] EnemiesSprites = new Sprite[3];
    public int[] EnemiesHp = new int[3];
    int currentEnemyHp = 0;
    int repeatEnemy = 0;
    int randEnemy = 0;

    [Header("Bosses")]
    public Sprite[] BossesSprites = new Sprite[1];
    public int[] BossesHp = new int[1];
    bool boss = false;

    void Awake()
    {
       
    }

    void Start()
    {
        RandEnemy();
        ActuallyHP();
        ActuallyStage();
    }

    void Update()
    {
        if (slider.value > hpProgress)
            slider.value -= FillSpeed * Time.deltaTime;

        if (currentEnemyHp <= 0 && slider.value <= 0 && attack == true)
            AttackClick();

        DelatePoint();
        ActuallyGold();
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
            Debug.Log("Actualy Lvl : " + GameManager.instance.level);
        }
    }

    void RandEnemy()
    {
        EnemySprite.SetActive(true);
        ClickButton.enabled = true;
        attack = true;

        while (randEnemy == repeatEnemy)
            randEnemy = Random.Range(0, EnemiesSprites.Length);

        repeatEnemy = randEnemy;

        //currentEnemy = enemyNameString[randEnemy];
        currentEnemyHp = EnemiesHp[randEnemy];

        hp = currentEnemyHp;
        ActuallyHP();

        hpProgress = 1;
        slider.value = 1;


        EnemySprite.GetComponent<SpriteRenderer>().sprite = EnemiesSprites[randEnemy];
    }

    void DieEnemy()
    {
        EnemySprite.SetActive(false);
        ClickButton.enabled = false;
        attack = false;

        //Enemy.GetComponent<Animator>().enabled = true; // Die Animation

        stage++;
        ActuallyStage();

        GameManager.instance.gold += 10;

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

            FightWithBossAnimation.enabled = true;

            stage = 0;
            GameManager.instance.gold += 40;
            //GameManager.instance.level++;       
        }

        StageText.text = stage.ToString() + "/" + maxStage.ToString();
    }

    void ActuallyGold()
    {
        GoldText.text = GameManager.instance.gold.ToString();
    }

    public void FightBoss()
    {
        //FightWithBoss.enabled = false;
        boss = false;

        FightWithBossAnimation.enabled = false;
        FightWithBoss.fontSize = 90;

        currentEnemyHp = BossesHp[GameManager.instance.level];

        hp = currentEnemyHp;
        ActuallyHP();

        hpProgress = 1;
        slider.value = 1;

        EnemySprite.GetComponent<SpriteRenderer>().sprite = BossesSprites[GameManager.instance.level];

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
