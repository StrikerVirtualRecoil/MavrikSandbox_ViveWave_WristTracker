using UnityEngine;

namespace Script
{
  public class Arrow : MonoBehaviour
  {
    [SerializeField] private Rigidbody _rigidbody;

    public void Launch(float speed)
    {
      _rigidbody.isKinematic = false;
      _rigidbody.gameObject.transform.SetParent(null);
      _rigidbody.velocity = transform.up * speed;
    }
  }
}