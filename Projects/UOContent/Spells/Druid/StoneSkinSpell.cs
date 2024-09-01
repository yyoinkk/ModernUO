
namespace Server.Spells.Druid
{
    public class StoneSkinSpell : DruidSpell
    {
        private static readonly SpellInfo _info = new(
            "Stone Skin",
            "In Ylem Sanct",
            203,
            9051,
            Reagent.SpidersSilk,
            Reagent.Bloodmoss
        );

        public StoneSkinSpell(Mobile caster, Item scroll) : base(caster, scroll, _info)
        {
        }

        public override SpellCircle Circle => SpellCircle.Third;

        public override void OnCast()
        {
            Caster.LocalOverheadMessage(MessageType.Regular, 0x22, true, "Not Implemented yet...");
        }
    }
}
