using Server.Spells.Druid;
using Server.Targeting;
using System;
using System.Collections.Generic;

namespace Server.Spells.Dark
{
    public class DrainLifeSpell : DarkSpell, ITargetingSpell<Mobile>
    {
        private static readonly SpellInfo _info = new(
            "Drain Life",
            "Rel Mani",
            203,
            9051,
            Reagent.Ginseng,
            Reagent.Bone
        );

        public DrainLifeSpell(Mobile caster, Item scroll) : base(caster, scroll, _info)
        {
        }

        public override SpellCircle Circle => SpellCircle.Second;

        private static readonly HashSet<Mobile> _table = new();

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

                if (!NatureBlessingSpell.HasEffect(m))
                {
                    m.Spell?.OnCasterHurt();
                }

                HarmfulSpell(m);
                m.Paralyzed = false;

                m.FixedParticles(0x374A, 10, 25, 5032, 0x781, 0, EffectLayer.Head);
                m.PlaySound(0x1E4);

                if (_table.Contains(m))
                {
                    return;
                }

                var toDrain = Math.Clamp(12 + (int)(GetDamageSkill(Caster) / 5 - GetResistSkill(m) / 10), 0, Math.Min(m.Stam, 40));

                Caster.SendMessage($"To Ddrain: {toDrain}");

                if (toDrain > 0)
                {
                    Caster.Hits += toDrain;
                    m.Stam -= toDrain / 4;

                    if (m != Caster)
                    {
                        Caster.Stam -= toDrain / 4;
                        Caster.FixedParticles(0x374A, 10, 25, 5032, 0x781, 0, EffectLayer.Head);
                        //m.FixedEffect(0x3779, 10, 25);
                    }

                    _table.Add(m);
                    Timer.StartTimer(TimeSpan.FromSeconds(5.0), () => _table.Remove(m));
                }
            }
        }
    }
}
