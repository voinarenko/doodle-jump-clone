using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
  private static readonly int Jump = Animator.StringToHash("Jump");
  private static readonly int Fall = Animator.StringToHash("Fall");
  private static readonly int DoubleJump = Animator.StringToHash("DoubleJump");

  private Animator _anim;
  private Rigidbody2D _rb;

  private void Start()
  {
    TryGetComponent(out _anim);
    TryGetComponent(out _rb);
  }

  private void Update()
  {
    if (_rb.linearVelocity.y < 0)
      PlayFall();
  }

  public void PlayJump()
  {
    _anim.SetBool(Fall, false);
    _anim.SetBool(DoubleJump, false);
    _anim.SetBool(Jump, true);
  }

  public void PlayDoubleJump()
  {
    _anim.SetBool(Fall, false);
    _anim.SetBool(Jump, false);
    _anim.SetBool(DoubleJump, true);
  }

  private void PlayFall()
  {
    if (_anim.GetCurrentAnimatorStateInfo(0).fullPathHash == Fall)
      return;

    _anim.SetBool(Jump, false);
    _anim.SetBool(DoubleJump, false);
    _anim.SetBool(Fall, true);
  }
}