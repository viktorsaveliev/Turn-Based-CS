using Echobay.FightSystem.StatusEffects;
using System.Threading;

namespace Echobay
{
    public class Contexts
    {
        public struct ExecuteStatusEffectContext
        {
            public StatusEffectableObject Attacker;
            public CancellationToken Token;

            public ExecuteStatusEffectContext(CancellationToken token)
            {
                Attacker = null;
                Token = token;
            }

            public ExecuteStatusEffectContext(CancellationToken token, StatusEffectableObject attacker)
            {
                Attacker = attacker;
                Token = token;
            }
        }
    }
}
