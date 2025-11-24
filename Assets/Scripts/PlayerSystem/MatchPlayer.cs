using System;
using UnityEngine;

namespace Echobay.PlayerSystem
{
    public class MatchPlayer
    {
        public event Action<int> OnActionPointsChanged;

        public PlayerConfig Data { get; }
        public int ActionPoints { get; private set; }

        public MatchPlayer(PlayerConfig config)
        {
            Data = config;
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
