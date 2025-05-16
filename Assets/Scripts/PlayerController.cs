using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
  public float jumpForce = 15f;
  public float springJumpForce = 25f;
  public LayerMask platformLayer;
  public Transform feetCheck;
  public float feetRadius = 0.2f;
  public bool isGrounded;
  public float swipeForce = 10f;
  public float maxForce = 20f;

  private const float RelativeVelocityValue = 10;
  private const string PlatformTag = "Platform";
  private const string SpringTag = "Spring";
  private const string BreakableTag = "Breakable";
  private Rigidbody2D _rb;
  private SpriteRenderer _sr;
  private PlayerAnimator _playerAnimator;
  private float _horizontal;
  private Vector2 _startTouch;
  private bool _isSwiping;

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
    if (Input.GetMouseButtonDown(0))
    {
      _startTouch = Input.mousePosition;
      _isSwiping = true;
    }
    else if (Input.GetMouseButtonUp(0) && _isSwiping)
    {
      Vector2 endTouch = Input.mousePosition;
      ApplySwipeForce(endTouch - _startTouch);
      _isSwiping = false;
    }
    if (Input.touchCount > 0)
    {
      var touch = Input.GetTouch(0);
      switch (touch.phase)
      {
        case TouchPhase.Began:
          _startTouch = touch.position;
          _isSwiping = true;
          break;
        case TouchPhase.Ended when _isSwiping:
        {
          var endTouch = touch.position;
          ApplySwipeForce(endTouch - _startTouch);
          _isSwiping = false;
          break;
        }
      }
    }
  }

  private void ApplySwipeForce(Vector2 swipeDelta)
  {
    if (Mathf.Abs(swipeDelta.x) < 20f) return;

    var direction = Mathf.Sign(swipeDelta.x);
    var strength = Mathf.Clamp(swipeDelta.magnitude / 100f, 0f, 1f);

    var force = direction * strength * swipeForce;
    force = Mathf.Clamp(force, -maxForce, maxForce);

    _rb.linearVelocity = new Vector2(force, _rb.linearVelocity.y);
    FlipSprite();
  }

  public void FixedUpdate() =>
    isGrounded = Physics2D.OverlapCircle(feetCheck.position, feetRadius, platformLayer);

  private void FlipSprite()
  {
    _sr.flipX = _rb.linearVelocity.x switch
    {
      > 0 when _sr.flipX => false,
      < 0 when !_sr.flipX => true,
      _ => _sr.flipX
    };
  }
}