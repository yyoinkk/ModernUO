
using Server.Items;
using System;
using System.Collections.Generic;

namespace Server.Spells.Dark
{
    public class UnholySpiritismSpell : DarkSpell
    {
        private static readonly SpellInfo _info = new(
            "Unholy Spiritism",
            "Uus Wis",
            203,
            9051,
            Reagent.BlackPearl,
            Reagent.Bone,
            Reagent.Nightshade
        );

        public UnholySpiritismSpell(Mobile caster, Item scroll) : base(caster, scroll, _info)
        {
        }

        private static readonly HashSet<Mobile> _table = new();

        public override SpellCircle Circle => SpellCircle.Fifth;

        public override void OnCast()
        {
            if (CheckSequence())
            {
                int damage; 

                if (_table.Contains(Caster))
                {
                    damage = 0;
                }
                else
                {
                    damage = Math.Clamp(2 + (int)(Caster.Skills[CastSkill].Value / 10), 0, 16);
                    damage = damage > Caster.Hits ? Caster.Hits - 1 : damage;
                }

                if (damage > 0)
                {
                    Caster.Damage(damage);
                    Caster.SendMessage($"Damage received: {damage}");

                    int mana = 14 + AOS.Scale(damage, 150);
                    _table.Add(Caster);
                    Timer.StartTimer(TimeSpan.FromSeconds(2.0), () => Delay_Callback(Caster, mana));
                }
            }
        }

        private void Delay_Callback(Mobile caster, int mana)
        {
            if (caster.Alive)
            {
                caster.Paralyzed = false;
                caster.Frozen = false;
                caster.Mana += mana;

                caster.SendMessage($"Mana restored: {mana}");

                caster.FixedParticles(0x3754, 10, 10, 0, EffectLayer.Head);
                caster.PlaySound(0x1A2);
            }

            _table.Remove(caster);
        }

        //public override bool CheckSequence()
        //{
        //    if (Caster.Deleted || !Caster.Alive || Caster.Spell != this || State != SpellState.Sequencing)
        //    {
        //        DoFizzle();
        //    }
        //    else if (Scroll != null && (Scroll.Amount <= 0 || Scroll.Deleted || Scroll.RootParent != Caster))
        //    {
        //        DoFizzle();
        //    }
        //    else if (CheckFizzle())
        //    {
        //        if (Scroll is SpellScroll)
        //        {
        //            Scroll.Consume();
        //        }
        //        else if (ClearHandsOnCast)
        //        {
        //            Caster.ClearHands();
        //        }
        //        return true;
        //    }
        //    else
        //    {
        //        DoFizzle();
        //    }

        //    return false;
        //}
    }
}
