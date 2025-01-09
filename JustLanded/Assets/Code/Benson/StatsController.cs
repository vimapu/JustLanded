using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class StatsController : MonoBehaviour, IListener<GearCollectedEvent>, IListener<DeadEnemyEvent>, 
    IListener<EndOfLevelEvent>, IListener<PlayerDeathEvent>
{

    [Header("Stat parameters")]
    [SerializeField] TextMeshProUGUI PointsText;
    [SerializeField] TextMeshProUGUI GearText;
    [Header("End of level screen parameters")]
    [SerializeField] TextMeshProUGUI GearCollectedText;
    [SerializeField] TextMeshProUGUI TotalPointsText;
    [SerializeField] TextMeshProUGUI EnemiesKilledText;
    [SerializeField] TextMeshProUGUI NumberOfDeathsText;
    [SerializeField] GameObject EndOfLevelPanel;

    private float _gearCount = 0f;
    private float _collectedGear = 0f;
    private float _gearPoints = 0f;
    private float _enemyCount = 0f;
    private float _killCount = 0f;
    private float _killPoints = 0f;
    private float _deathCount = 0f;

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

        // adding itself as listener
        List<ISubject<GearCollectedEvent>> gearSubjects = FindObjectsOfType<MonoBehaviour>(true).OfType<ISubject<GearCollectedEvent>>().ToList();
        foreach (ISubject<GearCollectedEvent> gearSubject in gearSubjects)
        {
            gearSubject.Add(this);
            _gearCount++;
        }
        List<ISubject<DeadEnemyEvent>> enemyDeathSubjects = FindObjectsOfType<MonoBehaviour>(true).OfType<ISubject<DeadEnemyEvent>>().ToList();
        foreach (ISubject<DeadEnemyEvent> enemyDeathSubject in enemyDeathSubjects)
        {
            enemyDeathSubject.Add(this);
        }
        List<ISubject<EndOfLevelEvent>> endOfLevelSubjects = FindObjectsOfType<MonoBehaviour>(true).OfType<ISubject<EndOfLevelEvent>>().ToList();
        foreach (ISubject<EndOfLevelEvent> endOfLevelSubject in endOfLevelSubjects)
        {
            endOfLevelSubject.Add(this);
        }
        List<ISubject<PlayerDeathEvent>> playerDeathSubjects = FindObjectsOfType<MonoBehaviour>(true).OfType<ISubject<PlayerDeathEvent>>().ToList();
        foreach (ISubject<PlayerDeathEvent> playerDeathSubject in playerDeathSubjects)
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

    public void Notify(DeadEnemyEvent notification)
    {
        AddDeadEnemy(notification.Points);
    }

    public void Notify(EndOfLevelEvent notification)
    {
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
