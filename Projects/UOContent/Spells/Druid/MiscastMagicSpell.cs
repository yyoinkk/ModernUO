using ModernUO.CodeGeneratedEvents;
using Server.Mobiles;
using Server.Targeting;
using System.Collections.Generic;

namespace Server.Spells.Druid
{
    public class MiscastMagicSpell : DruidSpell, ITargetingSpell<Mobile>
    {
        private static readonly SpellInfo _info = new(
            "Miscast Magic",
            "Rel Ort Wis",
            203,
            9051,
            Reagent.BlackPearl,
            Reagent.Bloodmoss
        );

        public MiscastMagicSpell(Mobile caster, Item scroll) : base(caster, scroll, _info)
        {
        }

        public override SpellCircle Circle => SpellCircle.Sixth;

        private static readonly Dictionary<Mobile, TimerExecutionToken> _table = new();

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
                    m.CheckLightLevels(true);

                    var length = SpellHelper.GetDuration(Caster, m, curse: true) * 0.25;
                    var toloss = (int)Caster.Skills[SkillName.Magery].Base * -0.4;

                    var mageryLoss = new DefaultSkillMod(SkillName.Magery, "MiscastMagic", true, toloss);
                    m.AddSkillMod(mageryLoss);

                    Timer.StartTimer(length, () => ClearEffect(m), out var token);
                    _table[m] = token;

                    Caster.SendMessage($"To loss: {toloss}, duration: {length:mm\\:ss}s.");

                    m.FixedEffect(0x3818, 5, 12, 1285, 0);
                    m.PlaySound(0x216);
                }
                else if (m == Caster)
                {
                    Caster.SendLocalizedMessage(502173); // You are already under a similar effect.
                }
                else
                {
                    Caster.SendLocalizedMessage(501775); // This spell is already in effect.
                }
                
                HarmfulSpell(m);
            }
        }

        [OnEvent(nameof(PlayerMobile.PlayerDeathEvent))]
        public static void OnPlayerDeathEvent(Mobile m)
        {
            ClearEffect(m);
        }

        public static void ClearEffect(Mobile m)
        {
            if (_table.Remove(m, out var token))
            {
                token.Cancel();
                m.RemoveSkillMod("MiscastMagic");
            }
        }

        public static bool HasEffect(Mobile m) => _table.ContainsKey(m);
    }
}
