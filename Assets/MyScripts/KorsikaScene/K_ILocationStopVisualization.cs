
public interface K_IStopColumnVisualization
{
    public void SetColumnData(int nof_stops, float lat, float lon);

    public void InstantiateColumn();

    public void UpdateVisualization(float width);

    public void DestroyColumn();
}
