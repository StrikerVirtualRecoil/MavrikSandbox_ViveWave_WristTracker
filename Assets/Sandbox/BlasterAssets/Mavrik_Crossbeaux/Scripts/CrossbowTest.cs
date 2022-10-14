using UnityEngine;

namespace Script
{
  public class CrossbowTest : MonoBehaviour
  {
    [SerializeField] private Crossbow _crossbow;
    private void Update()
    {
      if (Input.GetKeyDown(KeyCode.Q))
      {
        _crossbow.StartLoading();
      }

      if (Input.GetKeyDown(KeyCode.W))
      {
        _crossbow.Shoot();
      }

      if (Input.GetKeyDown(KeyCode.E))
      {
        _crossbow.SwapArrow();
      }
    }
  }
}