using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CollectibleFruitController : MonoBehaviour, Subject<HealthItemCollectedEvent>, IListener<PlayerDeathEvent>
{

    [SerializeField] float value = 50f;

    private List<IListener<HealthItemCollectedEvent>> Listeners;

    void Awake()
    {
        Listeners = new List<IListener<HealthItemCollectedEvent>>();
    }

    void Start()
    {
        List<Subject<PlayerDeathEvent>> playerDeathSubjects = FindObjectsOfType<MonoBehaviour>(true).OfType<Subject<PlayerDeathEvent>>().ToList();
        foreach (Subject<PlayerDeathEvent> playerDeathSubject in playerDeathSubjects)
        {
            playerDeathSubject.Add(this);
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            Notify(new HealthItemCollectedEvent(value));
            gameObject.SetActive(false);
        }
    }



    public void Add(IListener<HealthItemCollectedEvent> listener)
    {
        Debug.Log("Adding listener to gear controller");
        Listeners.Add(listener);
    }

    public void Detach(IListener<HealthItemCollectedEvent> listener)
    {
        Listeners.Remove(listener);
    }

    public void Notify(HealthItemCollectedEvent notification)
    {
        foreach (IListener<HealthItemCollectedEvent> listener in Listeners)
        {
            listener.Notify(notification);
        }
    }

    public void Notify(PlayerDeathEvent notification)
    {
        gameObject.SetActive(true);
    }
}
