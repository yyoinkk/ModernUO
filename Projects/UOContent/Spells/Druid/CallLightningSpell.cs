using Server.Targeting;

namespace Server.Spells.Druid
{
    public class CallLightningSpell : DruidSpell, ITargetingSpell<Mobile>
    {
        private static readonly SpellInfo _info = new(
            "Call Lightning",
            "Ort Grav",
            203,
            9051,
            Reagent.BlackPearl
        );

        public CallLightningSpell(Mobile caster, Item scroll) : base(caster, scroll, _info)
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

                double damage = GetNewAosDamage(19, 1, 5, m);

                m.BoltEffect(0);

                SpellHelper.Damage(this, m, damage, 0, 0, 0, 0, 100);
            }
        }
    }
}
