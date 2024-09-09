using Server.Targeting;
using System;

namespace Server.Spells.Druid
{
    public class EntangleSpell : DruidSpell, ITargetingSpell<Mobile>
    {
        private static readonly SpellInfo _info = new(
            "Entangle",
            "An Por",
            203,
            9051,
            Reagent.Ginseng,
            Reagent.BlackPearl,
            Reagent.MandrakeRoot
        );

        public EntangleSpell(Mobile caster, Item scroll) : base(caster, scroll, _info)
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
                var source = Caster;

                SpellHelper.Turn(source, m);

                SpellHelper.CheckReflect((int)Circle, ref source, ref m);

                if (!m.Entangled)
                {
                    var duration = Math.Clamp((int)(GetDamageSkill(Caster) - GetResistSkill(m)) * 0.1, 4, 8);

                    if (SpellHelper.Entangle(m, TimeSpan.FromSeconds(duration)))
                    {
                        m.FixedEffect(0x0D40, 1, 6);
                        m.PlaySound(0x204);

                        Caster.SendMessage($"Entangle duration: {(int)duration}s.");
                    }
                    else
                    {
                        m.PlaySound(0x201);
                        m.FixedEffect(0x376A, 6, 12);
                    }
                }
                else if (m == Caster)
                {
                    Caster.SendLocalizedMessage(502173); // You are already under a similar effect.
                }
                else
                {
                    Caster.SendLocalizedMessage(501775); // This spell is already in effect.
                }

                HarmfulSpell(m);
            }
        }
    }
}
