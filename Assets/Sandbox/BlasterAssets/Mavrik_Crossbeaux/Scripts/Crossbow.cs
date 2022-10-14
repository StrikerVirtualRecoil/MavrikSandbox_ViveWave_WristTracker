using System;
using System.Collections;
using UnityEngine;

namespace Script
{
    public class Crossbow : MonoBehaviour
    {
        [SerializeField] private CrossbowAnimator _animator;
        [SerializeField] private Transform _bulletSpot;
        [SerializeField] private TrailRenderer _trail;
        

        [SerializeField] private float _loadingSpeed;
        [SerializeField] private float _arrowSpeed;

        [SerializeField] private Arrow[] _arrowInstance;

        private bool _isLoaded;
        private float _loadingProgress;

        private Coroutine _movingCoroutine;
        private Arrow _arrow;

        public int _currentArrowInstance = 1;

        private bool reloadOne;
        private bool reloadTwo;
        private bool reloadThree;
        private bool reloadFour;
        private bool reloadFive;
    
        public Blaster blaster;

        private void Awake()
        {
          _trail.enabled = false;

            reloadOne = false;
            reloadTwo = false;
            reloadThree = false;
            reloadFour = false;
            reloadFive = false;
       
        }

        public void StartLoading()
        {
            if (_isLoaded || _movingCoroutine != null)
                return;
            _movingCoroutine = StartCoroutine(MoveLoading());
        }

        public void Shoot()
        {
            if (_isLoaded == false || _movingCoroutine != null)
                return;

            _animator.PressTrigger();
      
            _movingCoroutine = StartCoroutine(ShootDelay());
        }

        public void SwapArrow()
        {
            if (_arrowInstance.Length == 0)
            {
                Debug.LogError("Don't set any arrow instance");
                return;
            }

            _currentArrowInstance = (_currentArrowInstance + 1) % _arrowInstance.Length;
            _animator.SwapArrow();
      
            if (_arrow != null)
            {
                Destroy(_arrow.gameObject);
                SpawnArrow();
            }
        }

        private IEnumerator MoveLoading()
        {
           
            _loadingProgress = 0f;

            while (_loadingProgress < 1f)
            {
                //_loadingProgress += Time.deltaTime * _loadingSpeed;
                if (blaster.mavrik.GetRawSensorDown(blaster.slideBar11) || blaster.mavrik.GetRawSensorDown(blaster.slideBar12))
                {
                    _loadingProgress = Mathf.MoveTowards(_loadingProgress, 0.167f, Time.deltaTime * _loadingSpeed);
                    _animator.LoadProgress(_loadingProgress);
                    reloadOne = true;
                    CrossBowPullOneHaptic();
                }
                else if (blaster.mavrik.GetRawSensorDown(blaster.slideBar9) || blaster.mavrik.GetRawSensorDown(blaster.slideBar10) && reloadOne)
                {
                    _loadingProgress = Mathf.MoveTowards(_loadingProgress, .334f, Time.deltaTime * _loadingSpeed);
                    _animator.LoadProgress(_loadingProgress);
                    reloadTwo = true;
                    CrossBowPullTwoHaptic();
                }
                else if (blaster.mavrik.GetRawSensorDown(blaster.slideBar7) || blaster.mavrik.GetRawSensorDown(blaster.slideBar8) && reloadTwo)
                {
                    _loadingProgress = Mathf.MoveTowards(_loadingProgress, .501f, Time.deltaTime * _loadingSpeed);
                    _animator.LoadProgress(_loadingProgress);
                    reloadThree = true;
                    CrossBowPullThreeHaptic();
                }
                else if (blaster.mavrik.GetRawSensorDown(blaster.slideBar5) || blaster.mavrik.GetRawSensorDown(blaster.slideBar6) && reloadThree)
                {
                    _loadingProgress = Mathf.MoveTowards(_loadingProgress, .668f, Time.deltaTime * _loadingSpeed);
                    _animator.LoadProgress(_loadingProgress);
                    reloadFour = true;
                    CrossBowPullFourHaptic();
                }
                else if (blaster.mavrik.GetRawSensorDown(blaster.slideBar3) || blaster.mavrik.GetRawSensorDown(blaster.slideBar4) && reloadFour)
                {
                    _loadingProgress = Mathf.MoveTowards(_loadingProgress, .834f, Time.deltaTime * _loadingSpeed);
                    _animator.LoadProgress(_loadingProgress);
                    reloadFive = true;
                    CrossBowPullFiveHaptic();
                }
                else if (blaster.mavrik.GetRawSensorDown(blaster.slideBar1) || blaster.mavrik.GetRawSensorDown(blaster.slideBar2) && reloadFive)
                {
                    _loadingProgress = Mathf.MoveTowards(_loadingProgress, 1.2f, Time.deltaTime * _loadingSpeed);
                    _animator.LoadProgress(_loadingProgress);
                    CrossBowPullSnapHaptic();
                    reloadOne = false;
                    reloadTwo = false;
                    reloadThree = false;
                    reloadFour = false;
                    reloadFive = false;
                }
                // _animator.LoadProgress(_loadingProgress);
                yield return null;
            }

            _loadingProgress = 1f;
            _movingCoroutine = null;
            _isLoaded = true;

            if (_arrow == null && _arrowInstance.Length > 0 ) 
                SpawnArrow();
        }

        private void SpawnArrow()
        {
            _arrow = Instantiate(original: _arrowInstance[_currentArrowInstance], parent: _bulletSpot);
        }

        private IEnumerator ShootDelay()
        {
            _trail.enabled = true;
            _trail.Clear();
      
            yield return new WaitForSeconds(0.3f);

            _animator.Shoot();
            _loadingProgress = 0f;
            _isLoaded = false;

            if (_arrow != null)
            {
                _arrow.Launch(_arrowSpeed);
                _arrow = null;
            }

            yield return new WaitForSeconds(0.7f);
      
            _trail.enabled = false;
            _movingCoroutine = null;
        }

        public void CrossBowShootHaptic()
        {
            blaster.CrossBowShootHaptic();
        }

        public void CrossBowSlideHaptic()
        {
            blaster.CrossBowSlideHaptic();
        }

        public void CrossBowPullOneHaptic()
        {
            blaster.CrossBowPullOneHaptic();
        }

        public void CrossBowPullTwoHaptic()
        {
            blaster.CrossBowPullTwoHaptic();
        }

        public void CrossBowPullThreeHaptic()
        {
            blaster.CrossBowPullThreeHaptic();
        }

        public void CrossBowPullFourHaptic()
        {
            blaster.CrossBowPullFourHaptic();
        }

        public void CrossBowPullFiveHaptic()
        {
            blaster.CrossBowPullFiveHaptic();
        }

        public void CrossBowPullSnapHaptic()
        {
            blaster.CrossBowPullSnapHaptic();
        }

        public void RopeHaptic()
        {
            blaster.RopeHaptic();
        }

        
    }

}
