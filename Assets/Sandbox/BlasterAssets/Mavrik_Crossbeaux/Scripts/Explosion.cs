using UnityEngine;

namespace Script
{
    public class Explosion : MonoBehaviour
    {
        [SerializeField] private GameObject _explosionVFX;
        [SerializeField] private float _explosionTime = 3f;
        [SerializeField] private float _explosionForce;
        [SerializeField] private float _radius;

        public Blaster blaster;
        public GameObject mavrik;
        public AudioSource crossbow_explosion;


        void Awake()
        {
            mavrik = GameObject.Find("StrikerBlasterPrefab");
            blaster = mavrik.GetComponent<Blaster>();
        }
        private void OnCollisionEnter(Collision other)
        {
            Explode();
        }
    
        private void Explode()
        {
            Vector3 transformPosition = transform.position;
            Collider[] colliders = Physics.OverlapSphere(transformPosition, _radius);
            foreach (Collider hit in colliders)
            {
                Rigidbody rigidbody = hit.GetComponent<Rigidbody>();

                if (rigidbody != null)
                    rigidbody.AddExplosionForce(_explosionForce, transformPosition, _radius, 3.0F);
            }

            GameObject explosion = Instantiate(_explosionVFX, transform.position, Quaternion.identity);

            if(blaster != null)
            {
                blaster.CrossBowExplodeHaptic();
                crossbow_explosion.Play(0);
            }

            
            Destroy(explosion, _explosionTime);
            gameObject.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().enabled = false;
            gameObject.GetComponent<CapsuleCollider>().enabled = false;
            Destroy(gameObject, 3f);
        }
    }
}