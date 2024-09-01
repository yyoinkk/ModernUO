
namespace Server.Spells.Light
{
    public class SpiritArmorSpell : LightSpell
    {
        private static readonly SpellInfo _info = new(
            "Spirit Armor",
            "Sanct",
            203,
            9051,
            Reagent.Garlic,
            Reagent.SpidersSilk
        );

        public SpiritArmorSpell(Mobile caster, Item scroll) : base(caster, scroll, _info)
        {
        }

        public override SpellCircle Circle => SpellCircle.Second;

        public override void OnCast()
        {
            Caster.LocalOverheadMessage(MessageType.Regular, 0x22, true, "Not Implemented yet...");
        }
    }
}
