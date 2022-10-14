using UnityEngine;

namespace Script
{
  public class CrossbowAnimator : MonoBehaviour
  {
    [SerializeField] private Animator _animator;
    private readonly int _loadingProgressHash = Animator.StringToHash("LoadingProcess");
    private readonly int _shootHash = Animator.StringToHash("Shoot");
    private readonly int _triggerHash = Animator.StringToHash("Trigger");
    private readonly int _swapArrowHash = Animator.StringToHash("SwapArrow");

    public void LoadProgress(float progress) => 
      _animator.SetFloat(_loadingProgressHash, Mathf.Clamp01(progress));

    public void Shoot()
    {
      _animator.SetTrigger(_shootHash);
      _animator.SetFloat(_loadingProgressHash, 0f);
    }

    public void PressTrigger() => 
      _animator.SetTrigger(_triggerHash);

    public void SwapArrow() => 
      _animator.SetTrigger(_swapArrowHash);
  }
}