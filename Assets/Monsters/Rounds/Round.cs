using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Round", menuName = "ScriptableObjects/RoundScriptableObject", order = 1)]
public class Round : ScriptableObject
{
    public EnemyAtTime[] enemyAtTimes;
}
public class EnemyAtTime
{
    public EnemyController enemy;
    public int time;
}