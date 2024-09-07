
using System;
using Server.Mobiles;
using System.Collections.Generic;
using ModernUO.CodeGeneratedEvents;

namespace Server.Spells.Light
{
    public class SpiritArmorSpell : LightSpell
    {
        private static readonly SpellInfo _info = new(
            "Spirit Armor",
            "Sanct",
            203,
            9051,
            Reagent.Garlic,
            Reagent.SpidersSilk
        );

        public SpiritArmorSpell(Mobile caster, Item scroll) : base(caster, scroll, _info)
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
                    int duration = GetDamageSkill(Caster) > 80 ? 80 : 60;

                    Timer.StartTimer(TimeSpan.FromSeconds(duration), () => ClearEffect(Caster), out var token);
                    _table[Caster] = token;

                    Caster.FixedParticles(0x373A, 15, 16, 0, 0, 0, EffectLayer.Head);
                    Caster.PlaySound(0x1E7);
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
                m.SendMessage("Spirit Armor faded.");
            }
        }

        public static void OnDamage(Mobile m, int damage)
        {
            if (HasEffect(m) && m.Alive)
            {
                int heal = (int)(damage * 0.6);
                int mana = (int)(damage * 0.4);
                m.Hits += heal;
                m.Mana -= mana;
                m.SendMessage($"Healed: {heal}, mana lost: {mana}");
            }
            else
            {
                ClearEffect(m);
            }
        }

        public static bool HasEffect(Mobile m) => _table.ContainsKey(m);
    }
}
