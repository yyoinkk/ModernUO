using Server.Targeting;

namespace Server.Spells.Dark
{
    public class KillSpell : DarkSpell, ITargetingSpell<Mobile>
    {
        private static readonly SpellInfo _info = new(
            "Kill",
            "Corp",
            203,
            9051,
            Reagent.SpidersSilk,
            Reagent.SulfurousAsh,
            Reagent.Bloodmoss,
            Reagent.VialOfBlood
        );

        public KillSpell(Mobile caster, Item scroll) : base(caster, scroll, _info)
        {
        }

        public override SpellCircle Circle => SpellCircle.Eighth;

        public override void OnCast()
        {
            Caster.Target = new SpellTarget<Mobile>(this, TargetFlags.Harmful);
        }

        public void Target(Mobile m)
        {
            if (CheckHSequence(m))
            {
                var source = Caster;

                SpellHelper.Turn(Caster, m);

                SpellHelper.CheckReflect((int)Circle, ref source, ref m);
                m.FixedParticles(0x3773, 10, 11, 0, 2074, 1, EffectLayer.Waist);

                if (m.Hits <= AOS.Scale(m.HitsMax, 40) && Utility.RandomDouble() > 0.45)
                {
                    m.Kill();
                    Caster.PlaySound(0xFB); //0xFB
                }
                else
                {
                    double damage = GetNewAosDamage(10, 2, 5, m);

                    SpellHelper.Damage(this, m, damage, 0, 0, 0, 0, 100);

                    m.PlaySound(0x2B9);
                }
            }
        }
    }
}
