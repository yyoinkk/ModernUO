
namespace Server.Spells.Dark
{
    public class UnholySpiritismSpell : DarkSpell
    {
        private static readonly SpellInfo _info = new(
            "Unholy Spiritism",
            "Uus Wis",
            203,
            9051,
            Reagent.BlackPearl,
            Reagent.Bone,
            Reagent.Nightshade
        );

        public UnholySpiritismSpell(Mobile caster, Item scroll) : base(caster, scroll, _info)
        {
        }

        public override SpellCircle Circle => SpellCircle.Fifth;

        public override void OnCast()
        {
            Caster.LocalOverheadMessage(MessageType.Regular, 0x22, true, "Not Implemented yet...");
        }
    }
}
