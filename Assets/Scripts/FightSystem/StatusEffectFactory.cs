using UnityEngine;

namespace Echobay.FightSystem.StatusEffects
{
    public class StatusEffectFactory
    {
        public StatusEffect Create(StatusEffectData data)
        {
            var json = JsonUtility.ToJson(data.EffectPrefab);
            var copy = (StatusEffect)JsonUtility.FromJson(json, data.EffectPrefab.GetType());

            copy.Init(data);

            return copy;
        }
    }
}
