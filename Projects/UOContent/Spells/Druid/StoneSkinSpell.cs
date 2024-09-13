
using ModernUO.CodeGeneratedEvents;
using Server.Mobiles;
using System;
using System.Collections.Generic;

namespace Server.Spells.Druid
{
    public class StoneSkinSpell : DruidSpell
    {
        private static readonly SpellInfo _info = new(
            "Stone Skin",
            "In Ylem Sanct",
            203,
            9051,
            Reagent.SpidersSilk,
            Reagent.Bloodmoss
        );

        public StoneSkinSpell(Mobile caster, Item scroll) : base(caster, scroll, _info)
        {
        }

        public override SpellCircle Circle => SpellCircle.Third;

        private static readonly Dictionary<Mobile, TimerExecutionToken> _table = new();

        public override void OnCast()
        {
            if (CheckSequence())
            {
                if (!HasEffect(Caster))
                { 
                    TimeSpan duration = TimeSpan.FromSeconds(GetDamageFixed(Caster) / 25);

                    Timer.StartTimer(duration, () => ClearEffect(Caster), out var token);
                    _table[Caster] = token;

                    Caster.SendMessage($"Stone skin Duration: {duration:mm\\:ss}s.");

                    Caster.FixedParticles(0x3740, 15, 16, 0, 0, 0, EffectLayer.Head);
                    Caster.PlaySound(0x1ED);
                }
                else
                {
                    Caster.SendLocalizedMessage(502173); // You are already under a similar effect.
                }
            }
        }

        [OnEvent(nameof(PlayerMobile.PlayerDeathEvent))]
        public static void OnPlayerDeathEvent(Mobile m) => ClearEffect(m);

        public static void ClearEffect(Mobile m)
        {
            if (_table.Remove(m, out var token))
            {
                token.Cancel();
                m.SendMessage("Stone skin is over.");
            }
        }

        public static bool HasEffect(Mobile m) => _table.ContainsKey(m);
    }
}
