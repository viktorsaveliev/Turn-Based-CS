using System;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using DG.Tweening;

namespace Echobay.UISystem
{
    [Serializable]
    public class TweenAnimation
    {
        public event Action OnComplete;

        #region Variables
        public enum TargetType
        {
            Image,
            CanvasGroup
        }

        public TargetType Target;

        [ShowIf(nameof(Target), TargetType.Image)]
        public Image Image;

        [ShowIf(nameof(Target), TargetType.CanvasGroup)]
        public CanvasGroup CanvasGroup;

        [Space]
        [Title("Vectors")]

        public bool MovePosition;
        [ShowIf(nameof(MovePosition), true)]
        public Vector2 TargetPosition;
        [ShowIf(nameof(MovePosition), true), Range(0, 20)]
        public float MoveDuration;
        [ShowIf(nameof(MovePosition), true)]
        public Ease MoveEase;

        [Space]

        public bool MoveRotation;
        [ShowIf(nameof(MoveRotation), true)]
        public Vector2 TargetRotation;
        [ShowIf(nameof(MoveRotation), true), Range(0, 20)]
        public float RotateDuration;
        [ShowIf(nameof(MoveRotation), true)]
        public Ease RotateEase;

        [Space]

        public bool ChangeScale;
        [ShowIf(nameof(ChangeScale), true)]
        public Vector2 TargetScale;
        [ShowIf(nameof(ChangeScale), true), Range(0, 20)]
        public float ScaleDuration;
        [ShowIf(nameof(ChangeScale), true)]
        public Ease ScaleEase;

        [Space]
        [Title("Visual")]

        public bool Fade;
        [ShowIf(nameof(Fade), true), Range(0, 1)]
        public float TargetFadeValue;
        [ShowIf(nameof(Fade), true), Range(0, 20)]
        public float FadeDuration;
        [ShowIf(nameof(Fade), true)]
        public Ease FadeEase;

        [Space]

        [ShowIf(nameof(Target), TargetType.Image)]
        public bool ChangeColor;
        [ShowIf("@this.ChangeColor && this.Target == TargetType.Image")]
        public Color TargetColor;
        [ShowIf("@this.ChangeColor && this.Target == TargetType.Image"), Range(0, 20)]
        public float ColorDuration;
        [ShowIf("@this.ChangeColor && this.Target == TargetType.Image")]
        public Ease ColorEase;

        #endregion

        public void Play()
        {
            RectTransform rectTransform = Target == TargetType.Image ? Image.rectTransform : CanvasGroup.GetComponent<RectTransform>();

            int animationsCount = 0;
            int completedAnimations = 0;

            void AnimationCompleted()
            {
                completedAnimations++;
                if (completedAnimations == animationsCount)
                {
                    OnComplete?.Invoke();
                }
            }

            void StartAnimation(Tweener tweener)
            {
                animationsCount++;
                tweener.OnComplete(AnimationCompleted);
            }

            if (MovePosition)
            {
                StartAnimation(rectTransform.DOAnchorPos(TargetPosition, MoveDuration).SetEase(MoveEase));
            }

            if (MoveRotation)
            {
                StartAnimation(rectTransform.DORotate(TargetRotation, RotateDuration).SetEase(RotateEase));
            }

            if (ChangeScale)
            {
                StartAnimation(rectTransform.DOScale(TargetScale, ScaleDuration).SetEase(ScaleEase));
            }

            if (ChangeColor && Target == TargetType.Image)
            {
                StartAnimation(Image.DOColor(TargetColor, ColorDuration));
            }

            if (Fade)
            {
                if (Target == TargetType.Image)
                {
                    StartAnimation(Image.DOFade(TargetFadeValue, FadeDuration).SetEase(FadeEase));
                }
                else if (Target == TargetType.CanvasGroup)
                {
                    StartAnimation(CanvasGroup.DOFade(TargetFadeValue, FadeDuration).SetEase(FadeEase));
                }
            }

            if (animationsCount == 0)
            {
                OnComplete?.Invoke();
            }
        }
    }
}