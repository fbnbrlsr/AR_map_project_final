using UnityEngine;
using Mapbox.Unity.Map;
using Mapbox.Utils;
using Mapbox.Map;
using System.Collections;

public class MapOnCube : MonoBehaviour
{
    public AbstractMap mapboxMap;   // Reference to Mapbox's AbstractMap component
    public GameObject cube;         // The cube where the map texture will be applied
    private string mapboxApiKey = "YOUR_MAPBOX_ACCESS_TOKEN"; // Replace with your Mapbox access token
    public float zoomSpeed = 0.1f;  // Speed at which to zoom in/out
    private int zoomLevel = 16;     // Starting zoom level
    private RasterTile rasterTile;  // The raster tile object
    private Vector2d latLon;        // Coordinates (latitude, longitude)
    private CanonicalTileId tileID; 

    private string mapStyle = "mapbox://styles/mapbox/light-v11";

    void Start()
    {
        // Set initial latitude and longitude (you can modify this to any starting point)
        tileID = new CanonicalTileId(37454, -1234, 4);
        latLon = tileID.ToVector2d(); // Example: San Francisco coordinates
        StartCoroutine(LoadMapTile());
    }

    void Update()
    {
        // Handle zoom based on user input (scroll wheel)
        HandleZoom();
    }

    IEnumerator LoadMapTile()
    {
        // Create a tile at the given latitude, longitude, and zoom level
        TileResource tileResource = TileResource.MakeRaster(tileID, mapStyle);

        // Make a request to Mapbox to download the tile texture
        rasterTile = new RasterTile();
        rasterTile.Initialize(null, tileID, mapStyle, null);
        yield return new WaitUntil(() => rasterTile.HasError || rasterTile.IsCompleted);

        if (rasterTile.HasError)
        {
            Debug.LogError("Error fetching map tile: ...");
        }
        else if (rasterTile.IsCompleted)
        {
            // Create a texture from the tile data and apply it to the cube's material
            Texture2D texture = new Texture2D(256, 256);
            texture.LoadImage(rasterTile.Data); // Load the image data into the texture

            // Apply the texture to the cube
            cube.GetComponent<Renderer>().material.mainTexture = texture;
        }
    }

    private void HandleZoom()
    {
        float scrollDelta = Input.GetAxis("Mouse ScrollWheel");
        if (scrollDelta != 0)
        {
            zoomLevel = Mathf.Clamp(zoomLevel + (int)(scrollDelta * zoomSpeed * 10), 0, 20); // Adjust zoom level
            StopAllCoroutines(); // Stop any ongoing tile fetches
            StartCoroutine(LoadMapTile()); // Load the map tile with the new zoom level
        }
    }
}
