using Echobay.NetworkSystem.Lobby.Rooms;
using Echobay.UISystem;
using UnityEngine;
using Zenject;

namespace Echobay
{
    public class NicknameUpdateHandler : MonoBehaviour
    {
        [SerializeField] private NicknameField _nicknameField;

        private LocalRoomPlayer _localPlayer;

        [Inject]
        public void Construct(LocalRoomPlayer localPlayer)
        {
            _localPlayer = localPlayer;
        }

        private void OnEnable()
        {
            _nicknameField.SetNickname(_localPlayer.PlayerName);
            _nicknameField.OnNicknameChanged += OnNicknameChanged;
        }

        private void OnDisable()
        {
            _nicknameField.OnNicknameChanged -= OnNicknameChanged;
        }

        private void OnNicknameChanged(string nickname)
        {
            _localPlayer.SetNickname(nickname);
        }
    }
}
