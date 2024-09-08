using Server.Mobiles;
using Server.Targeting;
using System.Collections.Generic;
using ModernUO.CodeGeneratedEvents;

namespace Server.Spells.Druid
{
    public class PoisonProtectionSpell : DruidSpell, ITargetingSpell<Mobile>
    {
        private static readonly SpellInfo _info = new(
            "Poison Protection",
            "Des Nox",
            203,
            9051,
            Reagent.Garlic
        );

        public PoisonProtectionSpell(Mobile caster, Item scroll) : base(caster, scroll, _info)
        {
        }

        public override SpellCircle Circle => SpellCircle.First;

        private static readonly Dictionary<Mobile, TimerExecutionToken> _table = new();

        public override void OnCast()
        {
            Caster.Target = new SpellTarget<Mobile>(this, TargetFlags.Beneficial);
        }

        public void Target(Mobile m)
        {
            if (CheckBSequence(m))
            {
                if (!HasEffect(m))
                {
                    SpellHelper.Turn(Caster, m);

                    AddEffect(Caster, m);
                }
                else if (m == Caster)
                {
                    Caster.SendLocalizedMessage(502173); // You are already under a similar effect.
                }
                else
                {
                    Caster.SendLocalizedMessage(501775); // This spell is already in effect.
                }
            }
        }

        [OnEvent(nameof(PlayerMobile.PlayerDeathEvent))]
        public static void OnPlayerDeathEvent(Mobile m)
        {
            ClearEffect(m);
        }

        public static void AddEffect(Mobile caster, Mobile m)
        {
            var duration = SpellHelper.GetDuration(caster, m, true);

            var value = (int)(caster.Skills[SkillName.Magery].Value / 12) + Utility.RandomMinMax(6, 10);
            var physResMod = new ResistanceMod(ResistanceType.Poison, "PoisonProtection", value, m);

            m.AddResistanceMod(physResMod);

            Timer.StartTimer(duration, () => ClearEffect(m), out var token);
            _table[m] = token;

            m.PlaySound(0x1ED);
            m.FixedParticles(0x3740, 11, 10, 5016, EffectLayer.Waist);
        }

        public static void ClearEffect(Mobile m)
        {
            if (_table.Remove(m, out var token))
            {
                token.Cancel();
                m.RemoveResistanceMod("PoisonProtection");
            }
        }

        public static bool HasEffect(Mobile m) => _table.ContainsKey(m);
    }
}
