using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class StatsController : MonoBehaviour, IListener<GearCollectedEvent>,
IListener<HealthItemCollectedEvent>, IListener<DeadEnemyEvent>, IListener<EndOfLevelEvent>, IListener<PlayerDeathEvent>
{

    [Header("Stat parameters")]
    [SerializeField] float maxHealth;
    [SerializeField] TextMeshProUGUI PointsText;
    [SerializeField] TextMeshProUGUI GearText;
    [Header("End of level screen parameters")]
    [SerializeField] TextMeshProUGUI GearCollectedText;
    [SerializeField] TextMeshProUGUI TotalPointsText;
    [SerializeField] TextMeshProUGUI EnemiesKilledText;
    [SerializeField] TextMeshProUGUI NumberOfDeathsText;
    [SerializeField] GameObject EndOfLevelPanel;

    private float _gearCount = 0;
    private float _collectedGear = 0;
    private float _gearPoints = 0;
    private float _enemyCount = 0;
    private float _killCount = 0;
    private float _killPoints = 0;
    private float _currentHealth;
    private float _deathCount = 0;

    private String _pointsOriginalText;
    private String _gearOriginalText;

    // Start is called before the first frame update
    void Start()
    {
        _pointsOriginalText = PointsText.text;
        _gearOriginalText = GearText.text;
        // count number of enemy
        _enemyCount += GameObject.FindGameObjectsWithTag("TriangularEnemy").Length;
        _enemyCount += GameObject.FindGameObjectsWithTag("SquareEnemy").Length;
        // count num of gear
        /***numOfGear += GameObject.FindGameObjectsWithTag("Gear").Length; **/

        _currentHealth = maxHealth;

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
        List<Subject<EndOfLevelEvent>> endOfLevelSubjects = FindObjectsOfType<MonoBehaviour>(true).OfType<Subject<EndOfLevelEvent>>().ToList();
        foreach (Subject<EndOfLevelEvent> endOfLevelSubject in endOfLevelSubjects)
        {
            endOfLevelSubject.Add(this);
        }
        List<Subject<PlayerDeathEvent>> playerDeathSubjects = FindObjectsOfType<MonoBehaviour>(true).OfType<Subject<PlayerDeathEvent>>().ToList();
        foreach (Subject<PlayerDeathEvent> playerDeathSubject in playerDeathSubjects)
        {
            playerDeathSubject.Add(this);
        }

        // setting the points to zero
        GearText.text = _gearOriginalText + _collectedGear;
        PointsText.text = _pointsOriginalText + (_gearPoints + _killPoints);
    }

    // called by the observed when it dies
    private void AddDeadEnemy(float points)
    {
        _killCount++;
        _killPoints += points;
        PointsText.text = _pointsOriginalText + (_gearPoints + _killPoints);
    }

    private void AddCollectedGear(float points)
    {
        _collectedGear++;
        _gearPoints += points;
        GearText.text = _gearOriginalText + _collectedGear;
        PointsText.text = _pointsOriginalText + (_gearPoints + _killPoints);
    }

    public void Notify(GearCollectedEvent notification)
    {
        AddCollectedGear(notification.Points);
    }

    public void Notify(HealthItemCollectedEvent notification)
    {
        _currentHealth = Mathf.Min(maxHealth, _currentHealth + notification.HealthPoints);
    }

    public void Notify(DeadEnemyEvent notification)
    {
        AddDeadEnemy(notification.Points);
    }

    public void Notify(EndOfLevelEvent notification)
    {
        Debug.Log("Stats controller has received end of level event.");
        TotalPointsText.text += (_gearPoints + _killPoints);
        GearCollectedText.text += _collectedGear + "/" + _gearCount;
        EnemiesKilledText.text += _killCount + "/" + _enemyCount;
        NumberOfDeathsText.text += _deathCount;
        EndOfLevelPanel.SetActive(true);
    }

    public void Notify(PlayerDeathEvent notification)
    {
        _deathCount++;
    }
}
