using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class StatsController : MonoBehaviour, IListener<GearCollectedEvent>, IListener<HealthItemCollectedEvent>, IListener<DeadEnemyEvent>
{

    [Header("Stat parameters")]
    [SerializeField] float maxHealth;
    [SerializeField] TextMeshProUGUI PointsText;
    [SerializeField] TextMeshProUGUI GearText;

    private float numOfGear = 0;
    private float collectedGear = 0;
    private float gearPoints = 0;
    private float numOfEnemies = 0;
    private float numOfKill = 0;
    private float killPoints = 0;
    private float currentHealth;

    private String _pointsOriginalText;
    private String _gearOriginalText;

    // Start is called before the first frame update
    void Start()
    {
        _pointsOriginalText = PointsText.text;
        _gearOriginalText = GearText.text;
        // count number of enemy
        numOfEnemies += GameObject.FindGameObjectsWithTag("TriangularEnemy").Length;
        numOfEnemies += GameObject.FindGameObjectsWithTag("SquareEnemy").Length;
        // count num of gear
        /***numOfGear += GameObject.FindGameObjectsWithTag("Gear").Length; **/

        currentHealth = maxHealth;

        // adding itself as listener
        List<Subject<GearCollectedEvent>> gearSubjects = FindObjectsOfType<MonoBehaviour>(true).OfType<Subject<GearCollectedEvent>>().ToList();
        foreach (Subject<GearCollectedEvent> gearSubject in gearSubjects)
        {
            gearSubject.Add(this);
        }
        List<Subject<HealthItemCollectedEvent>> healthSubjects = FindObjectsOfType<MonoBehaviour>(true).OfType<Subject<HealthItemCollectedEvent>>().ToList();
        foreach (Subject<HealthItemCollectedEvent> healthSubject in healthSubjects)
        {
            healthSubject.Add(this);
        }
        List<Subject<DeadEnemyEvent>> enemyDeathSubjects = FindObjectsOfType<MonoBehaviour>(true).OfType<Subject<DeadEnemyEvent>>().ToList();
        foreach (Subject<DeadEnemyEvent> enemyDeathSubject in enemyDeathSubjects)
        {
            enemyDeathSubject.Add(this);
        }
        // setting the points to zero
        GearText.text = _gearOriginalText + collectedGear;
        PointsText.text = _pointsOriginalText + (gearPoints + killPoints);
    }

    // called by the observed when it dies
    private void AddDeadEnemy(float points)
    {
        numOfKill++;
        killPoints += points;
        PointsText.text = _pointsOriginalText + (gearPoints + killPoints);
    }

    private void AddCollectedGear(float points)
    {
        collectedGear++;
        gearPoints += points;
        GearText.text = _gearOriginalText + collectedGear;
        PointsText.text = _pointsOriginalText + (gearPoints + killPoints);
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
