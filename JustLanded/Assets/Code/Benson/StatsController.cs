using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsController : MonoBehaviour, IListener<GearCollectedEvent>, IListener<HealthItemCollectedEvent>, IListener<DeadEnemyEvent>
{

    [Header("Stat parameters")]
    [SerializeField] float maxHealth;

    private float numOfGear = 0;
    private float collectedGear = 0;
    private float gearPoints = 0;
    private float numOfEnemies = 0;
    private float numOfKill = 0;
    private float killPoints = 0;
    private float currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        // count number of enemy
        numOfEnemies += GameObject.FindGameObjectsWithTag("TriangularEnemy").Length;
        numOfEnemies += GameObject.FindGameObjectsWithTag("SquareEnemy").Length;
        // count num of gear
        numOfGear += GameObject.FindGameObjectsWithTag("Gear").Length;

        currentHealth = maxHealth;
    }

    // called by the observed when it dies
    private void AddDeadEnemy(float points)
    {
        numOfKill++;
        killPoints += points;
    }

    private void AddCollectedGear(int points)
    {
        collectedGear++;
        gearPoints += points;
    }

    public void Notify(GearCollectedEvent notification)
    {
        AddCollectedGear(notification.Points);
    }

    public void Notify(HealthItemCollectedEvent notification)
    {
        currentHealth = Mathf.Min(maxHealth, currentHealth + notification.HealthPoints);
    }

    public void Notify(DeadEnemyEvent notification)
    {
        AddDeadEnemy(notification.Points);
    }
}
