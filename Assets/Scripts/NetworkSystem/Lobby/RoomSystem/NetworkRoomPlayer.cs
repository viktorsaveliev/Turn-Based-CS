using Echobay.PlayerSystem;
using Fusion;
using System;
using UnityEngine;
using Zenject;

namespace Echobay.NetworkSystem.Lobby.Rooms
{
    public class NetworkRoomPlayer : NetworkBehaviour, IPlayerData
    {
        public event Action<NetworkRoomPlayer> OnPlayerReadyChanged;

        [Networked] public bool IsReady { get; private set; }
        [Networked] public NetworkString<_16> NetworkPlayerName { get; private set; }
        [Networked] public int TeamID { get; private set; }
        [Networked] public int Level { get; private set; }

        public string PlayerName => NetworkPlayerName.ToString();
        public int[] UnitsDataID { get; private set; }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        public void RPC_SetupPlayer(PlayerConfig playerConfig)
        {
            NetworkPlayerName = playerConfig.Name;
            gameObject.name = PlayerName;

            TeamID = playerConfig.TeamID;

            UnitsDataID = new int[playerConfig.UnitsDataID.Length];
            playerConfig.UnitsDataID.CopyTo(UnitsDataID);

            Debug.Log($"Player {NetworkPlayerName} setup on host with {UnitsDataID.Length} warriors");
        }

        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        public void RPC_SetReady(bool ready)
        {
            IsReady = ready;
            RPC_OnReadyChanged();

            Debug.Log($"{PlayerName} ready = {ready}");
        }

        [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
        public void RPC_SetNickname(string nickname)
        {
            NetworkPlayerName = nickname;
            gameObject.name = nickname;
        }

        [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
        public void RPC_SetUnits(int[] unitsIDs)
        {
            UnitsDataID = unitsIDs;
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void RPC_OnReadyChanged()
        {
            OnPlayerReadyChanged?.Invoke(this);
        }
    }
}
