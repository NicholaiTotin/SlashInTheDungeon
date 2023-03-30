using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TurnManager : MonoBehaviour
{
    [SerializeField] int indexTurn = 0;
    [SerializeField] List<GameObject> characters = new List<GameObject>();
    [SerializeField] GameObject ButtonsUI;

    public int IndexTurn { set { indexTurn = value; } }

    public void SortTurns()
    {
        characters.Clear();
        foreach(GameObject character in GameObject.FindGameObjectsWithTag("Character"))
        {
            characters.Add(character);
        }

        characters.Sort((x, y) => y.GetComponent<CharacterStats>().stats.speed.CompareTo(x.GetComponent<CharacterStats>().stats.speed));

        characters[0].GetComponent<CharacterStats>().IsMyTurn = true;
    }

    public void NextTurn()
    {
        indexTurn++;
        if (indexTurn >= characters.Count)
        {
            indexTurn = 0;
        }
        characters[indexTurn].GetComponent<CharacterStats>().BeginTurn();
    }

}
