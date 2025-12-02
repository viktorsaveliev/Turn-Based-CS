using Echobay.FightSystem.StatusEffects;
using System.Threading;

namespace Echobay
{
    public class Contexts
    {
        public struct ExecuteStatusEffectContext
        {
            public StatusEffectData Data;
            public StatusEffectableObject Attacker;
            public StatusEffectableObject Executer;
            public CancellationToken Token;

            public ExecuteStatusEffectContext(CancellationToken token)
            {
                Data = null;
                Attacker = null;
                Executer = null;
                Token = token;
            }

            public ExecuteStatusEffectContext(CancellationToken token, StatusEffectableObject attacker)
            {
                Data = null;
                Attacker = attacker;
                Executer = null;
                Token = token;
            }
        }
    }
}
