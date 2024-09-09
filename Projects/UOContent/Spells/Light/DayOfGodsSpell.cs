using ModernUO.CodeGeneratedEvents;
using Server.Spells.Second;
using Server.Mobiles;
using System.Collections.Generic;
namespace Server.Spells.Light
{
    public class DayOfGodsSpell : LightSpell
    {
        private static readonly SpellInfo _info = new(
            "Day Of Gods",
            "Vas Xen Tym",
            203,
            9051,
            Reagent.MandrakeRoot,
            Reagent.SulfurousAsh,
            Reagent.SpidersSilk
        );

        public DayOfGodsSpell(Mobile caster, Item scroll) : base(caster, scroll, _info)
        {
        }

        public override SpellCircle Circle => SpellCircle.Fifth;

        private static readonly Dictionary<Mobile, TimerExecutionToken> _table = new();

        public override void OnCast()
        {
            if (CheckSequence())
            {
                ProtectionSpell.ClearEffect(Caster);

                var value = (int)(Caster.Skills[SkillName.Magery].Value / 12) + Utility.RandomMinMax(8, 12);
                var physResMod = new ResistanceMod(ResistanceType.Physical, "Protection", value, Caster);

                Caster.AddResistanceMod(physResMod);

                var length = SpellHelper.GetDuration(Caster, Caster);

                int additionalPercent = (int)(Caster.Skills[CastSkill].Value / 24);
                SpellHelper.AddStatBonus(Caster, Caster, StatType.Str, length, false, additionalOffset: additionalPercent);
                SpellHelper.AddStatBonus(Caster, Caster, StatType.Dex, length, additionalOffset: additionalPercent);
                SpellHelper.AddStatBonus(Caster, Caster, StatType.Int, length, additionalOffset: additionalPercent);

                Caster.FixedParticles(0x373A, 10, 15, 5018, EffectLayer.Waist);
                Caster.PlaySound(0x20B);

                Timer.StartTimer(length, () => ClearEffect(Caster), out var token);
                _table[Caster] = token;
            }
        }


        [OnEvent(nameof(PlayerMobile.PlayerDeathEvent))]
        public static void OnPlayerDeathEvent(Mobile m) => ClearEffect(m);

        public static void ClearEffect(Mobile m)
        {
            if (_table.Remove(m, out var token))
            {
                m.RemoveResistanceMod("Protection");

                m.RemoveStatMod($"[Magic] {StatType.Str} Buff");
                m.RemoveStatMod($"[Magic] {StatType.Dex} Buff");
                m.RemoveStatMod($"[Magic] {StatType.Int} Buff");
                token.Cancel();
            }
        }

        public static bool HasEffect(Mobile m) => _table.ContainsKey(m);
    }
}
