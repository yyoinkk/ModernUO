
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

        public override void OnCast()
        {
            Caster.LocalOverheadMessage(MessageType.Regular, 0x22, true, "Not Implemented yet...");
        }
    }
}
