using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChoosePoints : MonoBehaviour
{
    int points = 10;
    [SerializeField] GameObject selectStats;
    [SerializeField] Player player;
    [SerializeField] TMP_Text[] texts;
    [SerializeField] GameObject inputTest;
    TurnManager turnManager;

    bool inGame;
    public bool InGame {get {return inGame;}}

    void Start()
    {
        turnManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<TurnManager>();
        selectStats.SetActive(true);
    }

    public void Increase(int i)
    {
        if(points > 0)
        {
            switch(i)
            {
                case 0:
                    player.stats.health += 1;
                    texts[1].text =  $"{player.stats.health}";
                    break;
                case 1:
                    player.stats.defense += 1;
                    texts[2].text =  $"{player.stats.defense}";
                    break;
                case 2:
                    player.stats.damage += 1;
                    texts[3].text =  $"{player.stats.damage}";
                    break;
                case 3:
                    player.stats.speed += 1;
                    texts[4].text =  $"{player.stats.speed}";
                    break;
                case 4:
                    player.stats.heal += 1;
                    texts[5].text =  $"{player.stats.heal}";
                    break;
            }
            points -= 1;
            texts[0].text =  $"Points: {points}";
        }
    }

    public void Decrease(int i)
    { 
        switch(i)
        {
            case 0:
                if(player.stats.health > 1)
                {
                    player.stats.health -= 1;
                    texts[1].text =  $"{player.stats.health}";
                    points += 1;
                }
                break;
            case 1:
                if(player.stats.defense > 1)
                {
                    player.stats.defense -= 1;
                    texts[2].text =  $"{player.stats.defense}";
                    points += 1;
                }
                break;
            case 2:
                if(player.stats.damage > 1)
                {
                    player.stats.damage -= 1;
                    texts[3].text =  $"{player.stats.damage}";
                    points += 1;
                }
                break;
            case 3:
                if(player.stats.speed > 1)
                {
                    player.stats.speed -= 1;
                    texts[4].text =  $"{player.stats.speed}";
                    points += 1;
                }
                break;
            case 4:
                if(player.stats.heal > 1)
                {
                    player.stats.heal -= 1;
                    texts[5].text =  $"{player.stats.heal}";
                    points += 1;
                }
                break;
            }
            texts[0].text =  $"Points: {points}";
    }

    public void StartGame()
    {
        if(points == 0)
        {
            inputTest.SetActive(true);
            player.CurrentHealth = player.stats.health;
            inGame = true;
            selectStats.SetActive(false);
            turnManager.SortTurns();
        }
    }
}
