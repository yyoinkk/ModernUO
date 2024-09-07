
using ModernUO.CodeGeneratedEvents;
using Server.Mobiles;
using System;
using System.Collections.Generic;

namespace Server.Spells.Light
{
    public class CelestialPowerSpell : LightSpell
    {
        private static readonly SpellInfo _info = new(
            "Celestial Power",
            "In Grav",
            203,
            9051,
            Reagent.SpidersSilk,
            Reagent.SulfurousAsh
        );

        public CelestialPowerSpell(Mobile caster, Item scroll) : base(caster, scroll, _info)
        {
        }

        public override SpellCircle Circle => SpellCircle.Second;

        private static readonly Dictionary<Mobile, TimerExecutionToken> _table = new();

        public override void OnCast()
        {
            if (CheckSequence())
            {
                if (!HasEffect(Caster))
                {
                    int duration = Caster.Skills.Magery.Value > 100 ? 40 : 30;

                    Timer.StartTimer(TimeSpan.FromSeconds(duration), () => ClearEffect(Caster), out var token);
                    _table[Caster] = token;

                    Caster.FixedParticles(0x3763, 15, 16, 0, 0, 0, EffectLayer.Head);
                    Caster.PlaySound(0x20B);
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
                m.SendMessage("Celestial Power ended.");
            }
        }

        public static bool HasEffect(Mobile m) => _table.ContainsKey(m);
    }
}
