using Echobay.NetworkSystem.Lobby.Rooms;
using Echobay.UnitSystem;
using UnityEngine;
using Zenject;

namespace Echobay
{
    public class LobbyInstaller : MonoInstaller
    {
        private LocalRoomPlayer _localRoomPlayer;

        private void Awake()
        {
            Resolve();
            _localRoomPlayer.Initialize();
        }

        public override void InstallBindings()
        {
            Container.Bind<LocalRoomPlayer>().FromNew().AsSingle();
            Container.Bind<SelectUnitHandler>().FromNew().AsSingle();
        }

        private void Resolve()
        {
            _localRoomPlayer = Container.Resolve<LocalRoomPlayer>();
        }
    }
}
