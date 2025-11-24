using Echobay.NetworkSystem.Lobby.Rooms;
using System;
using Zenject;

namespace Echobay.UnitSystem
{
    public class SelectUnitHandler
    {
        public const int MaxUnits = 3;
        private const int EmptySlot = -1;

        private readonly UnitsDatabase _unitsDatabase;
        private readonly LocalRoomPlayer _localRoomPlayer;

        private readonly int[] _temporarySelectedUnits = new int[MaxUnits];

        [Inject]
        public SelectUnitHandler(UnitsDatabase unitsDatabase, LocalRoomPlayer localRoomPlayer)
        {
            _unitsDatabase = unitsDatabase;
            _localRoomPlayer = localRoomPlayer;
        }

        public bool TryAddUnit(int unitID)
        {
            if (HasUnit(unitID)) return false;

            int freeIndex = GetFirstFreeSlotIndex();
            if (freeIndex < 0) return false;

            _temporarySelectedUnits[freeIndex] = unitID;
            return true;
        }

        public bool TryRemoveUnit(int unitID)
        {
            int idx = GetSlotIndexOfUnit(unitID);
            if (idx < 0) return false;

            _temporarySelectedUnits[idx] = EmptySlot;
            return true;
        }

        public bool HasUnit(UnitData unit)
        {
            return _unitsDatabase.TryGetUnitID(unit, out int id) && HasUnit(id);
        }

        public bool HasUnit(int unitID)
        {
            return GetSlotIndexOfUnit(unitID) >= 0;
        }

        public int GetOccupiedSlotCount()
        {
            int c = 0;
            for (int i = 0; i < MaxUnits; i++)
                if (_temporarySelectedUnits[i] != EmptySlot) c++;
            return c;
        }

        public bool IsFullySlots() => GetOccupiedSlotCount() == MaxUnits;

        public void ConfirmSelection()
        {
            int[] copy = new int[MaxUnits];

            for (int i = 0; i < MaxUnits; i++) copy[i] = _temporarySelectedUnits[i];
            _localRoomPlayer.SetUnits(copy);
        }

        public void CancelSelection()
        {
            SyncWithLocalPlayer();
        }

        public void SyncWithLocalPlayer()
        {
            for (int i = 0; i < MaxUnits; i++) _temporarySelectedUnits[i] = EmptySlot;

            if (_localRoomPlayer.UnitsDataID == null) return;

            int len = Math.Min(_localRoomPlayer.UnitsDataID.Length, MaxUnits);
            for (int i = 0; i < len; i++)
            {
                _temporarySelectedUnits[i] = _localRoomPlayer.UnitsDataID[i];
            }
        }

        public int GetSlotIndexOfUnit(int unitID)
        {
            for (int i = 0; i < MaxUnits; i++)
            {
                if (_temporarySelectedUnits[i] == unitID) return i;
            }

            return -1;
        }

        public int GetFirstFreeSlotIndex()
        {
            for (int i = 0; i < MaxUnits; i++)
            {
                if (_temporarySelectedUnits[i] == EmptySlot) return i;
            }

            return -1;
        }

        public int[] GetTemporaryUnitsCopy()
        {
            int[] copy = new int[MaxUnits];
            Array.Copy(_temporarySelectedUnits, copy, MaxUnits);
            return copy;
        }
    }
}
