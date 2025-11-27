using DG.Tweening;
using Echobay.NetworkSystem.Match;
using Echobay.PlayerSystem;
using Fusion;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

namespace Echobay.MatchSystem
{
    public class InfoTextUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField, Range(1, 3)] private float _animDuration = 2f;
        [SerializeField] private Vector2 _fromToMove = new(-50, 50);

        private readonly Queue<string> _messageQueue = new();
        private bool _isShowing = false;

        private NetworkTurnController _networkTurnController;
        private NetworkMatchController _networkMatchController;

        [Inject]
        public void Construct(
            NetworkTurnController netTurns,
            NetworkMatchController matchInfo)
        {
            _networkTurnController = netTurns;
            _networkMatchController = matchInfo;
        }

        private void Awake()
        {
            _networkTurnController.OnRoundChanged += HandleRoundChanged;
            _networkTurnController.OnCurrentPlayerChanged += HandlePlayerChanged;

            _text.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            _networkTurnController.OnRoundChanged -= HandleRoundChanged;
            _networkTurnController.OnCurrentPlayerChanged -= HandlePlayerChanged;
        }

        private void HandleRoundChanged(int round)
        {
            if (round == 0) return;

            ShowTextQueued($"Round <color=red>{round}</color>");
        }

        private void HandlePlayerChanged(PlayerRef newPlayer)
        {
            if (newPlayer == PlayerRef.None) return;

            if (newPlayer == _networkMatchController.LocalPlayer.Data.PlayerRef)
            {
                ShowTextQueued("Your turn");
            }
            else
            {
                if (_networkMatchController.TryGetMatchPlayerByRef(newPlayer, out MatchPlayer matchPlayer))
                {
                    ShowTextQueued($"{matchPlayer.Data.Name} turn");
                }
            }
        }

        private void ShowTextQueued(string text)
        {
            _messageQueue.Enqueue(text);
            if (!_isShowing)
                ShowNext();
        }

        private void ShowNext()
        {
            if (_messageQueue.Count == 0)
            {
                _isShowing = false;
                return;
            }

            _isShowing = true;
            string text = _messageQueue.Dequeue();
            _text.text = text;
            _text.rectTransform.anchoredPosition = new Vector2(_fromToMove.x, 0);
            _text.gameObject.SetActive(true);

            _text.DOFade(1, 0.5f);
            _text.rectTransform.DOAnchorPosX(_fromToMove.y, _animDuration)
                .SetEase(Ease.OutCubic)
                .OnComplete(() =>
                {
                    _text.DOFade(0, 0.5f).OnComplete(() =>
                    {
                        _text.gameObject.SetActive(false);
                        ShowNext();
                    });
                });
        }
    }
}
