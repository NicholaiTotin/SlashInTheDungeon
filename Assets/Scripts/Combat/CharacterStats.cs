using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CharacterStats : MonoBehaviour
{
    //SerializeField and public variables
    [Header("Stats")]
    public Stats stats;

    [Header("Managers")]
    [SerializeField] TurnManager manager;
    [SerializeField] float delayBtwMoves;

    [Header("UI")]
    [SerializeField] GameObject buttonsUI;
    [SerializeField] GameObject textUI;
    [SerializeField] GameObject textHolder;

    [Header("Entities")]
    [SerializeField] Enemies enemy;

    [SerializeField] bool defending;

    [SerializeField] List<GameObject> Texts = new List<GameObject>();


    //privates
    bool isMyTurn;
    int currentHealth = 1;

    //encapsulations

    public float DelayBtwMoves { get { return delayBtwMoves; } }
    public int CurrentHealth { get {return currentHealth;} set {currentHealth = value;}}
    public bool IsMyTurn { get { return isMyTurn; } set { isMyTurn = value; }}
    public bool Defending { get { return defending; } set { defending = value; }}
    public TurnManager Manager { get { return manager; } }
    public GameObject ButtonsUI { get { return buttonsUI; } }
    public GameObject TextUI { get { return textUI; } }
    public GameObject TextHolder { get { return textHolder; } }



    public virtual void BeginTurn()
    {
        isMyTurn = true;
    }

    public virtual void EndTurn()
    {
        isMyTurn = false;

        Manager.NextTurn();
    }

    //Heal
    public virtual void HealAction()
    {
        currentHealth += stats.heal;
        if(currentHealth > stats.health)
        {
            currentHealth = stats.health;
        }
    }

    //Info combat
    public void TextInstantiate(string text)
    {
        Texts.Add(Instantiate(TextUI, TextHolder.transform.position, Quaternion.identity, TextHolder.transform));
        int lastIndex = Texts.Count - 1;
        Texts[lastIndex].GetComponent<TMP_Text>().text = text;
        Texts[lastIndex].transform.SetSiblingIndex(0);
    }

    public IEnumerator DeleteText(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        for(int i = 0; i < textHolder.transform.childCount; i++)
        {
            Destroy(textHolder.transform.GetChild(i).gameObject);
            enemy.InputTest.enemyInFront = false;
            enemy.RandomValue = 0;
            enemy.InputTest.valueEnemy = 0;
            manager.IndexTurn = 0;
        }
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(0);
    }
}
