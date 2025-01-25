using System;
using System.Collections;
using UnityEngine;

namespace V.Tool.JuicyFeeling
{
    public class SquashAndStretch : MonoBehaviour
    {
        private static event Action onSquashAndStretchAll;

        [SerializeField] private Transform affectTransfrom;
        public SquashStretchSO config;

        private Coroutine squashAndStretchCoroutine;
        private Coroutine squashAndStretchAllCoroutine;

        private Vector3 originScaleVector;

        private bool isReverse;

        // Flag 是否有選擇
        private bool affectX => (config.AxisToAffect & SquashStretchAxis.X) != 0;
        private bool affectY => (config.AxisToAffect & SquashStretchAxis.Y) != 0;
        private bool affectZ => (config.AxisToAffect & SquashStretchAxis.Z) != 0;
        
        public static void SquashAndStretchAll()
        {
            onSquashAndStretchAll?.Invoke();
        }

        #region Unity Fuc
        private void Awake() 
        {
            if(affectTransfrom == null)
            {
                affectTransfrom = transform;
            }

            originScaleVector = affectTransfrom.localScale;
        }

        private void Start() 
        {
            if(config.PlayOnStart)
            {
                StartSquashAndStretch();
            }
        }

        private void OnEnable() 
        {
            onSquashAndStretchAll += PlaySquashAndStretch;

            if(config.PlayOnEnable)
            {
                StartSquashAndStretch();
            }
        }
        private void OnDisable() 
        {
            if(squashAndStretchCoroutine != null)
            {
                StopCoroutine(squashAndStretchCoroutine);
            }

            onSquashAndStretchAll -= PlaySquashAndStretch;
        }
        #endregion

        #region Squash And Stretch
        public void PlaySquashAndStretch()
        {
            if (config.CanLoop && !config.CanOverwritten) 
            {
                return;
            }

            StartSquashAndStretch();
        }
        public void PlaySquashAndStretch(SquashStretchSO _SquashStretchSO)
        {
            if (config.CanLoop && !config.CanOverwritten) 
            {
                return;
            }

            StartSquashAndStretch();            
        }
        public void PlaySquashAndStretchAll()
        {
            if (config.CanLoop && !config.CanOverwritten) 
            {
                return;
            }

            StartSquashAndStretchAll();
        }

        private void StartSquashAndStretch()
        {
            if(config.AxisToAffect == SquashStretchAxis.None)
            {
                Debug.Log("No Affect Vector");
                return;
            }

            if(squashAndStretchCoroutine != null)
            {
                StopCoroutine(squashAndStretchCoroutine);

                if(config.CanPlayEveryTime && config.ResetScaleOrNot)
                {
                    affectTransfrom.localScale = originScaleVector;
                }
            }
            squashAndStretchCoroutine = StartCoroutine(Coroutine_SquashStretch());
        }

        private IEnumerator Coroutine_SquashStretch()
        {
            WaitForSeconds _loopingDelay = new WaitForSeconds(config.LoopingDelay);

            do
            {   
                // 依照機率播放
                if(!config.CanPlayEveryTime)
                {
                    if(UnityEngine.Random.Range(0f, 100f) > config.PlayPercentage)
                    {
                        yield return null;
                        continue;
                    }
                }

                if(config.CanReverseAfterPlaying)
                {
                    isReverse = !isReverse;
                }

                float _elapsedTimer = 0;
                Vector3 _originScale = originScaleVector;
                Vector3 _modifiedScale = _originScale;

                while (_elapsedTimer < config.Duration)
                {
                    _elapsedTimer += Time.deltaTime;
                    
                    // Curve Postion
                    float _curvePosition;

                    // 判斷是否要依照 Curve 擠壓或膨脹
                    if(isReverse)
                    {
                        _curvePosition = 1 - (_elapsedTimer / config.Duration);
                    }
                    else
                    {
                        _curvePosition = _elapsedTimer / config.Duration;
                    }

                    // Curve Value
                    float _curveValue = config.SquashStretchCurve.Evaluate(_curvePosition);
                    float _remapValue = config.OriginScale + (_curveValue * (config.MaxScale - config.OriginScale));    // 確保大小介於 Origin Scale and MaxScale

                    float _miniumThreshold = .0001f;
                    if(Mathf.Abs(_remapValue) < _miniumThreshold)
                    {
                        _remapValue = _miniumThreshold;
                        Debug.Log("Minimun");
                    }

                    _modifiedScale = CheckModifiedVector(_modifiedScale, _originScale, _remapValue);
                    affectTransfrom.localScale = _modifiedScale;

                    yield return null;
                }

                if(config.ResetScaleOrNot)
                {
                    affectTransfrom.localScale = _originScale;
                }

                if(config.CanLoop)
                {
                    yield return _loopingDelay;
                }
            }while(config.CanLoop);
        }
    
