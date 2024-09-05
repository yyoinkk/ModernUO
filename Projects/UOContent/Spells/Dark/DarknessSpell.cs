using Server.Targeting;
using System;

namespace Server.Spells.Dark
{
    public class DarknessSpell : DarkSpell, ITargetingSpell<Mobile>
    {
        private static readonly SpellInfo _info = new(
            "Darkness",
            "An Lor",
            203,
            9051,
            Reagent.SpidersSilk,
            Reagent.FertileDirt
        );

        public DarknessSpell(Mobile caster, Item scroll) : base(caster, scroll, _info)
        {
        }

        public override SpellCircle Circle => SpellCircle.First;

        public override void OnCast()
        {
            Caster.Target = new SpellTarget<Mobile>(this, TargetFlags.Harmful);
        }

        public void Target(Mobile m)
        {
            if (CheckHSequence(m))
            {
                var source = Caster;

                SpellHelper.Turn(source, m);

                SpellHelper.CheckReflect((int)Circle, ref source, ref m);
                if (m.BeginAction<DarknessSpell>())
                {
                    m.CheckLightLevels(true);

                    var length = SpellHelper.GetDuration(Caster, m, curse: true);

                    m.AddSkillMod(new TimedSkillMod(SkillName.Parry, $"{Name}", true, m.Skills[SkillName.Parry].Base * -0.28, length));
                    new DarknessTimer(m, length).Start();

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

        private class DarknessTimer : Timer
        {
            private readonly Mobile m_Owner;

            public DarknessTimer(Mobile owner, TimeSpan duration) : base(duration)
            {
                m_Owner = owner;
            }

            protected override void OnTick()
            {
                m_Owner.EndAction<DarknessSpell>();
            }
        }
    }
}
