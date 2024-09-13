using Server.Targeting;

namespace Server.Spells.Dark
{
    public class GreatHarmSpell : DarkSpell, ITargetingSpell<Mobile>
    {
        private static readonly SpellInfo _info = new(
            "Great Harm",
            "An Vas Mani",
            203,
            9051,
            Reagent.SulfurousAsh,
            Reagent.SpidersSilk,
            Reagent.BatWing
        );

        public GreatHarmSpell(Mobile caster, Item scroll) : base(caster, scroll, _info)
        {
        }

        public override SpellCircle Circle => SpellCircle.Sixth;

        public int TargetRange = 15;

        public override bool DelayedDamage => false;

        public override void OnCast()
        {
            Caster.Target = new SpellTarget<Mobile>(this, TargetFlags.Harmful);
        }

        public void Target(Mobile m)
        {
            SpellHelper.Turn(Caster, m);

            SpellHelper.CheckReflect((int)Circle, Caster, ref m);

            double damage = GetNewAosDamage(40, 2, 5, m);

            if (!m.InRange(Caster, 5))
            {
                damage *= 0.3; // 0.3 damage at > 5 tile range
            }
            else if (!m.InRange(Caster, 3))
            {
                damage *= 0.60; // 0.6 damage at 3 tile range
            }

            m.FixedParticles(0x374A, 10, 15, 5013, EffectLayer.Waist);
            m.PlaySound(0x1F1);

            SpellHelper.Damage(this, m, damage, 0, 0, 100, 0, 0);
        }
    }
}