        private Vector3 CheckModifiedVector(Vector3 _modifiedScale, Vector3 _originScale, float _remapValue)
        {
            if (affectX)
            {
                _modifiedScale.x = _originScale.x * _remapValue;
            }
            else
            {
                _modifiedScale.x = _originScale.x / _remapValue;
            }

            if (affectY)
            {
                _modifiedScale.y = _originScale.y * _remapValue;
            }
            else
            {
                _modifiedScale.y = _originScale.y / _remapValue;
            }

            if (affectZ)
            {
                _modifiedScale.z = _originScale.z * _remapValue;
            }
            else
            {
                _modifiedScale.z = _originScale.z / _remapValue;
            }

            return _modifiedScale;
        }
        #endregion


        private void StartSquashAndStretchAll()
        {
            if(config.AxisToAffect == SquashStretchAxis.None)
            {
                Debug.Log("No Affect Vector");
                return;
            }

            if(squashAndStretchCoroutine != null)
            {
                StopCoroutine(squashAndStretchCoroutine);

                if(config.CanPlayEveryTime && config.ResetScaleOrNot)
                {

                    affectTransfrom.localScale = originScaleVector;

                }
            }
            squashAndStretchCoroutine = StartCoroutine(Coroutine_SquashStretchAll());
        }

        private IEnumerator Coroutine_SquashStretchAll()
        {
            WaitForSeconds _loopingDelay = new WaitForSeconds(config.LoopingDelay);

            do
            {   
                // 依照機率播放
                if(!config.CanPlayEveryTime)
                {
                    if(UnityEngine.Random.Range(0f, 100f) > config.PlayPercentage)
                    {
                        yield return null;
                        continue;
                    }
                }

                if(config.CanReverseAfterPlaying)
                {
                    isReverse = !isReverse;
                }

                float _elapsedTimer = 0;
                Vector3 _originScale = originScaleVector;
                Vector3 _modifiedScale = _originScale;

                while (_elapsedTimer < config.Duration)
                {
                    _elapsedTimer += Time.deltaTime;
                    
                    // Curve Postion
                    float _curvePosition;

                    // 判斷是否要依照 Curve 擠壓或膨脹
                    if(isReverse)
                    {
                        _curvePosition = 1 - (_elapsedTimer / config.Duration);
                    }
                    else
                    {
                        _curvePosition = _elapsedTimer / config.Duration;
                    }

                    // Curve Value
                    float _curveValue = config.SquashStretchCurve.Evaluate(_curvePosition);
                    float _remapValue = config.OriginScale + (_curveValue * (config.MaxScale - config.OriginScale));    // 確保大小介於 Origin Scale and MaxScale

                    float _miniumThreshold = .0001f;
                    if(Mathf.Abs(_remapValue) < _miniumThreshold)
                    {
                        _remapValue = _miniumThreshold;
                        Debug.Log("Minimun");
                    }

                    _modifiedScale = CheckModifiedVector(_modifiedScale, _originScale, _remapValue);
                    
                    affectTransfrom.localScale = _modifiedScale;


                    yield return null;
                }

                if(config.ResetScaleOrNot)
                {
                    affectTransfrom.localScale = _originScale;
                }

                if(config.CanLoop)
                {
                    yield return _loopingDelay;
                }
            }while(config.CanLoop);
        }
    }
}
