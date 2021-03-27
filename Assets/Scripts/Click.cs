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
    public GameObject HPslider;
    public Slider slider;
    public float FillSpeed = 0.5f;
    private float hpProgress = 1;
    private float hp = 0;
    private bool attack = true;

    [Header("Attack Text Points")]
    public GameObject GeneratePointsCointener;
    public GameObject AttackPointText;
    public GameObject AttackPointBlocker;

    [Header("Enemy")]
    public SpriteRenderer EnemySprite;
    public Sprite[] enemyNameString = new Sprite[3];

    string[] enemyName = { "Ninja", "Elf", "Lumberjack" };
    int[] enemyHp = { 50, 75, 100 };
    string currentEnemy;
    int currentEnemyHp = 0;

    int repeatEnemy = 0;
    int randEnemy = 0;

    void Start()
    {
        RandEnemy();

        //slider = HPslider.GetComponent<Slider>();
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
        else
        {
            int r = Random.Range(minAttack, maxAttack);

            currentEnemyHp -= r;

            IncrementHpProgress(currentEnemyHp / hp);
            ShowRandAttackEnemy(r);

            //Debug.Log(r);
            Debug.Log("Enemy : " + currentEnemy);
            Debug.Log("Actualy HP : " + currentEnemyHp);
        }
    }

    public void RandEnemy()
    {

        HPslider.SetActive(true);
        ClickButton.enabled = true;
        EnemySprite.enabled = true;
        attack = true;

        while (randEnemy == repeatEnemy)
            randEnemy = Random.Range(0, enemyName.Length);

        repeatEnemy = randEnemy;

        currentEnemy = enemyName[randEnemy];
        currentEnemyHp = enemyHp[randEnemy];

        hp = currentEnemyHp;

        hpProgress = 1;
        slider.value = 1;


        EnemySprite.sprite = enemyNameString[randEnemy];
    }

    public void DieEnemy()
    {
        HPslider.SetActive(false);
        ClickButton.enabled = false;
        EnemySprite.enabled = false;
        attack = false;

        Debug.Log("Die");
    }

    public void ShowRandAttackEnemy(int r)
    {
        GameObject att = Instantiate(AttackPointText, GeneratePointsCointener.transform);

        att.GetComponent<TextMeshProUGUI>().text = r.ToString();
    }

    public void IncrementHpProgress(float newProgress)
    {
        hpProgress = newProgress;
    }

    void DelatePoint()
    {

        //Debug.Log(col);
        //Debug.Log(AttackPointBlocker.GetComponent<Collider2D>());

        foreach (Transform child in GeneratePointsCointener.GetComponent<Transform>())
        {
            if (child.GetComponent<Collider2D>().IsTouching(AttackPointBlocker.GetComponent<Collider2D>()))
            {
                Destroy(child.gameObject);
            }
        }   
    }
}
