using StrikerLink.Shared.Devices.DeviceFeatures;
using StrikerLink.Unity.Runtime.HapticEngine;
using StrikerLink.Unity.Runtime.Core;
using UnityEngine;

namespace Script
{
      public class Attacher : MonoBehaviour
      {
            [SerializeField] private Rigidbody _rigidbody;
        
            private bool _attached;

            private GameObject mavrik;
            private Blaster blaster;
            private StrikerDevice mavrikBlaster;
            private Crossbow crossbow;
            private GameObject mavrikCrossbow;

            public HapticEffectAsset crossbow_attach;
            public AudioSource crossbow_attach_sfx;
             
            


            void Awake()
            {
                mavrik = GameObject.Find("StrikerBlasterPrefab");
                blaster = mavrik.GetComponent <Blaster>();
                mavrikBlaster = mavrik.GetComponent<StrikerDevice>();
                mavrikCrossbow = GameObject.Find("MavrikCrossbow");
                crossbow = mavrikCrossbow.GetComponent<Crossbow>();
            }

            private void OnCollisionEnter(Collision other)
            {
                TryAttachTo(other);
            }
    
            private void TryAttachTo(Collision other)
            {

                if (_attached == true || other == null)
                return;

                transform.parent = other.transform;

                
                blaster.crossbow_rope_sfx.Stop();

                mavrikBlaster.FireHaptic(crossbow_attach);  
                crossbow_attach_sfx.Play(0);

                Destroy(_rigidbody);
      
                _attached = true;
            }

            
      }
}