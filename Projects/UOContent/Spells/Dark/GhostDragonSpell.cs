
namespace Server.Spells.Dark
{
    public class GhostDragonSpell : DarkSpell
    {
        private static readonly SpellInfo _info = new(
            "Ghost Dragon",
            "Kal Corp Ort Xen",
            203,
            9051,
            Reagent.Bone,
            Reagent.Nightshade,
            Reagent.BatWing,
            Reagent.FertileDirt
        );

        public GhostDragonSpell(Mobile caster, Item scroll) : base(caster, scroll, _info)
        {
        }

        public override SpellCircle Circle => SpellCircle.Eighth;

        public override void OnCast()
        {
            Caster.LocalOverheadMessage(MessageType.Regular, 0x22, true, "Not Implemented yet...");
        }
    }
}
