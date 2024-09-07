using System;
using Server.Targeting;
using Server.Spells.Dark;
using System.Collections.Generic;

namespace Server.Spells.Light
{
    public class DivineInterventionSpell : LightSpell, ITargetingSpell<Mobile>
    {
        private static readonly SpellInfo _info = new(
            "Divine Intervention",
            "Vas Xen Ort Grav",
            203,
            9051,
            Reagent.Ginseng,
            Reagent.Garlic,
            Reagent.MandrakeRoot,
            Reagent.SpidersSilk
        );

        public DivineInterventionSpell(Mobile caster, Item scroll) : base(caster, scroll, _info)
        {
        }

        public override SpellCircle Circle => SpellCircle.Eighth;

        private static readonly TimeSpan CD = TimeSpan.FromSeconds(180);

        private static readonly HashSet<Mobile> _table = new();

        public override bool CheckCast()
        {
            if (_table.Contains(Caster))
            {
                Caster.SendMessage("You can't use Divine yet.");
                return false;
            }
            return true;
        }

        public override void OnCasterHurt() { }

        public override void OnCast()
        {
            Caster.Target = new SpellTarget<Mobile>(this, TargetFlags.Beneficial);
        }

        public void Target(Mobile m)
        {
            if (CheckBSequence(m))
            {
                SpellHelper.Turn(Caster, m);

                m.Hits = m.HitsMax;
                m.Mana = m.ManaMax;
                m.Stam = m.StamMax;

                VulnerabilitySpell.ClearEffect(m);
                LowerResistanceSpell.ClearEffect(m);
                DarknessSpell.ClearEffect(m);

                m.PlaySound(0x214);
                m.PlaySound(0x1FD);

                m.FixedEffect(0x375A, 20, 15, 2066, 0);
                m.FixedEffect(0x373A, 10, 15, 2066, 0);

                _table.Add(Caster);
                Timer.StartTimer(CD, () => EndCD(Caster));
            }
        }

        public static void EndCD(Mobile m)
        {
            if (_table.Remove(m))
            {
                m.SendMessage("You can use Divine again.");
            }
        }
    }
}
