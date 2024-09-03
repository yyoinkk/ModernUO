using Server.Targeting;

namespace Server.Spells.Dark
{
    public class ImpaleSpell : DarkSpell, ITargetingSpell<Mobile>
    {
        private static readonly SpellInfo _info = new(
            "Impale",
            "In Corp Grav",
            203,
            9051,
            Reagent.BlackPearl,
            Reagent.Bone
        );

        public ImpaleSpell(Mobile caster, Item scroll) : base(caster, scroll, _info)
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
                var source = Caster;

                SpellHelper.Turn(source, m);

                SpellHelper.CheckReflect((int)Circle, ref source, ref m);

                double damage;

                if (Core.AOS)
                {
                    damage = GetNewAosDamage(18, 1, 8, m);
                }
                else
                {
                    damage = Utility.Random(10, 7);

                    if (CheckResisted(m))
                    {
                        damage *= 0.75;

                        m.SendLocalizedMessage(501783); // You feel yourself resisting magical energy.
                    }

                    damage *= GetDamageScalar(m);
                }

                m.FixedParticles(0x37C4, 10, 15, 5013, EffectLayer.Waist);
                source.PlaySound(Core.AOS ? 0x15E : 0x44B);

                SpellHelper.Damage(this, m, damage, 0, 20, 20, 0, 60);
            }
        }
    }
}
