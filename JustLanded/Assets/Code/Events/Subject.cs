using System.Collections.Generic;

public abstract class Subject<T>
{
    public abstract List<IListener<T>> Listeners { get; set; }

    void Add(IListener<T> listener)
    {
        Listeners.Add(listener);
    }

    void Detach(IListener<T> listener)
    {
        Listeners.Remove(listener);
    }

    void Notify(T notification)
    {
        foreach (IListener<T> listener in Listeners)
        {
            listener.Update(notification);
        }
    }

}