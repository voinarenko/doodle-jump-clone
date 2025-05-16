using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
  public static AudioManager Instance;

  [SerializeField] private Button muteButton;
  [SerializeField] private Image muteButtonImage;
  [SerializeField] private List<Sprite> muteButtonSprites;
  [SerializeField] private AudioSource backgroundAudio;
  [SerializeField] private AudioSource soundEffectAudio;

  [SerializeField] private AudioClip JumpSound;
  [SerializeField] private AudioClip BreakSound;
  [SerializeField] private AudioClip DoubleJumpSound;
  [SerializeField] private AudioClip ScoreSound;
  [SerializeField] private AudioClip GameOverSound;
  private bool _isMuted;

  private void Awake() =>
    Instance = this;

  private void Start() =>
    muteButton.onClick.AddListener(SwitchMute);

  private void OnDestroy() =>
    muteButton.onClick.RemoveListener(SwitchMute);

  public void PlayJump() =>
    soundEffectAudio.PlayOneShot(JumpSound);

  public void PlayBreak() =>
    soundEffectAudio.PlayOneShot(BreakSound);

  public void PlayDoubleJump() =>
    soundEffectAudio.PlayOneShot(DoubleJumpSound);

  public void PlayScore() =>
    soundEffectAudio.PlayOneShot(ScoreSound);

  public void PlayGameOver() =>
    soundEffectAudio.PlayOneShot(GameOverSound);

  private void SwitchMute()
  {
    if (_isMuted)
    {
      _isMuted = false;
      soundEffectAudio.mute = false;
      backgroundAudio.mute = false;
      muteButtonImage.sprite = muteButtonSprites[1];
    }
    else
    {
      _isMuted = true;
      soundEffectAudio.mute = true;
      backgroundAudio.mute = true;
      muteButtonImage.sprite = muteButtonSprites[0];
    }
  }
}