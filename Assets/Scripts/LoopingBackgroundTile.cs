using UnityEngine;

public class LoopingBackgroundTile : MonoBehaviour
{
  public Camera MainCamera { get; set; }
  public bool IsInitialized { get; set; }
  public float TileHeight { get; set; }

  private float BottomEdge =>
    MainCamera.transform.position.y - MainCamera.orthographicSize;

  private void Update()
  {
    if (!IsInitialized)
      return;

    if (transform.position.y + TileHeight < BottomEdge)
      transform.position += new Vector3(0f, TileHeight * BackgroundLooper.TotalTiles, 0f);
  }
}