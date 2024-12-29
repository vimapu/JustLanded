

public class HealthItemCollectedEvent
{
    public float HealthPoints { get; }
    public HealthItemCollectedEvent(float points)
    {
        HealthPoints = points;
    }

}