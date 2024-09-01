
namespace Server.Spells.Druid
{
    public class NatureBlessingSpell : DruidSpell
    {
        private static readonly SpellInfo _info = new(
            "Nature Blessing",
            "In Ylem Sanct",
            203,
            9051,
            Reagent.Ginseng,
            Reagent.Bloodmoss,
            Reagent.BlackPearl
        );

        public NatureBlessingSpell(Mobile caster, Item scroll) : base(caster, scroll, _info)
        {
        }

        public override SpellCircle Circle => SpellCircle.Fifth;

        public override void OnCast()
        {
            Caster.LocalOverheadMessage(MessageType.Regular, 0x22, true, "Not Implemented yet...");
        }
    }
}
