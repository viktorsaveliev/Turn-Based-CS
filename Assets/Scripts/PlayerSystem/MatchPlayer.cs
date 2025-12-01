using Echobay.UnitSystem;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Echobay.PlayerSystem
{
    public class MatchPlayer
    {
        public event Action<int> OnActionPointsChanged;

        public PlayerConfig Data { get; }
        public int ActionPoints { get; private set; }
        public IReadOnlyCollection<Unit> Units => _units;

        private readonly HashSet<Unit> _units = new();

        public MatchPlayer(PlayerConfig config)
        {
            Data = config;
        }

        public void AddUnit(Unit unit)
        {
            _units.Add(unit);
        }

        public void RemoveUnit(Unit unit)
        {
            _units.Remove(unit);
        }

        public void SetActionPoints(int amount)
        {
            ActionPoints = amount;
            OnActionPointsChanged?.Invoke(ActionPoints);
        }

        public bool SpendPoints(int amount)
        {
            if (amount > ActionPoints)
            {
                Debug.LogError("Not enough AP");
                return false;
            }

            ActionPoints -= amount;
            OnActionPointsChanged?.Invoke(ActionPoints);

            return ActionPoints <= 0;
        }
        
        public void EndTurn()
        {

        }
    }
}
