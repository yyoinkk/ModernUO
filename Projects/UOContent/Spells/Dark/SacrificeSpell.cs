using Server.Targeting;

namespace Server.Spells.Dark
{
    public class SacrificeSpell : DarkSpell, ITargetingSpell<Mobile>
    {
        private static readonly SpellInfo _info = new(
            "Sacrifice",
            "Vas Rel Mani",
            203,
            9051,
            Reagent.MandrakeRoot,
            Reagent.SulfurousAsh,
            Reagent.Bone,
            Reagent.FertileDirt
        );

        public SacrificeSpell(Mobile caster, Item scroll) : base(caster, scroll, _info)
        {
        }

        public override SpellCircle Circle => SpellCircle.Seventh;

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

                double damage = GetNewAosDamage(49, 1, 7, m);

                m.FixedParticles(0x3728, 30, 30, 22, 1258, 2, EffectLayer.Head);
                m.FixedParticles(0x36BD, 20, 20, 22, 1258, 2, EffectLayer.Head);  //Explosion

                m.PlaySound(0x207);
                m.PlaySound(0x213); 

                int selfDmg = SpellHelper.CalculateDamage(this, m, Caster, damage, 0, 100, 0, 0, 0) / 2;

                SpellHelper.Damage(this, m, damage, 0, 100, 0, 0, 0);
                Caster.Damage(selfDmg, Caster);
            }
        }
    }
}
