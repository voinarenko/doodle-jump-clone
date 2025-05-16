using UnityEngine;

public class PlatformAnimator : MonoBehaviour
{
  private static readonly int Fire = Animator.StringToHash("Fire");
  [SerializeField] private Animator anim;

  public void PlayFire() =>
      anim.SetTrigger(Fire);
}