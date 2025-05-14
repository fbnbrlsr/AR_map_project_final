using UnityEngine;
using XCharts.Runtime;

public class ChartManager
{   
    private BarChart barChart;

    public void SetBarChart(BarChart barChart)
    {
        this.barChart = barChart;

        ConfigureBarChart();
    }

    public BarChart GetBarChart()
    {
        return barChart;
    }

    public void UpdateData(int[] carCount, int[] bikeCount, int[] walkCount, int[] carPassengerCount, int[] ptCount)
    {   
        foreach(Serie s in barChart.series)
        {
            foreach(SerieData data in s.data)
            {   
                switch(s.serieName)
                {
                    case "Car":
                        data.data[1] = carCount[(int) data.data[0]];
                        break;
                    case "Bike":
                        data.data[1] = bikeCount[(int) data.data[0]];
                        break;
                    case "Walk":
                        data.data[1] = walkCount[(int) data.data[0]];
                        break;
                    case "Car Passenger":
                        data.data[1] = carPassengerCount[(int) data.data[0]];
                        break;
                    case "Public Transport":
                        data.data[1] = ptCount[(int) data.data[0]];
                        break;
                    default:
                        Debug.LogError("Series " + s.serieName + " has no data!");
                        break;
                }
            }

        }
        
        barChart.RefreshChart();
        barChart.AnimationFadeIn();
    }

    void ConfigureBarChart()
    {   
        foreach(Serie s in barChart.series)
        {
            s.animation.fadeIn.duration = 100f;
        }
    }

}
