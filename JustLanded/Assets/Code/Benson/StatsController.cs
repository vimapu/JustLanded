using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsController : MonoBehaviour
{

    [Header("Stat parameters")]
    [SerializeField] float maxHealth;

    private float numOfGear = 0;
    private float collectedGear = 0;
    private float gearPoints = 0;
    private float numOfEnemies = 0;
    private float numOfKill = 0;
    private float killPoints = 0;

    // Start is called before the first frame update
    void Start()
    {
        // count number of enemy
        numOfEnemies += GameObject.FindGameObjectsWithTag("TriangularEnemy").Length;
        numOfEnemies += GameObject.FindGameObjectsWithTag("SquareEnemy").Length;
        // count num of gear
        numOfGear += GameObject.FindGameObjectsWithTag("Gear").Length;
    }

    // called by the observed when it dies
    public void addDeadEnemy(float points)
    {
        numOfKill++;
        killPoints += points;
    }

    public void addCollectedGear(float points)
    {
        collectedGear++;
        gearPoints += points;
    }
}
