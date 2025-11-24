using Echobay.NetworkSystem.Lobby.Rooms;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Echobay.NetworkSystem.Lobby.Ui
{
    public class PlayerInfoElement : MonoBehaviour
    {
        public NetworkRoomPlayer Player { get; private set; }

        [SerializeField] private TMP_Text _nickname;
        [SerializeField] private TMP_Text _level;
        [SerializeField] private Image _readyIndicator;
        [SerializeField] private Sprite _notReadySprite;
        [SerializeField] private Sprite _readySprite;

        public void Init(NetworkRoomPlayer player)
        {
            Player = player;

            _nickname.text = player.PlayerName;
            _level.text = $"LVL {player.Level}";
            _readyIndicator.sprite = _notReadySprite;

            SetStatus(player.IsReady);
        }

        public void SetStatus(bool ready)
        {
            _readyIndicator.sprite = ready ? _readySprite : _notReadySprite;
        }
    }
}
