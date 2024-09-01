
namespace Server.Spells.Light
{
    public class FreeActionSpell : LightSpell
    {
        private static readonly SpellInfo _info = new(
            "Free Action",
            "In Por Sanct",
            203,
            9051,
            Reagent.BlackPearl,
            Reagent.Ginseng
        );

        public FreeActionSpell(Mobile caster, Item scroll) : base(caster, scroll, _info)
        {
        }

        public override SpellCircle Circle => SpellCircle.Third;

        public override void OnCast()
        {
            Caster.LocalOverheadMessage(MessageType.Regular, 0x22, true, "Not Implemented yet...");
        }
    }
}
