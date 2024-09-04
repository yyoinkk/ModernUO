using Server.Targeting;
using System;
using System.Collections.Generic;

namespace Server.Spells.Dark
{
    public class RegenerationSpell : DarkSpell, ITargetingSpell<Mobile>
    {
        private static readonly SpellInfo _info = new(
            "Regeneration",
            "Tym Rel Mani",
            203,
            9051,
            Reagent.BlackPearl,
            Reagent.Bone
        );

        public RegenerationSpell(Mobile caster, Item scroll) : base(caster, scroll, _info)
        {
        }

        public override SpellCircle Circle => SpellCircle.Fifth;

        private static readonly Dictionary<Mobile, InternalTimer> _table = new();

        public override void OnCast()
        {
            Caster.Target = new SpellTarget<Mobile>(this, TargetFlags.Beneficial);
        }

        public void Target(Mobile m)
        {
            if (CheckBSequence(m))
            {
                SpellHelper.Turn(Caster, m);

                if (!_table.TryGetValue(m, out var timer))
                {
                    int hp = Math.Clamp(1 + (int)(Caster.Skills[CastSkill].Value / 40), 1, 4);
                    int duration = Math.Clamp(60 + (int)(GetDamageSkill(Caster) * 0.6), 60, 180);
                    int interval = 4;

                    timer = new InternalTimer(m, hp, TimeSpan.FromSeconds(duration), TimeSpan.FromSeconds(interval));
                    _table[m] = timer;
                    timer.Start();

                    m.FixedParticles(0x374A, 10, 20, 0, EffectLayer.Waist);
                    m.PlaySound(0x1E4);
                }
                else if (m == Caster)
                {
                    m.SendLocalizedMessage(502173); // You are already under a similar effect.
                }
                else
                {
                    m.SendLocalizedMessage(501775); // This spell is already in effect.
                }
            }
        }

        public static void ClearEffect(Mobile m)
        {
            _table.Remove(m, out var timer);
            timer.Stop();

            m.SendMessage("Regeneration is over.");
        }

        public static bool HasEffect(Mobile m) => _table.ContainsKey(m);

        private class InternalTimer : Timer
        {
            private readonly Mobile _owner;
            private readonly int _hp;
            private readonly DateTime _endTime;

            public InternalTimer(Mobile owner, int hp, TimeSpan duration, TimeSpan interval) : base(TimeSpan.FromSeconds(0.1), interval)
            {
                _owner = owner;
                _hp = hp;
                _endTime = Core.Now + duration;
            }

            protected override void OnTick()
            {
                if (!_owner.Alive || Core.Now >= _endTime)
                {
                    RegenerationSpell.ClearEffect(_owner);
                    return;
                }

                _owner.Hits += _hp;
                //_owner.SendMessage($"Healed: {_hp} hp.");
            }
        }
    }
}
