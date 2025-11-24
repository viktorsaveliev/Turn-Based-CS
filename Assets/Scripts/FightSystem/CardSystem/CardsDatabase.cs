using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Echobay.CardSystem
{
    [CreateAssetMenu(fileName = "CardsDatabase", menuName = "Cards/CardsDatabase")]
    public class CardsDatabase : ScriptableObject
    {
        public IReadOnlyCollection<CardData> Cards => _cards;

        [SerializeField] private List<CardData> _cards;

        public bool TryGetCardDataByID(int id, out CardData cardData)
        {
            if (_cards == null || _cards.Count <= id)
            {
                cardData = null;

                Debug.LogError("[CD]: problems detected");
                return false;
            }

            cardData = _cards[id];
            return true;
        }

        public bool TryGetCardID(CardData cardData, out int cardID)
        {
            cardID = 0;

            if (_cards == null)
            {
                Debug.LogError("[CD]: Cards database is null");
                return false;
            }

            for (int i = 0; i < _cards.Count; i++)
            {
                if (cardData != _cards[i]) continue;
                cardID = i;
                return true;
            }

            Debug.LogError($"[CD]: Target card [{cardData.Name}] was not found");
            return false;
        }

#if UNITY_EDITOR
        [Button]
        private void FindAllObjectData()
        {
            _cards.Clear();
            string[] guids = AssetDatabase.FindAssets("t:CardData");

            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                CardData obj = AssetDatabase.LoadAssetAtPath<CardData>(path);
                _cards.Add(obj);
            }

            Debug.Log($"CardsDatabase updated: found {_cards.Count} items.");
        }
#endif
    }
}
