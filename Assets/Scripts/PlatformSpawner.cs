using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
  public GameObject normalPlatform;
  public GameObject springPlatform;
  public GameObject breakablePlatform;

  public int initialPlatformCount = 10;
  public float minY = 1f, maxY = 3f;
  public float levelWidth = 3f;

  [Header("Difficulty Settings")] 
  public Transform player;
  private const float SpawnHeightOffset = 10f;
  private float _highestY;
  private Camera _camera;

  private void Start()
  {
    _camera = Camera.main;
    var pos = Vector3.zero;
    for (var i = 0; i < initialPlatformCount; i++)
    {
      SpawnPlatform(pos.y);
      pos.y += Random.Range(minY, maxY);
    }

    _highestY = pos.y;
  }

  private void Update()
  {
    var camY = _camera.transform.position.y + SpawnHeightOffset;
    while (_highestY < camY)
    {
      SpawnPlatform(_highestY);
      _highestY += Random.Range(minY, maxY);
    }
  }

  private void SpawnPlatform(float y)
  {
    var position = new Vector3(Random.Range(-levelWidth, levelWidth), y, 0f);
    var playerY = player ? player.position.y : 0f;

    const float springChance = 0.1f;
    var breakableChance = Mathf.Clamp01(playerY / 120f);
    var normalChance = Mathf.Clamp01(1f - springChance - breakableChance);

    var rand = Random.value;

    GameObject prefab;
    if (rand < springChance)
      prefab = springPlatform;
    else if (rand < springChance + breakableChance)
      prefab = breakablePlatform;
    else if (rand < springChance + breakableChance + normalChance)
      prefab = normalPlatform;
    else
      prefab = normalPlatform;

    Instantiate(prefab, position, Quaternion.identity);
  }
}