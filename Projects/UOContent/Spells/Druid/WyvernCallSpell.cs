
namespace Server.Spells.Druid
{
    public class WyvernCallSpell : DruidSpell
    {
        private static readonly SpellInfo _info = new(
            "Wyvern Call",
            "Kal Bet Nox Xen",
            203,
            9051,
            Reagent.MandrakeRoot,
            Reagent.SpidersSilk,
            Reagent.Bloodmoss
        );

        public WyvernCallSpell(Mobile caster, Item scroll) : base(caster, scroll, _info)
        {
        }

        public override SpellCircle Circle => SpellCircle.Eighth;

        public override void OnCast()
        {
            Caster.LocalOverheadMessage(MessageType.Regular, 0x22, true, "Not Implemented yet...");
        }
    }
}
