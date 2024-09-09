using ModernUO.CodeGeneratedEvents;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;
using System;
using System.Collections.Generic;

namespace Server.Spells.Druid
{
    public class BurnedHandsSpell : DruidSpell, ITargetingSpell<Mobile>
    {
        private static readonly SpellInfo _info = new(
            "Burned Hands",
            "Uus Flam Grav",
            203,
            9051,
            Reagent.SpidersSilk,
            Reagent.SulfurousAsh
        );

        public BurnedHandsSpell(Mobile caster, Item scroll) : base(caster, scroll, _info)
        {
        }

        public override SpellCircle Circle => SpellCircle.Fourth;

        private static readonly Dictionary<Mobile, TimerExecutionToken> _table = new();

        private static readonly TimeSpan CD = TimeSpan.FromSeconds(10);

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

                if (!HasEffect(m)) 
                {
                    var duration = TimeSpan.FromSeconds(4);

                    m.ClearHand(m.FindItemOnLayer(Layer.OneHanded), clearAnyway: true);
                    m.ClearHand(m.FindItemOnLayer(Layer.TwoHanded), clearAnyway: true);

                    m.PlaySound(0x3B9);
                    m.FixedParticles(0x36F4, 23, 25, 9948, EffectLayer.LeftHand);

                    BuffInfo.AddBuff(m, new BuffInfo(BuffIcon.NoRearm, 1075637, duration, m));

                    BaseWeapon.BlockEquip(m, duration);
                    BaseShield.BlockEquip(m, duration);

                    // TODO: separate CD and duration, make startCD()
                    Timer.StartTimer(duration + CD, () => ClearEffect(m), out var token);

                    _table[m] = token;
                }
            }
        }

        [OnEvent(nameof(PlayerMobile.PlayerDeathEvent))]
        public static void OnPlayerDeathEvent(Mobile m) => ClearEffect(m);

        public static void ClearEffect(Mobile m)
        {
            if (_table.Remove(m, out var token))
            {
                token.Cancel();
            }
        }

        public static bool HasEffect(Mobile m) => _table.ContainsKey(m);
    }
}
