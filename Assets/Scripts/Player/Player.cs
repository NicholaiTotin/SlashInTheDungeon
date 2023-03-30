using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class Player : CharacterStats
{
    [SerializeField] InputTest inputTest;
    [SerializeField] Enemies enemies;
    [SerializeField] TMP_Text[] statsText;
    [SerializeField] GameObject gameEndedGame;
    ChoosePoints choosePoints;
    int action;

    void Start()
    {
        choosePoints = GameObject.FindGameObjectWithTag("Manager").GetComponent<ChoosePoints>();

        //reset stats
        stats.health = 1;
        stats.defense = 1;
        stats.damage = 1;
        stats.speed = 1;
        stats.heal = 1;
    }

    private void Update()
    {
        //Show in screen player stats
        statsText[0].text = $"Health: {CurrentHealth}/{stats.health} ";
        statsText[1].text = $"Defense: {stats.defense}";
        statsText[2].text = $"Damage: {stats.damage}";
        statsText[3].text = $"Speed: {stats.speed}";
        statsText[4].text = $"Heal: {stats.heal}";

        //Game ended
        if (choosePoints.InGame && CurrentHealth <= 0)
        {
            gameEndedGame.SetActive(true);
            
            CurrentHealth = 0;
        }

        //Restart scene
        if (Input.GetMouseButtonDown(0) && CurrentHealth == 0)
        {
            RestartScene();
        }
    }

    //Which action was made by player - 0 Attack - 1 Heal - 2 Run
    public void Action(int i)
    {
        ButtonsUI.transform.GetChild(0).gameObject.SetActive(false);

        //see if is player turn or not
        if(IsMyTurn)
        {
            action = i + 1;
            BeginTurn();
        }
        else
        {
            action = i + 1;
            enemies.BeginTurn();
        }
    }

    void Attack()
    {
        //Set red the enemy
        StartCoroutine(enemies.setWhite(1, Color.red));
        //damage receive by enemy
        enemies.CurrentHealth -= stats.damage;

        //Text Method for info in screen
        TextInstantiate($"Oval Man receive {stats.damage} points of damage");

        EndTurn();
    } 

    void Run()
    {
        //chance to escape from battle
        int rand = Random.Range(0, 100 + 1);
        Debug.Log($"{rand} Run chanse");
        
        if(stats.speed > enemies.stats.speed && rand > 25)
        {
            inputTest.RemoveAction();
            ButtonsUI.transform.GetChild(0).gameObject.SetActive(true);
            IsMyTurn = true;
            enemies.Dead();
            enemies.Anim.Play("None");
            TextInstantiate("You ran away successfully"); 
        }
        else if(rand > 75)
        {
            inputTest.RemoveAction();
            ButtonsUI.transform.GetChild(0).gameObject.SetActive(true);
            IsMyTurn = false;
            enemies.Dead();
            enemies.Anim.Play("None");
            TextInstantiate("You ran away barely"); 
        }
        else
        {
            TextInstantiate("You could not escape..."); 
            EndTurn();
        }
    }

    public override void HealAction()
    {
        TextInstantiate($"You heal yourself {stats.heal} points of current health");
        base.HealAction();
    }

    IEnumerator HealCoroutine(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        HealAction();
        EndTurn();
    }

    public override void BeginTurn()
    {
        //Do the action selected by player
        base.BeginTurn();
        switch (action)
        {
            case 1:
                TextInstantiate("Preparing for attack...");
                Invoke("Attack", DelayBtwMoves);
                break;
            case 2:
                TextInstantiate("Preparing for heal...");
                StartCoroutine(HealCoroutine(DelayBtwMoves));
                break;
            case 3:
                TextInstantiate("Preparing for run...");
                Invoke("Run", DelayBtwMoves);
                break;
        }
        action = 0;
    }

    public override void EndTurn()
    {
        //Finishing player turn
        if(enemies.CurrentHealth <= 0 && enemies.stats.speed >= stats.speed )
        {
            IsMyTurn = false;
            enemies.EnemyDying(true);
        }
        if(enemies.stats.speed < stats.speed && enemies.CurrentHealth <= 0)
        {
            enemies.EnemyDying(false);
        }
        if(enemies.CurrentHealth > 0 && stats.speed <= enemies.stats.speed)
        {
            TextInstantiate("Now is Oval Man turn");
            IsMyTurn = false;
            enemies.IsMyTurn = true;
            Manager.IndexTurn = 0;
            ButtonsUI.transform.GetChild(0).gameObject.SetActive(true);
        }
        if(enemies.CurrentHealth > 0 && stats.speed > enemies.stats.speed)
        {
            TextInstantiate("Now is Oval Man turn");
            base.EndTurn();
        }
    }

}
