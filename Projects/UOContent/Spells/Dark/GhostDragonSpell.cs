
using System;
using Server.Mobiles;
using System.Collections.Generic;
using ModernUO.CodeGeneratedEvents;

namespace Server.Spells.Dark
{
    public class GhostDragonSpell : DarkSpell
    {
        private static readonly SpellInfo _info = new(
            "Ghost Dragon",
            "Kal Corp Ort Xen",
            203,
            9051,
            Reagent.Bone,
            Reagent.Nightshade,
            Reagent.VialOfBlood,
            Reagent.FertileDirt
        );

        public GhostDragonSpell(Mobile caster, Item scroll) : base(caster, scroll, _info)
        {
        }

        public override SpellCircle Circle => SpellCircle.Eighth;

        public static readonly Dictionary<Mobile, SummonedGhostDragon> _table = new();

        public override void OnCast()
        {
            if (CheckSequence())
            {
                var duration = Core.Expansion switch
                {
                    Expansion.None => TimeSpan.FromSeconds(Caster.Skills.Magery.Value),
                    // T2A -> Current
                    _ => TimeSpan.FromSeconds(3 * Math.Max(5, Caster.Skills.Magery.Value)),
                };

                Caster.SendMessage($"Duration: {duration:mm\\:ss}s.");

                SummonedGhostDragon dragon = new SummonedGhostDragon();
                SpellHelper.Summon(dragon, Caster, 0x216, duration, false, false);

                if (!dragon.Deleted)
                {
                    dragon.FixedParticles(0x3728, 8, 20, 5042, EffectLayer.Head);
                    _table[Caster] = dragon;
                }
            }

            FinishSequence();
        }

        [OnEvent(nameof(PlayerMobile.PlayerDeathEvent))]
        public static void OnPlayerDeathEvent(Mobile m)
        {
            if (_table.TryGetValue(m, out var dragon))
            {
                dragon.Kill();
                _table.Remove(m);
            }
        }

        public override bool CheckCast()
        {
            if (!base.CheckCast())
            {
                return false;
            }

            if (Caster.Followers + 4 > Caster.FollowersMax)
            {
                Caster.SendLocalizedMessage(1049645); // You have too many followers to summon that creature.
                return false;
            }

            return true;
        }
    }
}
