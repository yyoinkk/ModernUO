
using ModernUO.CodeGeneratedEvents;
using Server.Collections;
using Server.Mobiles;
using System;
using System.Collections.Generic;
using static Server.Timer;

namespace Server.Spells.Dark
{
    public class WraithAuraSpell : DarkSpell
    {
        private static readonly SpellInfo _info = new(
            "Wraith Aura",
            "In Jux Grav",
            203,
            9051,
            Reagent.BlackPearl,
            Reagent.FertileDirt,
            Reagent.Bone
        );

        public WraithAuraSpell(Mobile caster, Item scroll) : base(caster, scroll, _info)
        {
        }

        public override SpellCircle Circle => SpellCircle.Sixth;

        private static readonly Dictionary<Mobile, InternalTimer> _table = new();

        public override void OnCast()
        {
            if (CheckSequence())
            {
                if (!HasEffect(Caster))
                {
                    int interval = Math.Max(10 - (int)(Caster.Skills[CastSkill].Value / 25), 5);
                    int range = Caster.Skills[CastSkill].Value > 100 ? 5 : 4;
                    int duration = 70 + AOS.Scale((int)GetDamageSkill(Caster), 50);
                    int dmg = AOS.Scale((int)GetDamageSkill(Caster), 10) + Utility.RandomMinMax(3, 6);

                    var timer = new InternalTimer(this, Caster, dmg, range, TimeSpan.FromSeconds(duration), TimeSpan.FromSeconds(interval));
                    _table[Caster] = timer;
                    timer.Start();
                }
                else
                {
                    Caster.SendLocalizedMessage(502173); // You are already under a similar effect.
                }
            }
        }

        [OnEvent(nameof(PlayerMobile.PlayerDeathEvent))]
        public static void OnPlayerDeathEvent(Mobile m)
        {
            ClearEffect(m);
        }

        public static void ClearEffect(Mobile m)
        {
            if (_table.Remove(m, out var timer))
            {
                timer.Stop();
                m.SendMessage("Wraith Aura faded.");
            }
        }

        public static bool HasEffect(Mobile m) => _table.ContainsKey(m);

        private class InternalTimer : Timer
        {
            private readonly Spell _spell;
            private readonly Mobile _owner;
            private readonly int _dmg;
            private readonly int _range;
            private readonly DateTime _endTime;

            public InternalTimer(Spell spell, Mobile owner, int dmg, int range, TimeSpan duration, TimeSpan interval) : base(interval, interval)
            {
                _spell = spell;
                _owner = owner;
                _dmg = dmg;
                _range = range;
                _endTime = Core.Now + duration;
            }

            protected override void OnTick()
            {
                if (!_owner.Alive || Core.Now >= _endTime || _owner.Map == null)
                {
                    ClearEffect(_owner);
                    return;
                }

                using var queue = PooledRefQueue<Mobile>.Create();
                foreach (var m in _owner.GetMobilesInRange(_range))
                {
                    if (_owner != m && SpellHelper.ValidIndirectTarget(_owner, m, ignoreNotoriety: true) && _owner.CanBeHarmful(m, false) &&
                        (!Core.AOS || _owner.InLOS(m)))
                    {
                        queue.Enqueue(m);
                    }
                }

                while (queue.Count > 0)
                {
                    var m = queue.Dequeue();
                    _owner.DoHarmful(m);
                    SpellHelper.Damage(_spell, TimeSpan.Zero, m, _owner, _dmg, 0, 0, 0, 0, 100);
                    _owner.RevealingAction();

                    m.FixedParticles(0x374A, 20, 10, 0, EffectLayer.Waist);
                    m.PlaySound(0x17F);
                }
            }
        }
    }
}
