using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using XCharts.Runtime;

public class DashboardManager : MonoBehaviour
{   
    [SerializeField] BarChart barChart;
    [SerializeField] TMP_Text totalLegCountText;

    private ChartManager chartManager;
    
    void Start()
    {
        chartManager = new ChartManager();
        chartManager.SetBarChart(barChart);
        totalLegCountText.text = "Total: 0";
    }

    public void UpdateDatasetChart(List<K_DatabaseLegData> filteredData)
    {
        int[] carCount = new int[24];
        int[] bikeCount = new int[24];
        int[] walkCount = new int[24];
        int[] carPassengerCount = new int[24];
        int[] ptCount = new int[24];
        int totalLegCount = 0;

        foreach(K_DatabaseLegData leg in filteredData)
        {   
            totalLegCount += 1;
            int hour = ((int) leg.departure_time) % 86400 / 3600;
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
                case TravelMode.pt:
                    ptCount[hour] += 1;
                    break;
                default:
                    Debug.LogError("Travel mode is not valid!");
                    break;
            }
        }

        chartManager.UpdateData(carCount, bikeCount, walkCount, carPassengerCount, ptCount);
        totalLegCountText.text = "Total: " + totalLegCount;
    }


}
