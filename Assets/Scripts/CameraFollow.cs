using UnityEngine;

public class CameraFollow : MonoBehaviour
{
  public Transform target;
  private float _highestY;

  private void LateUpdate()
  {
    if (!target || !(target.position.y > _highestY)) 
      return;
    
    _highestY = target.position.y;
    transform.position = new Vector3(0f, _highestY, transform.position.z);
  }
}