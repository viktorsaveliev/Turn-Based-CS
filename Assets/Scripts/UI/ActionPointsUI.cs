using Echobay.NetworkSystem.Match;
using System.Collections;
using TMPro;
using UnityEngine;
using Zenject;

namespace Echobay.UISystem
{
    public class ActionPointsUI : PanelUI
    {
        [SerializeField] private TMP_Text _pointsText;

        private NetworkMatchController _matchController;

        [Inject]
        public void Construct(NetworkMatchController matchController)
        {
            _matchController = matchController;
        }

        private void OnEnable()
        {
            StartCoroutine(SubscribeProcess());
        }

        private void OnDisable()
        {
            if (_matchController.LocalPlayer != null)
            {
                _matchController.LocalPlayer.OnActionPointsChanged -= OnActionPointsChanged;
            }
        }

        private void OnActionPointsChanged(int actionPoints)
        {
            _pointsText.text = $"{actionPoints}";
        }

        private IEnumerator SubscribeProcess()
        {
            while (true)
            {
                if (_matchController.LocalPlayer == null)
                {
                    yield return null;
                }

                _matchController.LocalPlayer.OnActionPointsChanged += OnActionPointsChanged;
                break;
            }
        }
    }
}
