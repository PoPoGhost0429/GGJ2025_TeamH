using System;
using UnityEngine;

namespace V.Tool.JuicyFeeling
{
    [CreateAssetMenu(fileName = "Squash & Strecth", menuName = "SO/JuicyFeeling/Squash & Stretch Config", order = 0)]
    public class SquashStretchSO : ScriptableObject
    {
        [Header("SquashAndStretch")]
        public SquashStretchAxis AxisToAffect = SquashStretchAxis.Y;
        public bool CanOverwritten;
        public bool PlayOnStart;
        public bool PlayOnEnable = false;
        public bool CanPlayEveryTime = true;
        [Range(0, 100f), Tooltip("播放的機率")]
        public float PlayPercentage = 100f;  

        [Range(0, 1f)] 
        public float Duration = .25f;

        [Header("Animation Setting")]
        public float OriginScale = 1f;
        public float MaxScale = 1.5f;
        public bool ResetScaleOrNot;
        public AnimationCurve SquashStretchCurve;
        public bool CanReverseAfterPlaying; // 依照擠壓的 Curve 放大

        [Header("Looping")]
        public bool CanLoop;
        public float LoopingDelay = .5f;
    }

    [Flags]
    public enum SquashStretchAxis
    {
        None = 0,
        X = 1,
        Y = 2,
        Z = 4,
    }
}
