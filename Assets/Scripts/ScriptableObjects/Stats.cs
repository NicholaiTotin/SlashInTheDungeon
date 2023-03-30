using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stats", menuName = "ScriptableObjects/Stats")]
public class Stats : ScriptableObject
{
    public int level;
    [Header("Stats")]
    public int health;
    public int defense;
    public int damage;
    public int speed;
    public int heal;
}

