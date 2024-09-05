using System;
using Server.Targeting;
using System.Collections.Generic;

namespace Server.Spells.Dark
{
    public class LowerResistanceSpell : DarkSpell, ITargetingSpell<Mobile>
    {
        private static readonly SpellInfo _info = new(
            "Lower Resistance",
            "Des Ort Sanct",
            203,
            9051,
            Reagent.SpidersSilk,
            Reagent.BlackPearl
        );

        public LowerResistanceSpell(Mobile caster, Item scroll) : base(caster, scroll, _info)
        {
        }

        public override SpellCircle Circle => SpellCircle.Second;

        private static readonly HashSet<Mobile> _table = new();

        public override void OnCast()
        {
            Caster.Target = new SpellTarget<Mobile>(this, TargetFlags.Harmful);
        }

        public void Target(Mobile m)
        {
            if (CheckHSequence(m))
            {
                if (!HasEffect(m))
                {
                    SpellHelper.Turn(Caster, m);

                    SpellHelper.CheckReflect((int)Circle, Caster, ref m);

                    //20 base + 1% for each percent magery after 75% of skill
                    var resistLoss = -(Math.Clamp((int)((Caster.Skills.Magery.Value - 75) / 3), 0, 25) + 20);

                    var duration = SpellHelper.GetDuration(Caster, m, true);
                    Caster.SendMessage($"To lost: {resistLoss}, duration {duration.ToString(@"mm\:ss")}s.");

                    var resistMod = new DefaultSkillMod(SkillName.MagicResist, "LowerResistance", true, resistLoss);

                    var fireResMod = new ResistanceMod(ResistanceType.Fire, "LowerResistance", resistLoss, m);
                    var coldResMod = new ResistanceMod(ResistanceType.Cold, "LowerResistance", resistLoss, m);
                    var enrgResMod = new ResistanceMod(ResistanceType.Energy, "LowerResistance", resistLoss, m);

                    //var poisResMod = new ResistanceMod(ResistanceType.Poison, "LowerResistance", resistLoss, m);

                    _table.Add(m);

                    m.AddSkillMod(resistMod);
                    m.AddResistanceMod(fireResMod);
                    m.AddResistanceMod(coldResMod);
                    m.AddResistanceMod(enrgResMod);
                    //m.AddResistanceMod(poisResMod);

                    Timer.StartTimer(duration, () => ClearEffect(m));

                    m.FixedParticles(0x3818, 9, 20, 5016, EffectLayer.CenterFeet);
                    m.PlaySound(0x28e);

                }
                else if (m == Caster)
                {
                    m.SendLocalizedMessage(502173); // You are already under a similar effect.
                }
                else
                {
                    m.SendLocalizedMessage(501775); // This spell is already in effect.
                }

                HarmfulSpell(m);
            }
        }

        public static void ClearEffect(Mobile m)
        {
            if (HasEffect(m))
            {
                _table.Remove(m);
                m.RemoveSkillMod("LowerResistance");
                m.RemoveResistanceMod("LowerResistance");
            }
        }

        public static bool HasEffect(Mobile m) => _table.Contains(m);

    }
}
