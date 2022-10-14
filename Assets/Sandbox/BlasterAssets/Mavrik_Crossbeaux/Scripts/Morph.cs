using System.Collections;
using UnityEngine;

namespace Script
{
    public class Morph : MonoBehaviour
    {
        [SerializeField] private SkinnedMeshRenderer _meshRenderer;

        [SerializeField] private AnimationCurve _curveX;
        [SerializeField] private AnimationCurve _curveY;
        [SerializeField] private AnimationCurve _curveZ;

        private float _curvePoint = 0f;
        private int _weightScale = 100;

        private Coroutine _spawnCoroutine;

        public Blaster blaster;
        public GameObject mavrik;

        private void Awake()
        {
            mavrik = GameObject.Find("StrikerBlasterPrefab");
            blaster = mavrik.GetComponent<Blaster>();

            Spawn();
            ArrowSpawnHaptic();
        }

        private IEnumerator Spawning()
        {
            _curvePoint = 0f;

            while (_curvePoint >= 0 && _curvePoint < 1)
            {
                _curvePoint += Time.deltaTime;

                float valueX = 1 - _curveX.Evaluate(_curvePoint);
                float valueY = 1 - _curveY.Evaluate(_curvePoint);
                float valueZ = 1 - _curveZ.Evaluate(_curvePoint);

                _meshRenderer.SetBlendShapeWeight(4, valueX * _weightScale);
                _meshRenderer.SetBlendShapeWeight(1, valueY * _weightScale);
                _meshRenderer.SetBlendShapeWeight(2, valueZ * _weightScale);

                yield return null;
            }

             _spawnCoroutine = null;
        }

        public void Spawn()
        {
            if (_spawnCoroutine == null)
            _spawnCoroutine = StartCoroutine(Spawning());
        }

        public void ArrowSpawnHaptic()
        {
            blaster.ArrowSpawnHaptic();
        }
    }
}