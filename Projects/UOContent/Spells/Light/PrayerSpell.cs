
namespace Server.Spells.Light
{
    public class PrayerSpell : LightSpell
    {
        private static readonly SpellInfo _info = new(
            "Prayer",
            "Kal Vas Xen",
            203,
            9051,
            Reagent.Garlic,
            Reagent.SpidersSilk,
            Reagent.MandrakeRoot
        );

        public PrayerSpell(Mobile caster, Item scroll) : base(caster, scroll, _info)
        {
        }

        public override SpellCircle Circle => SpellCircle.Fourth;

        public override void OnCast()
        {
            Caster.LocalOverheadMessage(MessageType.Regular, 0x22, true, "Not Implemented yet...");
        }
    }
}
