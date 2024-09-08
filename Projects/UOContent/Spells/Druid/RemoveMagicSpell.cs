using Server.Spells.Dark;
using Server.Spells.Light;
using Server.Targeting;
using System;

namespace Server.Spells.Druid
{
    public class RemoveMagicSpell : DruidSpell, ITargetingSpell<Mobile>
    {
        private static readonly SpellInfo _info = new(
            "Remove Magic",
            "An Vas Ort",
            203,
            9051,
            Reagent.SpidersSilk,
            Reagent.Bloodmoss
        );

        public RemoveMagicSpell(Mobile caster, Item scroll) : base(caster, scroll, _info)
        {
        }

        public override SpellCircle Circle => SpellCircle.Third;

        public override void OnCast()
        {
            Caster.Target = new SpellTarget<Mobile>(this, TargetFlags.Harmful);
        }

        public void Target(Mobile m)
        {
            if (CheckHSequence(m))
            {
                SpellHelper.Turn(Caster, m);

                SpellHelper.CheckReflect((int)Circle, Caster, ref m);

                RegenerationSpell.ClearEffect(m);
                PainReflectionSpell.ClearEffect(m);

                PoisonProtectionSpell.ClearEffect(m);
                StoneSkinSpell.ClearEffect(m);
                NatureBlessingSpell.ClearEffect(m);

                SpiritArmorSpell.ClearEffect(m);
                FreeActionSpell.ClearEffect(m);
                CelestialPowerSpell.ClearEffect(m);

                //also cleares protection & bless statmodes
                DayOfGodsSpell.ClearEffect(m);

                m.FixedParticles(0x374A, 10, 12, 5016, EffectLayer.CenterFeet);
                m.PlaySound(0x2A1);
                HarmfulSpell(m);
            }
        }
    }
}
