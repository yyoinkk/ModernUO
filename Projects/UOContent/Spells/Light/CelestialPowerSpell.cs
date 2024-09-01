
namespace Server.Spells.Light
{
    public class CelestialPowerSpell : LightSpell
    {
        private static readonly SpellInfo _info = new(
            "Celestial Power",
            "In Grav",
            203,
            9051,
            Reagent.SpidersSilk,
            Reagent.SulfurousAsh
        );

        public CelestialPowerSpell(Mobile caster, Item scroll) : base(caster, scroll, _info)
        {
        }

        public override SpellCircle Circle => SpellCircle.Second;

        public override void OnCast()
        {
            Caster.LocalOverheadMessage(MessageType.Regular, 0x22, true, "Not Implemented yet...");
        }
    }
}
