using UnityEngine;

public class Params_PanelStartingPositions
{   

    /*
    *   This class defines the parameters where the transforms of various 
    *   objects of the interface will be initialized with.
    */

    public struct WorldPositionParameters
    {   

        /*
        *   WorldPositionParameters defines the position, rotation, and scale
        *   of a GameObject.
        */

        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scale;
        public WorldPositionParameters(Vector3 p, Quaternion r, Vector3 s)
        {
            position = p;
            rotation = r;
            scale = s;
        }
    }

    public static WorldPositionParameters GetWorldPositionParametersByName(string name)
    {   
        if(name.Equals("MapControlsPanel"))
        {
            return new WorldPositionParameters(new Vector3(0.5f,0f,0f), new Quaternion(0f,0.28f,0f,0.95f), Vector3.one);
        }
        else if(name.Equals("DataFilterPanel"))
        {
            return new WorldPositionParameters(new Vector3(1.5f,1.7f,2f), new Quaternion(-0.18f,0.17f,0.033f,0.96f), Vector3.one);
        }
        else if(name.Equals("CommentManagementPanel"))
        {
            return new WorldPositionParameters(new Vector3(2f,0f,2f), new Quaternion(0f,0.25f,0f,0.96f), Vector3.one);
        }
        else if(name.Equals("StatisticsPanel"))
        {
            return new WorldPositionParameters(new Vector3(-1.5f,1.2f,2f), new Quaternion(-0.13f,-0.17f,-0.02f,0.97f), Vector3.one);
        }
        else if(name.Equals("CommentHistoryPanel"))
        {
            return new WorldPositionParameters(new Vector3(4f,0f,1f), new Quaternion(0f,0.4f,0f,0.9f), Vector3.one);
        }
        else if(name.Equals("ImageSlideshowPanel"))
        {
            return new WorldPositionParameters(new Vector3(-3.5f,1.5f,2.5f), new Quaternion(0,-0.3f,0,1f), Vector3.one);
        }
        else if(name.Equals("DataTransformationsPanel") || name.Equals("BellBarParent"))
        {
            return new WorldPositionParameters(new Vector3(1.5f,1.1f,2f), new Quaternion(0,0.25f,0,0.96f), Vector3.one);
        }
        else
        {   
            Debug.LogError("WorldPositionParameters not defined for " + name);
            return new WorldPositionParameters();
        }
    }
    
}
