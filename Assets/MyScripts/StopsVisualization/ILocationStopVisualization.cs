public interface IStopColumnVisualization
{   

    /*
    *   This interface defines the methods that must be provided
    *   by the stops visualization classes.
    */

    public void SetColumnData(int nof_stops, float lat, float lon);

    public void InstantiateColumn();

    public void UpdateVisualization(float width);

    public void DestroyColumn();
}
