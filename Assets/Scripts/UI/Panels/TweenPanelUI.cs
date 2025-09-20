using Sirenix.OdinInspector;
using UnityEngine;

namespace Echobay.UISystem
{
    public class TweenPanelUI : PanelUI
    {
        [SerializeField] private bool _showOnStart;

        [Space]

        [Title("Animation Settings")]
        [SerializeField] private TweenAnimation _showAnimation;
        [SerializeField] private TweenAnimation _hideAnimation;

        private void OnValidate()
        {
            if (_showAnimation.CanvasGroup == null)
            {
                _showAnimation.CanvasGroup = Group;
            }

            if (_hideAnimation.CanvasGroup == null)
            {
                _hideAnimation.CanvasGroup = Group;
            }
        }

        private void Awake()
        {
            if (_showOnStart)
            {
                Show();
            }
        }

        public override void Show()
        {
            base.Show();

            _showAnimation.Play();
        }

        public override void Hide()
        {
            _hideAnimation.Play();
            _hideAnimation.OnComplete += OnHide;
        }

        private void OnHide()
        {
            _hideAnimation.OnComplete -= OnHide;
            base.Hide();
        }
    }
}