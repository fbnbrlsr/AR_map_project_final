using System;
using Mapbox.Unity.Map;
using UnityEngine;

public class ThreePointLegPath
{
    // Data reference
    private DatabaseLegData _leg;

    // Visualization parameters
    private GameObject startPointPrefab;
    private GameObject midPointPrefab;
    private GameObject endPointPrefab;
    private GameObject linePrefab;
    private AbstractMap _map;

    // Instances
    private GameObject startPointInstance;
    private GameObject midPointInstance;
    private GameObject endPointInstance;
    private GameObject line1Instance;
    private GameObject line2Instance;

    public void SetLeg(DatabaseLegData leg)
    {
        this._leg = leg;
    }

    public void SetParameters(GameObject startPointPrefab, GameObject midPointPrefab, GameObject endPointPrefab, 
        GameObject linePrefab, AbstractMap map)
    {
        this.startPointPrefab = startPointPrefab;
        this.midPointPrefab = midPointPrefab;
        this.endPointPrefab = endPointPrefab;
        this.linePrefab = linePrefab;
        this._map = map;
    }

    public void InstantiatePath()
    {   
        // TODO
        throw new NotImplementedException();
    }

    public void UpdateVisualization()
    {
        // TODO
        throw new NotImplementedException();
    }


}
