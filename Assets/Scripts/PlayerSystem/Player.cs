using System;
using UnityEngine;

namespace Echobay.PlayerSystem
{
    public class Player : IPlayerTurn
    {
        public string Name { get; private set; }
        public int TeamID { get; private set; }
        public int ActionPoints { get; private set; }

        public Player(PlayerConfig config)
        {
            Name = config.Name;
            TeamID = config.TeamID;
        }

        public void SetActionPoints(int actionPoints)
        {
            ActionPoints = actionPoints;
        }

        public void GiveActionPoints(int actionPoints)
        {
            ActionPoints += actionPoints;
        }

        public bool SpendPointsAndCheckTurnEnd(int points)
        {
            if (points > ActionPoints)
            {
                Debug.LogError("Points > TurnPointsRemaining");
                return true;
            }

            ActionPoints -= points;

            if (ActionPoints <= 0)
            {
                return true;
            }

            return false;
        }

        public void PassTurn()
        {

        }

        public void EndTurn()
        {

        }
    }
}
