using Server.Targeting;
using System;

namespace Server.Spells.Druid
{
    public class IceStrikeSpell : DruidSpell, ITargetingSpell<Mobile>
    {
        private static readonly SpellInfo _info = new(
            "Ice Strike",
            "An Flam Grav",
            203,
            9051,
            Reagent.MandrakeRoot,
            Reagent.SulfurousAsh
        );

        public IceStrikeSpell(Mobile caster, Item scroll) : base(caster, scroll, _info)
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

                double damage = GetNewAosDamage(43, 1, 5, m);
                var duration = 0.2 + GetDamageSkill(Caster) / 40 * 0.25;

                m.Freeze(TimeSpan.FromSeconds(duration));

                m.FixedParticles(0x3789, 10, 30, 5052, EffectLayer.Head);
                m.PlaySound(0x212);

                SpellHelper.Damage(this, m, damage, 0, 0, 100, 0, 0);
            }
        }
    }
}
