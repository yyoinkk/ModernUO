using ModernUO.CodeGeneratedEvents;
using Server.Mobiles;
using Server.Targeting;
using System;
using System.Collections.Generic;

namespace Server.Spells.Druid
{
    public class InsectPlagueSpell : DruidSpell, ITargetingSpell<Mobile>
    {
        private static readonly SpellInfo _info = new(
            "Insect Plague",
            "Vas Kal Jux Xen",
            203,
            9051,
            Reagent.BlackPearl,
            Reagent.Bloodmoss
        );

        public InsectPlagueSpell(Mobile caster, Item scroll) : base(caster, scroll, _info)
        {
        }

        public override SpellCircle Circle => SpellCircle.Sixth;

        private static readonly Dictionary<Mobile, InternalTimer> _table = new();

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

                if (!HasEffect(m))
                {
                    int interval = Math.Max(10 - (int)(GetDamageSkill(Caster) / 25), 5);

                    int duration = AOS.Scale((int)GetDamageSkill(Caster), 40);

                    int dmg = AOS.Scale((int)GetDamageSkill(Caster), 20) + Utility.RandomMinMax(1, 4);

                    var timer = new InternalTimer(this, m, Caster, dmg, TimeSpan.FromSeconds(duration), TimeSpan.FromSeconds(interval));

                    Caster.SendMessage($"Interval: {interval}, duration: {duration}, dmg: {dmg}");
                    _table[m] = timer;
                    timer.Start();
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
                m.SendMessage("Insect plague is over.");
                timer.Stop();
            }
        }

        public static bool HasEffect(Mobile m) => _table.ContainsKey(m);

        private class InternalTimer : Timer
        {
            private readonly Mobile _caster;
            private readonly Spell _spell;
            private readonly Mobile _owner;
            private readonly int _dmg;
            private readonly DateTime _endTime;

            public InternalTimer(Spell spell,Mobile owner, Mobile caster, int dmg, TimeSpan duration, TimeSpan interval) : base(TimeSpan.Zero, interval)
            {
                _caster = caster;
                _spell = spell;
                _owner = owner;
                _dmg = dmg;
                _endTime = Core.Now + duration;
            }

            protected override void OnTick()
            {
                if (!_owner.Alive || Core.Now >= _endTime || _owner.Map == null)
                {
                    ClearEffect(_owner);
                    return;
                }

                SpellHelper.Damage(_spell, TimeSpan.Zero, _owner, _caster, _dmg, 50, 0, 0, 50, 0);

                _owner.FixedParticles(0x091B, 20, 7, 0, EffectLayer.Waist);
                _owner.PlaySound(0x5CB); //0x5CB|0x5CC|0x22F
            }
        }
    }
}
