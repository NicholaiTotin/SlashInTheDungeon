using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Enemies : CharacterStats
{
    public Stats[] statsForEnemies;

    [SerializeField] int randomValue;
    [SerializeField] Sprite[] enemies;
    [SerializeField] Player player;
    [SerializeField] InputTest inputTest;

    Image enemyImage;
    Animator anim;

    public int RandomValue { set { randomValue = value; } }
    public InputTest InputTest { get { return inputTest; } }
    public Animator Anim { get {return anim;}}

    void Start()
    {
        stats = statsForEnemies[Random.Range(0, statsForEnemies.Length)];
        CurrentHealth = stats.health;
        anim = GetComponent<Animator>();
        enemyImage = GetComponent<Image>();
    }

    void Update()
    {
        //probability of enemy to appear
        if (inputTest.valueEnemy == 3)
        {
            randomValue = Random.Range(0, 11);
            inputTest.valueEnemy = 0;
        }

        //If appear
        if (randomValue > 5)
        {
            TextInstantiate("Oval Man appeared!");
            if(stats.speed < player.stats.speed)
            {
                TextInstantiate("You are faster than Oval man, you start");
            }
            else
            {
                TextInstantiate("Oval man is faster than you, Oval man start");
            }
            anim.Play("Idle");
            inputTest.RemoveAction();
            ButtonsUI.transform.GetChild(0).gameObject.SetActive(true);
            ButtonsUI.SetActive(true);
            inputTest.enemyInFront = true;
            enemyImage.sprite = enemies[1];
            randomValue = 0;
        }
    }
    
    public void Attack()
    {
        if(IsMyTurn)
        {
            //Attack player
            TextInstantiate("Oval Man attack you");
            anim.Play("Attack");

            player.CurrentHealth -= stats.damage;
            TextInstantiate($"You receive {stats.damage} points of damage");

            EndTurn();
        }
    } 

    public void Dead()
    {
        inputTest.RemoveEnemy();
        TextInstantiate($"Oval Man is gone");
        Manager.IndexTurn = 0;
        inputTest.RemoveAction();
        StatsReset();
        StartCoroutine(DeleteText(2));
    }
    
    public override void HealAction()
    {
        StartCoroutine(setWhite(1, Color.green));
        base.HealAction();
        TextInstantiate($"Oval Man heal his self {stats.heal} points of current health");
    }

    IEnumerator HealCoroutine(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        HealAction();
        EndTurn();
    }

    public IEnumerator setWhite(float seconds, Color color)
    {
        GetComponent<Image>().color = color;
        yield return new WaitForSeconds(seconds);
        GetComponent<Image>().color = Color.white;
    }

    public override void BeginTurn()
    {

        base.BeginTurn();
        //choose action in base of health
        if(CurrentHealth < (stats.health / 2) + 1)
        {
            int rand = Random.Range(0, 100+1);
            if(rand > 40)
            {
                StartCoroutine(HealCoroutine(DelayBtwMoves));
            }
            else
            {
                Invoke("Attack", DelayBtwMoves);
            }
        }
        else
        {
            Invoke("Attack", DelayBtwMoves);
        }
    }

    public void EnemyDying(bool turn)
    {
        TextInstantiate($"Oval Man could not take more damage");
        ButtonsUI.SetActive(false);
        IsMyTurn = turn;
        anim.Play("Death");
    }

    public override void EndTurn()
    {
        //finishing enemy turn
        if(CurrentHealth <= 0 && stats.speed >= player.stats.speed )
        {
            player.IsMyTurn = false;
            EnemyDying(true);
        }
        if(stats.speed < player.stats.speed && CurrentHealth <= 0)
        {
            EnemyDying(false);
        }
        if(CurrentHealth > 0 && player.stats.speed <= stats.speed)
        {
            TextInstantiate("Now is your turn");
            base.EndTurn();
        }
        if(CurrentHealth > 0 && player.stats.speed > stats.speed)
        {
            TextInstantiate("Now is your turn");
            ButtonsUI.transform.GetChild(0).gameObject.SetActive(true);
            base.EndTurn();
        }
    }

    void StatsReset()
    {
        //Reset stats
        stats = statsForEnemies[Random.Range(0, statsForEnemies.Length)];
        Manager.SortTurns();
        CurrentHealth = stats.health;
    }
}
