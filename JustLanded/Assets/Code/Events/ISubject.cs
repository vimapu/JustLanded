using System.Collections.Generic;

public interface ISubject<T>
{

    void Add(IListener<T> listener);


    void Detach(IListener<T> listener);


    void Notify(T notification);


}