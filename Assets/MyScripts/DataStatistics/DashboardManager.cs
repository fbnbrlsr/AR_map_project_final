using System.Collections.Generic;
using TMPro;
using UnityEngine;
using XCharts.Runtime;

public class DashboardManager : MonoBehaviour
{   

    /*
    *   This class initializes and updates the data in the chart on the
    *   StatisticsPanel. It also shows the total number of legs satisfying
    *   the active data filters.
    *   The update is triggered when the data filters have been modified.
    */

    [SerializeField] BarChart barChart;
    [SerializeField] TMP_Text totalLegCountText;

    private ChartManager chartManager;
    
    public void UpdateDatasetChart(List<DatabaseLegData> filteredData)
    {   
        int[] carCount = new int[30];
        int[] bikeCount = new int[30];
        int[] walkCount = new int[30];
        int[] carPassengerCount = new int[30];
        int[] ptCount = new int[30];
        int totalLegCount = 0;

        foreach(DatabaseLegData leg in filteredData)
        {   
            totalLegCount += 1;
            int hour = ((int) leg.departure_time) % 108000 / 3600;
            switch(leg.travel_mode)
            {
                case TravelMode.Car:
                    carCount[hour] += 1;
                    break;
                case TravelMode.Bike:
                    bikeCount[hour] += 1;
                    break;
                case TravelMode.Walk:
                    walkCount[hour] += 1;
                    break;
                case TravelMode.CarPassenger:
                    carPassengerCount[hour] += 1;
                    break;
                case TravelMode.PublicTr:
                    ptCount[hour] += 1;
                    break;
                default:
                    Debug.LogError("Travel mode is not valid!");
                    break;
            }
        }

        if(chartManager == null) this.Initialize();
        chartManager.UpdateData(carCount, bikeCount, walkCount, carPassengerCount, ptCount);
        totalLegCountText.text = "Total: " + totalLegCount;
    }

    private void Initialize()
    {
        chartManager = new ChartManager();
        chartManager.SetBarChart(barChart);
        totalLegCountText.text = "Total: 0";
    }


}
