using ModernUO.CodeGeneratedEvents;
using Server.Mobiles;
using Server.Targeting;
using System;
using System.Collections.Generic;

namespace Server.Spells.Dark
{
    public class DarknessSpell : DarkSpell, ITargetingSpell<Mobile>
    {
        private static readonly SpellInfo _info = new(
            "Darkness",
            "An Lor",
            203,
            9051,
            Reagent.SpidersSilk
        );

        public DarknessSpell(Mobile caster, Item scroll) : base(caster, scroll, _info)
        {
        }

        public override SpellCircle Circle => SpellCircle.First;

        public override void OnCast()
        {
            Caster.Target = new SpellTarget<Mobile>(this, TargetFlags.Harmful);
        }

        private static readonly Dictionary<Mobile, TimerExecutionToken> _table = new();

        public void Target(Mobile m)
        {
            if (CheckHSequence(m))
            {
                var source = Caster;

                SpellHelper.Turn(source, m);

                SpellHelper.CheckReflect((int)Circle, ref source, ref m);
                if (!HasEffect(m))
                {
                    m.CheckLightLevels(true);

                    var length = SpellHelper.GetDuration(Caster, m, curse: true);

                    var parryLossMod = new DefaultSkillMod(SkillName.Parry, "Darkness", true, m.Skills[SkillName.Parry].Base * -0.28);
                    m.AddSkillMod(parryLossMod);

                    Timer.StartTimer(length, () => ClearEffect(m), out var token);
                    _table[m] = token;

                    BuffInfo.AddBuff(m, new BuffInfo(BuffIcon.SpellPlague, 1077773, 1060393, length, m)); // 1077773 - The Darkness 1060393 -  ""
                    m.FixedEffect(0x376A, 10, 16, 1197, 16);
                }
                else if (m == Caster)
                {
                    Caster.SendLocalizedMessage(502173); // You are already under a similar effect.
                }
                else
                {
                    Caster.SendLocalizedMessage(501775); // This spell is already in effect.
                }

                m.PlaySound(0x229);
                HarmfulSpell(m);
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

                m.RemoveSkillMod("Darkness");
                m.CheckLightLevels(false);
                BuffInfo.RemoveBuff(m, BuffIcon.SpellPlague);
            }
        }

        public static bool HasEffect(Mobile m) => _table.ContainsKey(m);
    }
}
