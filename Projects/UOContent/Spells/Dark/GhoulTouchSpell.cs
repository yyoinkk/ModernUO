
using ModernUO.CodeGeneratedEvents;
using Server.Mobiles;
using System;
using System.Collections.Generic;

namespace Server.Spells.Dark
{
    public class GhoulTouchSpell : DarkSpell
    {
        private static readonly SpellInfo _info = new(
            "Ghoul touch",
            "Corm Xen",
            203,
            9051,
            Reagent.Nightshade,
            Reagent.Bone
        );

        public GhoulTouchSpell(Mobile caster, Item scroll) : base(caster, scroll, _info)
        {
        }

        private static readonly HashSet<Mobile> _table = new();

        public override SpellCircle Circle => SpellCircle.Fourth;

        public override void OnCast()
        {
            if (CheckSequence())
            {
                if (!HasEffect(Caster))
                {
                    _table.Add(Caster);

                    Caster.FixedParticles(0x374A, 15, 16, 0, 2068, 0, EffectLayer.Head);
                    Caster.PlaySound(0x1C8);
                }
                else
                {
                    Caster.SendLocalizedMessage(502173); // You are already under a similar effect.
                }
            }
        }

        public static void Hit(Mobile caster, Mobile target)
        {
            int damage = Utility.RandomMinMax(10, 14) + (int)(caster.Skills[SkillName.SpiritSpeak].Value / 10);

            int dealt = AOS.Damage(target, caster, damage, false, 0, 0, 50, 0, 50);

            if (target != null)
            {
                caster.Hits += Math.Min(dealt, target.Hits);
                target.FixedParticles(0x374A, 20, 16, 0, 2068, 0, EffectLayer.Head);
            }

            caster.PlaySound(0x180);

            ClearEffect(caster);
        }

        [OnEvent(nameof(PlayerMobile.PlayerDeathEvent))]
        public static void OnPlayerDeathEvent(Mobile m)
        {
            ClearEffect(m);
        }

        public static void ClearEffect(Mobile m) => _table.Remove(m);

        public static bool HasEffect(Mobile m) => _table.Contains(m);
    }
}
