using Echobay.NetworkSystem.Match;
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
            _matchController.LocalPlayer.OnActionPointsChanged += OnActionPointsChanged;
        }

        private void OnDisable()
        {
            _matchController.LocalPlayer.OnActionPointsChanged -= OnActionPointsChanged;
        }

        private void OnActionPointsChanged(int actionPoints)
        {
            _pointsText.text = $"{actionPoints}";
        }
    }
}
