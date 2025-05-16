using UnityEngine;

public class BackgroundLooper : MonoBehaviour
{
  public GameObject tilePrefab;
  public int tileCount = 3;
  public Camera mainCamera;

  public static int TotalTiles;

  private void Awake()
  {
    TotalTiles = tileCount;

    var tileHeight = tilePrefab.GetComponentInChildren<SpriteRenderer>().bounds.size.y;
    var cameraBottom = mainCamera.transform.position.y - mainCamera.orthographicSize;

    for (var i = 0; i < tileCount; i++)
    {
      var tile = Instantiate(tilePrefab, transform);
        
      var yPos = cameraBottom + i * tileHeight;
      tile.transform.position = new Vector3(0f, yPos, 0f);

      var loop = tile.AddComponent<LoopingBackgroundTile>();
      loop.TileHeight = tileHeight;
      loop.MainCamera = mainCamera;
      loop.IsInitialized = true;
    }
  }
}