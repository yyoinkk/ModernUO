using System;
using Server.Items;
using Server.Targeting;
using System.Collections.Generic;

namespace Server.Spells.Light
{
    public class SacredWeaponSpell : LightSpell, ITargetingSpell<Item>
    {
        private static readonly SpellInfo _info = new(
            "Sacred Weapon",
            "In Ort",
            203,
            9051,
            Reagent.Ginseng,
            Reagent.SulfurousAsh
        );

        public SacredWeaponSpell(Mobile caster, Item scroll) : base(caster, scroll, _info)
        {
        }

        public override SpellCircle Circle => SpellCircle.First;

        private static readonly HashSet<BaseWeapon> _table = new();

        public override void OnCast()
        {
            Caster.Target = new SpellTarget<Item>(this, TargetFlags.None);
        }

        public void Target(Item weap)
        {
            if (weap is not BaseWeapon weapon)
            {
                return;
            }

            if (CheckSequence()) 
            {
                if (weapon.Cursed)
                {
                    Caster.SendMessage("Weapon is cursed!");
                    return;
                }

                if (HasEffect(weapon))
                {
                    Caster.SendLocalizedMessage(501775); // This spell is already in effect.
                    return;
                }

                _table.Add(weapon);

                if (weapon.Parent == Caster)
                {
                    Caster.FixedParticles(0x377A, 8, 10, 5042, EffectLayer.Head);
                }
                else
                {
                    Effects.SendLocationParticles(
                        EffectItem.Create(weapon.Location, weapon.Map, TimeSpan.FromSeconds(1.3)),
                        0x377A,
                        8,
                        10,
                        5042
                    );
                }

                Effects.PlaySound(weapon.GetWorldLocation(), weapon.Map, 0x1F5);
            }
        }
        public static void ClearEffect(BaseWeapon w) => _table.Remove(w);

        public static bool HasEffect(BaseWeapon w) => _table.Contains(w);

        public static int Apply(Mobile attacker, Mobile defender, BaseWeapon w)
        {
            defender.FixedEffect(0xA652, 5, 12, 2733, 0);
            attacker.SendMessage("Sacred weapon charge used.");
            ClearEffect(w);

            return attacker.Karma > defender.Karma ? 20 : -20 ;
        }
    }
}
