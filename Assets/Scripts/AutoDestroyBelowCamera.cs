using UnityEngine;

public class AutoDestroyBelowCamera : MonoBehaviour
{
  public float offset = 1f;
  private Camera _camera;

  private void Start() =>
    _camera = Camera.main;

  private void Update()
  {
    var cameraBottom = _camera.transform.position.y - _camera.orthographicSize;

    if (transform.position.y < cameraBottom - offset)
      Destroy(gameObject);
  }
}