using Server.Targeting;

namespace Server.Spells.Light
{
    public class HolyWrathSpell : LightSpell, ITargetingSpell<Mobile>
    {
        private static readonly SpellInfo _info = new(
            "Holy Wrath",
            "Wis Was Xen",
            203,
            9051,
            Reagent.SulfurousAsh,
            Reagent.BlackPearl
        );

        public HolyWrathSpell(Mobile caster, Item scroll) : base(caster, scroll, _info)
        {
        }

        public override SpellCircle Circle => SpellCircle.Sixth;

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

                double damage = GetNewAosDamage(40, 2, 4, m);
 
                m.BoltEffect(2049);
                m.PlaySound(0x1E9);
                m.FixedEffect(0x374A, 20, 5, 2066, 0);

                SpellHelper.Damage(this, m, damage, 0, 0, 40, 0, 60);
            }
        }
    }
}
