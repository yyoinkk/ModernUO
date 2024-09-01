
namespace Server.Spells.Dark
{
    public class WraithAuraSpell : DarkSpell
    {
        private static readonly SpellInfo _info = new(
            "Wraith Aura",
            "In Jux Grav",
            203,
            9051,
            Reagent.BlackPearl,
            Reagent.FertileDirt,
            Reagent.Bone
        );

        public WraithAuraSpell(Mobile caster, Item scroll) : base(caster, scroll, _info)
        {
        }

        public override SpellCircle Circle => SpellCircle.Sixth;

        public override void OnCast()
        {
            Caster.LocalOverheadMessage(MessageType.Regular, 0x22, true, "Not Implemented yet...");
        }
    }
}
