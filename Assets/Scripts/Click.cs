using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Click : MonoBehaviour
{

    [Header("Attack Configuration Points")]
    public Button clickButton;
    public int minAttack;
    public int maxAttack;

    [Header("HP Slider")]
    public Slider slider;
    public TextMeshProUGUI actuallyHP;
    float fillSpeed = 0.5f;
    float hpProgress = 1;
    float hp = 0;
    bool attack = true;

    [Header("Attack Text Points")]
    public GameObject generatePointsCointener;
    public GameObject attackPointText;
    public GameObject attackPointBlocker;

    [Header("Stage")]
    public Button stagesButton;
    public TextMeshProUGUI fightWithBoss;
    public Animator fightWithBossAnimation;
    public TextMeshProUGUI stageText;
    public int stage = 14; // Public is test, change to private later
    int maxStage = 15;

    [Header("Gold")]
    public TextMeshProUGUI goldText;

    [Header("Enemys")]
    public GameObject enemyObject;
    public string[] enemiesNames = new string[3];
    //public Sprite[,] enemiesSprites = new Sprite[3, 3];
    public int[] enemiesHp = new int[3];
    int currentEnemyHp = 0;
    int repeatEnemy = 0;
    int randEnemy = 0;

    [Header("Bosses")]
    public Sprite[] bossesSprites = new Sprite[1];
    public int[] bossesHp = new int[1];
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
            slider.value -= fillSpeed * Time.deltaTime;

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
            enemyObject.GetComponent<Animator>().SetTrigger("Attack");

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
        enemyObject.SetActive(true);
        clickButton.enabled = true;
        attack = true;

        for (int i = 0; i < enemiesNames.Length; i++)
            enemyObject.GetComponent<Animator>().SetBool(enemiesNames[i], false);

        while (randEnemy == repeatEnemy)
            randEnemy = Random.Range(0, enemiesNames.Length);

        repeatEnemy = randEnemy;

        enemyObject.GetComponent<Animator>().SetBool(enemiesNames[randEnemy], true);

        Debug.Log("Enemy: " + enemiesNames[randEnemy]);

        //currentEnemy = enemyNameString[randEnemy];
        currentEnemyHp = enemiesHp[randEnemy];

        hp = currentEnemyHp;
        ActuallyHP();

        hpProgress = 1;
        slider.value = 1;


        //enemyObject.GetComponent<SpriteRenderer>().sprite = enemiesSprites[randEnemy];
    }

    void DieEnemy()
    {
        enemyObject.GetComponent<Animator>().Play("Die Animation");

        //enemyObject.SetActive(false);
        clickButton.enabled = false;
        attack = false;

        stage++;
        ActuallyStage();

        GameManager.instance.gold += 10;

        Debug.Log("Die");
    }

    void ShowRandAttackEnemy(int r)
    {
        GameObject att = Instantiate(attackPointText, generatePointsCointener.transform);

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
            fightWithBoss.enabled = true;
            stageText.enabled = false;
            stagesButton.enabled = true;

            boss = true;
        }
        else if (stage > maxStage && boss == false)
        {
            fightWithBoss.enabled = false;
            stageText.enabled = true;
            stagesButton.enabled = false;

            fightWithBossAnimation.enabled = true;

            stage = 0;
            GameManager.instance.gold += 40;
            //GameManager.instance.level++;       
        }

        stageText.text = stage.ToString() + "/" + maxStage.ToString();
    }

    void ActuallyGold()
    {
        goldText.text = GameManager.instance.gold.ToString();
    }

    public void FightBoss()
    {
        //FightWithBoss.enabled = false;
        boss = false;

        fightWithBossAnimation.enabled = false;
        fightWithBoss.fontSize = 90;

        currentEnemyHp = bossesHp[GameManager.instance.level];

        hp = currentEnemyHp;
        ActuallyHP();

        hpProgress = 1;
        slider.value = 1;

        enemyObject.GetComponent<SpriteRenderer>().sprite = bossesSprites[GameManager.instance.level];

        Debug.Log("Boss");
    }

    void DelatePoint()
    {
        foreach (Transform child in generatePointsCointener.GetComponent<Transform>())
        {
            if (child.GetComponent<Collider2D>().IsTouching(attackPointBlocker.GetComponent<Collider2D>()))
            {
                Destroy(child.gameObject);
            }
        }   
    }
}
