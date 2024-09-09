using Server.Mobiles;
using Server.Targeting;
using System.Collections.Generic;
using ModernUO.CodeGeneratedEvents;
using Server.Spells.Light;

namespace Server.Spells.Second
{
    public class ProtectionSpell : MagerySpell, ITargetingSpell<Mobile>
    {
        private static readonly SpellInfo _info = new(
            "Protection",
            "Uus Sanct",
            236,
            9011,
            Reagent.Garlic,
            Reagent.Ginseng,
            Reagent.SulfurousAsh
        );

        private static readonly Dictionary<Mobile, TimerExecutionToken> _table = new();

        public ProtectionSpell(Mobile caster, Item scroll = null) : base(caster, scroll, _info)
        {
        }

        public override SpellCircle Circle => SpellCircle.Second;

        public override void OnCast()
        {
            Caster.Target = new SpellTarget<Mobile>(this, TargetFlags.Beneficial);






            //if (Core.AOS)
            //{
            //    if (CheckSequence())
            //    {
            //        Toggle(Caster, Caster);
            //    }

            //    FinishSequence();
            //}
            //else
            //{
            //    if (Registry.ContainsKey(Caster))
            //    {
            //        Caster.SendLocalizedMessage(1005559); // This spell is already in effect.
            //    }
            //    else if (!Caster.CanBeginAction<DefensiveSpell>())
            //    {
            //        Caster.SendLocalizedMessage(1005385); // The spell will not adhere to you at this time.
            //    }
            //    else if (CheckSequence())
            //    {
            //        if (Caster.BeginAction<DefensiveSpell>())
            //        {
            //            double value = (Caster.Skills.EvalInt.Value +
            //                            Caster.Skills.Meditation.Value +
            //                            Caster.Skills.Inscribe.Value) * 10 / 4;

            //            Registry.Add(Caster, Math.Clamp((int)value, 0, 750)); // 75.0% protection from disruption
            //            new InternalTimer(Caster).Start();

            //            Caster.FixedParticles(0x375A, 9, 20, 5016, EffectLayer.Waist);
            //            Caster.PlaySound(0x1ED);
            //        }
            //        else
            //        {
            //            Caster.SendLocalizedMessage(1005385); // The spell will not adhere to you at this time.
            //        }
            //    }

            //    FinishSequence();
            //}
        }

        public void Target(Mobile m)
        {
            if (CheckBSequence(m))
            {
                if (!HasEffect(m) && !DayOfGodsSpell.HasEffect(m))
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
            var duration = SpellHelper.GetDuration(caster, m);

            var value = (int)(caster.Skills[SkillName.Magery].Value / 12) + Utility.RandomMinMax(6, 10);
            var physResMod = new ResistanceMod(ResistanceType.Physical, "Protection", value, m);

            m.AddResistanceMod(physResMod);

            Timer.StartTimer(duration, () => ClearEffect(m), out var token);
            _table[m] = token;

            m.PlaySound(0x1ED);
            m.FixedParticles(0x375A, 9, 20, 5016, EffectLayer.Waist);

            BuffInfo.AddBuff(m, new BuffInfo(BuffIcon.Protection, 3002025, 1107367, duration, m)); //3002025 - Protection  //1107367 - blank
        }

        public static void ClearEffect(Mobile m)
        {
            if (_table.Remove(m, out var token))
            {
                token.Cancel();

                m.RemoveResistanceMod("Protection");
                BuffInfo.RemoveBuff(m, BuffIcon.Protection);
            }
        }

        public static bool HasEffect(Mobile m) => _table.ContainsKey(m);
    }
}
