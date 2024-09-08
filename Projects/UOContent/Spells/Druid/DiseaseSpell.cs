using Server.Mobiles;
using Server.Targeting;
using System.Collections.Generic;
using ModernUO.CodeGeneratedEvents;

namespace Server.Spells.Druid
{
    public class DiseaseSpell : DruidSpell, ITargetingSpell<Mobile>
    {
        private static readonly SpellInfo _info = new(
            "Disease",
            "In Nox Ylem",
            203,
            9051,
            Reagent.Nightshade,
            Reagent.BlackPearl
        );

        public DiseaseSpell(Mobile caster, Item scroll) : base(caster, scroll, _info)
        {
        }

        public override SpellCircle Circle => SpellCircle.First;

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
                    var toLoss = (int)m.Skills[SkillName.Tactics].Base * -0.28;

                    var tacticLossMod = new DefaultSkillMod(SkillName.Tactics, "Disease", true, toLoss);
                    m.AddSkillMod(tacticLossMod);

                    Timer.StartTimer(length, () => ClearEffect(m), out var token);
                    _table[m] = token;

                    Caster.SendMessage($"To loss: {toLoss}, duration: {length:mm\\:ss}s.");

                    m.FixedEffect(0x374A, 5, 6, 1285, 0);
                }
                else if (m == Caster)
                {
                    Caster.SendLocalizedMessage(502173); // You are already under a similar effect.
                }
                else
                {
                    Caster.SendLocalizedMessage(501775); // This spell is already in effect.
                }

                m.PlaySound(0x205);
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
                m.RemoveSkillMod("Disease");
            }
        }

        public static bool HasEffect(Mobile m) => _table.ContainsKey(m);
    }
}
