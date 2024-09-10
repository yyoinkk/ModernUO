using System;
using Server.Spells.Druid;
using Server.Targeting;

namespace Server.Spells.Third
{
    public class PoisonSpell : MagerySpell, ITargetingSpell<Mobile>
    {
        private static readonly SpellInfo _info = new(
            "Poison",
            "In Nox",
            203,
            9051,
            Reagent.Nightshade
        );

        public PoisonSpell(Mobile caster, Item scroll = null) : base(caster, scroll, _info)
        {
        }

        public override SpellCircle Circle => SpellCircle.Third;

        public void Target(Mobile m)
        {
            if (CheckHSequence(m))
            {
                SpellHelper.Turn(Caster, m);

                SpellHelper.CheckReflect((int)Circle, Caster, ref m);

                if (!NatureBlessingSpell.HasEffect(m))
                {
                    m.Spell?.OnCasterHurt(); 
                }

                m.Paralyzed = false;

                if (CheckResisted(m))
                {
                    m.SendLocalizedMessage(501783); // You feel yourself resisting magical energy.
                }
                else
                {
                    int level = PoisonImpl.GetPoisonLevel(Caster, m);

                    m.ApplyPoison(Caster, Poison.GetPoison(Math.Max(level, 0)));
                }

                m.FixedParticles(0x374A, 10, 15, 5021, EffectLayer.Waist);
                m.PlaySound(0x205);

                HarmfulSpell(m);
            }
        }

        public override void OnCast()
        {
            Caster.Target = new SpellTarget<Mobile>(this, TargetFlags.Harmful);
        }
    }
}
