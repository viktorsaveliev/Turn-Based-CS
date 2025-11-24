using Echobay.UnitSystem;
using System.Collections.Generic;
using UnityEngine;

namespace Echobay.PlayerSystem
{
    public interface IPlayerData
    {
        public string PlayerName { get; }
        public int TeamID { get; }
        public int[] UnitsDataID { get; }
    }

}
