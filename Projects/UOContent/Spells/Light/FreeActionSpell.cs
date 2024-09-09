
using System;
using Server.Mobiles;
using System.Collections.Generic;
using ModernUO.CodeGeneratedEvents;

namespace Server.Spells.Light
{
    public class FreeActionSpell : LightSpell
    {
        private static readonly SpellInfo _info = new(
            "Free Action",
            "In Por Sanct",
            203,
            9051,
            Reagent.BlackPearl,
            Reagent.Ginseng
        );

        public FreeActionSpell(Mobile caster, Item scroll) : base(caster, scroll, _info)
        {
        }

        public override SpellCircle Circle => SpellCircle.Third;

        private static readonly TimeSpan CD = TimeSpan.FromSeconds(10);

        private static readonly Dictionary<Mobile, TimerExecutionToken> _table = new();

        public override void OnCast()
        {
            if (CheckSequence())
            {
                if (!HasEffect(Caster) && !HasCD(Caster))
                {
                    _table[Caster] = new TimerExecutionToken();

                    Caster.FixedParticles(0x376A, 15, 16, 0, 2068, 0, EffectLayer.Head);
                    Caster.PlaySound(0x1E8);
                }
                else
                {
                    Caster.SendLocalizedMessage(502173); // You are already under a similar effect.
                }
            }
        }

        [OnEvent(nameof(PlayerMobile.PlayerDeathEvent))]
        public static void OnPlayerDeathEvent(Mobile m)
        {
            StartCD(m);
        }

        public static void ClearEffect(Mobile m)
        {
            if (_table.Remove(m, out var token))
            {
                token.Cancel();
            }
        }

        public static void StartCD(Mobile m)
        {
            if (HasEffect(m))
            {
                Timer.StartTimer(CD, () => ClearEffect(m), out var token);
                _table[m] = token;
                m.SendMessage("Free Action canceled.");
            }
        }

        public static bool HasCD(Mobile m) => _table.ContainsKey(m) && _table[m].Running;

        public static bool HasEffect(Mobile m) => _table.ContainsKey(m) && _table[m].Running != true;
    }
}
