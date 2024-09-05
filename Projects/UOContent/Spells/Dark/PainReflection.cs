
using System;
using System.Collections.Generic;

namespace Server.Spells.Dark
{
    public class PainReflectionSpell : DarkSpell
    {
        private static readonly SpellInfo _info = new(
            "Pain Reflection",
            "Rel Corp Jux",
            203,
            9051,
            Reagent.SpidersSilk,
            Reagent.Bone,
            Reagent.Ginseng
        );

        public PainReflectionSpell(Mobile caster, Item scroll) : base(caster, scroll, _info)
        {
        }

        public override SpellCircle Circle => SpellCircle.Fourth;

        private static readonly HashSet<Mobile> _table = new();

        public override void OnCast()
        {
            if (!HasEffect(Caster))
            {
                _table.Add(Caster);
                var duration = SpellHelper.GetDuration(Caster, Caster, true);
                Timer.StartTimer(duration, () => ClearEffect(Caster));

                Caster.FixedParticles(0x3728, 12, 30, 0, 0, 0, EffectLayer.Waist);
                Caster.PlaySound(0x10C);
                Caster.PlaySound(0x11D);
            }
            else
            {
                Caster.SendLocalizedMessage(502173); // You are already under a similar effect.
            }

        }
    
        public static void ClearEffect(Mobile m)
        {
            if (HasEffect(m))
            {
                _table.Remove(m);
            }
        }

        public static bool HasEffect(Mobile m) => _table.Contains(m);
    }
}
