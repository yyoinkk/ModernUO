
using ModernUO.CodeGeneratedEvents;
using Server.Mobiles;
using System;
using System.Collections.Generic;

namespace Server.Spells.Dark
{
    public class PainReflectionSpell : DarkSpell
    {
        private static readonly SpellInfo _info = new(
            "Pain Reflection",
            "Rel Corp Jux",
            203,
            9051,
            Reagent.SpidersSilk,
            Reagent.Bone,
            Reagent.Ginseng
        );

        public PainReflectionSpell(Mobile caster, Item scroll) : base(caster, scroll, _info)
        {
        }

        public override SpellCircle Circle => SpellCircle.Fourth;

        private static readonly Dictionary<Mobile, TimerExecutionToken> _table = new();

        public override void OnCast()
        {
            if (!HasEffect(Caster))
            {
                var duration = SpellHelper.GetDuration(Caster, Caster, true);
                Timer.StartTimer(duration, () => ClearEffect(Caster), out var token);

                _table[Caster] = token;

                Caster.FixedParticles(0x3728, 12, 30, 0, 0, 0, EffectLayer.Waist);
                Caster.PlaySound(0x10C);
                Caster.PlaySound(0x11D);
            }
            else
            {
                Caster.SendLocalizedMessage(502173); // You are already under a similar effect.
            }
        }

        [OnEvent(nameof(PlayerMobile.PlayerDeathEvent))]
        public static void OnPlayerDeathEvent(Mobile m)
        {
            ClearEffect(m);
        }

        public static void ClearEffect(Mobile m)
        {
            if (_table.Remove(m, out var token))
            {
                token.Cancel();
                m.SendMessage("Pain reflection is over.");
            }
        }

        public static bool HasEffect(Mobile m) => _table.ContainsKey(m);
    }
}
