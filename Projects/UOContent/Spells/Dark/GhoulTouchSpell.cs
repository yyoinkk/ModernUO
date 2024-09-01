
namespace Server.Spells.Dark
{
    public class GhoulTouchSpell : DarkSpell
    {
        private static readonly SpellInfo _info = new(
            "Ghoul touch",
            "Corm Xen",
            203,
            9051,
            Reagent.Nightshade,
            Reagent.Bone
        );

        public GhoulTouchSpell(Mobile caster, Item scroll) : base(caster, scroll, _info)
        {
        }

        public override SpellCircle Circle => SpellCircle.Fourth;

        public override void OnCast()
        {
            Caster.LocalOverheadMessage(MessageType.Regular, 0x22, true, "Not Implemented yet...");
        }
    }
}
