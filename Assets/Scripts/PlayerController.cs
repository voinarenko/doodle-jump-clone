using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
  public float moveSpeed = 10f;
  public float jumpForce = 15f;
  public float springJumpForce = 25f;
  public LayerMask platformLayer;
  public Transform feetCheck;
  public float feetRadius = 0.2f;
  public bool isGrounded;

  private const float RelativeVelocityValue = 10;
  private const string PlatformTag = "Platform";
  private const string SpringTag = "Spring";
  private const string BreakableTag = "Breakable";
  private Rigidbody2D _rb;
  private SpriteRenderer _sr;
  private PlayerAnimator _playerAnimator;
  private float _horizontal;


  public void Start()
  {
    TryGetComponent(out _rb);
    TryGetComponent(out _sr);
    TryGetComponent(out _playerAnimator);
  }

  private void OnCollisionEnter2D(Collision2D collision)
  {
    if (!(collision.relativeVelocity.y <= RelativeVelocityValue))
      return;

    if (!isGrounded)
      return;

    if (collision.collider.CompareTag(PlatformTag))
    {
      var validContact = collision.contacts.Any(contact => contact.normal.y > 0.5f);
      if (!validContact)
        return;

      _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, jumpForce);
      AudioManager.Instance.PlayJump();
      _playerAnimator.PlayJump();
    }
    else if (collision.collider.CompareTag(SpringTag))
    {
      var validContact = collision.contacts.Any(contact => contact.normal.y > 0.5f);
      if (!validContact)
        return;

      _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, springJumpForce);
      collision.collider.gameObject.TryGetComponent<PlatformAnimator>(out var animator);
      animator.PlayFire();
      AudioManager.Instance.PlayDoubleJump();
      _playerAnimator.PlayDoubleJump();
    }
    else if (collision.collider.CompareTag(BreakableTag))
    {
      var validContact = collision.contacts.Any(contact => contact.normal.y > 0.5f);
      if (!validContact)
        return;

      _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, jumpForce);
      AudioManager.Instance.PlayBreak();
      AudioManager.Instance.PlayJump();
      _playerAnimator.PlayJump();
      Destroy(collision.collider.gameObject);
    }
  }

  private void Update()
  {
#if UNITY_WEBGL && !UNITY_EDITOR
    _horizontal = Input.acceleration.x * moveSpeed;
#else
    _horizontal = Input.GetAxis("Horizontal") * moveSpeed;
#endif
    FlipSprite();
  }

  public void FixedUpdate()
  {
    isGrounded = Physics2D.OverlapCircle(feetCheck.position, feetRadius, platformLayer);
    _rb.linearVelocity = new Vector2(_horizontal, _rb.linearVelocity.y);
  }

  private void FlipSprite()
  {
    _sr.flipX = _horizontal switch
    {
      > 0 when _sr.flipX => false,
      < 0 when !_sr.flipX => true,
      _ => _sr.flipX
    };
  }
}