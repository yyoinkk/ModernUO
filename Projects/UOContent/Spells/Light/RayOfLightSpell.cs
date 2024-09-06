using Server.Targeting;

namespace Server.Spells.Light
{
    public class RayOfLightSpell : LightSpell, ITargetingSpell<Mobile>
    {
        private static readonly SpellInfo _info = new(
            "Ray Of Light",
            "Lor Jux",
            203,
            9051,
            Reagent.SulfurousAsh,
            Reagent.BlackPearl
        );

        public RayOfLightSpell(Mobile caster, Item scroll) : base(caster, scroll, _info)
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

                double damage = GetNewAosDamage(17, 1, 9, m);

                m.FixedEffect(0x37B9, 14, 22, 2733, 0);
                m.FixedEffect(0x374A, 14, 14, 2733, 0);
                m.PlaySound(0x1E9);

                SpellHelper.Damage(this, m, damage, 0, 50, 0, 0, 50);
            }
        }
    }
}
