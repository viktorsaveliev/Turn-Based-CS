using DG.Tweening;
using Echobay.MatchSystem.TurnSystem;
using Echobay.PlayerSystem;
using TMPro;
using UnityEngine;
using Zenject;

namespace Echobay.MatchSystem
{
    public class InfoTextUI : MonoBehaviour, IMatchObserver, ITurnObserver
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField, Range(1, 3)] private float _animDuration = 2f;
        [SerializeField] private Vector2 _fromToMove = new(-50, 50);

        private IMatchMaster _matchMaster;
        private IMatchInfo _matchInfo;
        private ITurnMaster _master;
        private ITurnInfo _turnInfo;

        [Inject]
        public void Construct(IMatchMaster matchMaster, IMatchInfo matchInfo, ITurnMaster master, ITurnInfo turnInfo)
        {
            _matchMaster = matchMaster;
            _matchInfo = matchInfo;
            _master = master;
            _turnInfo = turnInfo;
        }

        private void Awake()
        {
            _matchMaster.Register(this);
            _master.Register(this);    
        }

        private void OnDestroy()
        {
            _matchMaster.Unregister(this);
            _master.Unregister(this);
        }

        public void OnMatchStarted()
        {
            _turnInfo.OnTurnGained += OnTurnGained;
        }

        public void OnMatchEnded()
        {
            _turnInfo.OnTurnGained -= OnTurnGained;
        }

        public void OnRoundStarted()
        {
            ShowText($"Round <color=red>{_master.CurrentRound}</color> started");
        }

        public void OnTurnEnded()
        {
            ShowText($"Round <color=red>{_master.CurrentRound}</color> ended");
        }

        private void OnTurnGained(Player player)
        {
            if (player == _matchInfo.LocalPlayer)
            {
                Debug.Log(player.Name + " turn");
                ShowText("Your turn");
            }
            else
            {
                Debug.Log(player.Name + "Opponent turn");
                ShowText("Opponent's turn");
            }
        }

        private void ShowText(string text)
        {
            _text.text = text;
            _text.rectTransform.anchoredPosition = new Vector2(_fromToMove.x, 0);

            _text.gameObject.SetActive(true);

            _text.DOFade(1, 0.5f);
            _text.rectTransform.DOAnchorPosX(_fromToMove.y, _animDuration).SetEase(Ease.OutCubic)
                .OnComplete(() =>
                {
                    _text.DOFade(0, 0.5f).OnComplete(() =>
                    {
                        _text.gameObject.SetActive(false);
                    });
                });
        }
    }
}
