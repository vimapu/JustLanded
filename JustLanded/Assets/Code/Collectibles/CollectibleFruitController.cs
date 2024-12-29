using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleFruitController : MonoBehaviour, Subject<HealthItemCollectedEvent>
{

    [SerializeField] float value = 50f;

    private List<IListener<HealthItemCollectedEvent>> Listeners;

    void Awake()
    {
        Listeners = new List<IListener<HealthItemCollectedEvent>>();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            Notify(new HealthItemCollectedEvent(value));
            Destroy(gameObject);
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
}
