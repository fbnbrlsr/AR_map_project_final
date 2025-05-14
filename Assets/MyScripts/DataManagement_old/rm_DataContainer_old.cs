using System;
using System.Collections;
using System.Collections.Generic;
using Mapbox.Unity.Utilities;
using Mapbox.Utils;
using UnityEngine;
using UnityEngine.UIElements;

public class rm_DataContainer_old
{

    /*private static int currentPointIndex = 0;
    private static int currentPathIndex = 0;
    private static List<string> coordinateStrings;
    private static List<string[]> pathCoordinatesStrings;
    
    public static Vector2d GetNextPoint()
    {   
        Vector2d point = Conversions.StringToLatLon(coordinateStrings[currentPointIndex]);
        currentPointIndex += 1;
        return point;
    }

    public static void ReadCoordinateStrings()
    {
        coordinateStrings = new List<string>();

        // ETH Zürich
        coordinateStrings.Add("47.376358, 8.547992");

        // My apartment
        coordinateStrings.Add("47.357995, 8.579141");

        // Zürich HB
        coordinateStrings.Add("47.378428, 8.538315");

        // Uetliberg
        coordinateStrings.Add("47.349827, 8.491053");

        // Appenzell Bahnhof
        coordinateStrings.Add("47.328413, 9.409811");

    }

    public static Vector2d[] GetNextPath()
    {   
        Vector2d[] path = new Vector2d[3];
        path[0] = Conversions.StringToLatLon(pathCoordinatesStrings[currentPathIndex][0]);
        path[1] = Conversions.StringToLatLon(pathCoordinatesStrings[currentPathIndex][1]);
        path[2] = Conversions.StringToLatLon(pathCoordinatesStrings[currentPathIndex][2]);
        currentPathIndex += 1;
        return path;
    }

    public static void ReadRandomPaths()
    {
        pathCoordinatesStrings = new List<string[]>();

        double lower_x = 45.82;
        double upper_x = 47.81;
        double lower_y = 5.96;
        double upper_y = 10.49;

        int nof_points = 15;
        
        for(int i = 0; i < nof_points; i++)
        {   
            System.Random r = new System.Random();
            string[] arr = new string[3];
            for(int p = 0; p < 3; p++)
            {   
                double rand_x = r.NextDouble() * (upper_x - lower_x) + lower_x;
                double rand_y = r.NextDouble() * (upper_y - lower_y) + lower_y;
                arr[p] = rand_x + ", " + rand_y;
            }
            pathCoordinatesStrings.Add(arr);
        }
    }

    public static void ReadDatabasePathsStatic()
    {   
        List<string[]> stringArrays = new();
        stringArrays.Add(new string[]{"8.803880709111372","47.32619089452405","8.033246520250307","47.503626841598916","7.39454048478781","47.83609532690372"});
        stringArrays.Add(new string[]{"7.438632452734678","46.949283737274506","7.936280523881481","47.35387919687069","8.536384816588612","47.37851914199306"});
        stringArrays.Add(new string[]{"7.726589408450273","47.494001170844705","8.199171845864893","47.132969540153795","8.639876678427221","46.85842625188009"});
        stringArrays.Add(new string[]{"6.629423881730888","46.51641265955995","6.992426228966958","46.74241440363452","7.437318922653007","46.9483842013205"});
        stringArrays.Add(new string[]{"8.57664097413195","47.27918102371199","8.800579759710118","46.57502505642171","9.026596433041618","45.829809744745"});
        stringArrays.Add(new string[]{"7.417620516527514","46.93668837632688","7.755932925139687","47.29605065411481","8.2724001690084","47.34924398843366"});
        stringArrays.Add(new string[]{"7.438632452734678","46.949283737274506","7.842925625985373","47.26069996902638","7.58872711943797","47.54732476162554"});
        stringArrays.Add(new string[]{"7.697285799303482","47.471585170079145","8.5467561809072","47.43149101654509","9.390039122376892","47.434725729585914"});
        stringArrays.Add(new string[]{"7.437318922653007","46.9483842013205","6.756100191216662","46.49475582029542","6.1125009875072385","46.232643853367996"});
        stringArrays.Add(new string[]{"6.6294104167478585","46.51731217159965","7.296953655818689","46.879032011582666","7.9435920013294785","47.28728753350537"});
        stringArrays.Add(new string[]{"8.639876678427221","46.85842625188009","9.015518422745409","46.25013996192726","9.062512332982578","45.521600933577055"});
        stringArrays.Add(new string[]{"8.994236861696544","47.184338243161704","9.518714370285474","47.096868217291544","9.088886768911014","47.30087482302118"});
        stringArrays.Add(new string[]{"8.536384816588612","47.37851914199306","7.917767418507686","47.35575631094476","7.438632452734678","46.949283737274506"});
        stringArrays.Add(new string[]{"9.204748163652921","45.4870506692373","8.416615248256784","45.39780153823193","7.67722725616248","45.06186977326276"});
        stringArrays.Add(new string[]{"7.713360867700355","47.5021277960594","8.23311994313961","47.11115129690222","8.59709456328919","46.69873190388275"});
        stringArrays.Add(new string[]{"2.355313974378314","48.88077517605861","2.785205494986157","50.19845896402871","4.3334084356664695","50.83402784732568"});
        stringArrays.Add(new string[]{"10.429579046529867","50.51643484942153","9.339698813767738","49.17926255053825","8.715908109152133","47.74096606399971"});
        stringArrays.Add(new string[]{"9.062512332982578","45.521600933577055","9.551369494836223","44.85213216407984","9.919013480291323","44.132930991369776"});
        stringArrays.Add(new string[]{"7.437318922653007","46.9483842013205","7.931521046037272","47.26845085665794","8.537708840957789","47.37850655257806"});
        stringArrays.Add(new string[]{"2.373965336640164","48.8437499332263","5.323392658020797","47.21946419914216","7.439946014677682","46.94838422313765"});
        stringArrays.Add(new string[]{"8.579319741092963","47.52831929746148","8.627102322950579","46.873853455801395","8.799483039646791","46.16837489654256"});
        stringArrays.Add(new string[]{"7.541574274258714","47.203795481485","8.004036722298538","47.37785278575308","8.536403340161442","47.37941847831911"});
        stringArrays.Add(new string[]{"6.629423881730888","46.51641265955995","7.5491943025553585","47.058071892358925","8.536403340161442","47.37941847831911"});
        stringArrays.Add(new string[]{"7.590055482836491","47.54732304316594","7.842925625985373","47.26069996902638","7.438632452734678","46.949283737274506"});
        stringArrays.Add(new string[]{"8.366591653384347","47.36201792602526","8.33073145077291","47.117631682361285","7.901561307117199","46.5552452336142"});
        stringArrays.Add(new string[]{"8.310498797631736","47.08900143076169","8.592557344011574","46.66998918945759","9.029497809809076","46.195959316834376"});

        pathCoordinatesStrings = new List<string[]>();
        foreach(string[] arr in stringArrays)
        {
            string[] a = new string[3];
            a[0] = arr[1] + ", " + arr[0];
            a[1] = arr[3] + ", " + arr[2];
            a[2] = arr[5] + ", " + arr[4];
            Debug.Log("Read path: start=" + a[0] + " | mid=" + a[1] + " | end=" + a[2]);
            pathCoordinatesStrings.Add(a);
        }
    }

    public static void ReadDatabasePaths()
    {   
        int limit = 100;
        DatabaseManager.Initialize(limit);

        pathCoordinatesStrings = new();
        for(int i = 0; i < limit; i++)
        {
            pathCoordinatesStrings.Add(DatabaseManager.GetNextCoordinateStringArray());
        }
    }*/

    
}
