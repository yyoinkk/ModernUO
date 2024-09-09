using Server.Items;
using Server.Targeting;

namespace Server.Spells.Light
{
    public class MagicFistSpell : LightSpell, ITargetingSpell<Mobile>
    {
        private static readonly SpellInfo _info = new(
            "Magic Fist",
            "Por Ort Ylem",
            203,
            9051,
            Reagent.SulfurousAsh,
            Reagent.MandrakeRoot
        );

        public MagicFistSpell(Mobile caster, Item scroll) : base(caster, scroll, _info)
        {
        }

        public override SpellCircle Circle => SpellCircle.Fourth;

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

                double damage = GetNewAosDamage(24, 1, 5, m);

                new Blood().MoveToWorld(m.Location, m.Map);

                m.PlaySound(Utility.RandomList(0x3B2, 0x3A2, 0x3A5, 0x3AE, 0x3B1, 0x3B0));
                m.FixedEffect(0xA652, 5, 12, 2733, 0);
                SpellHelper.Damage(this, m, damage, 100, 0, 0, 0, 0);
            }
        }
    }
}
