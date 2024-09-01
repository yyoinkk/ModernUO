
namespace Server.Spells.Light
{
    public class LightElementalSpell : LightSpell
    {
        private static readonly SpellInfo _info = new(
            "Light Elemental",
            "Kal Lor Xen",
            203,
            9051,
            Reagent.BlackPearl,
            Reagent.SulfurousAsh,
            Reagent.MandrakeRoot
        );

        public LightElementalSpell(Mobile caster, Item scroll) : base(caster, scroll, _info)
        {
        }

        public override SpellCircle Circle => SpellCircle.Seventh;

        public override void OnCast()
        {
            Caster.LocalOverheadMessage(MessageType.Regular, 0x22, true, "Not Implemented yet...");
        }
    }
}
