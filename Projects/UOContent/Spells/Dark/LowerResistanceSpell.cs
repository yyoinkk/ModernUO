using Server.Targeting;

namespace Server.Spells.Dark
{
    public class LowerResistanceSpell : DarkSpell, ITargetingSpell<Mobile>
    {
        private static readonly SpellInfo _info = new(
            "Lower Resistance",
            "Des Ort Sanct",
            203,
            9051,
            Reagent.SpidersSilk,
            Reagent.BlackPearl
        );

        public LowerResistanceSpell(Mobile caster, Item scroll) : base(caster, scroll, _info)
        {
        }

        public override SpellCircle Circle => SpellCircle.Second;

        public override void OnCast()
        {
            Caster.Target = new SpellTarget<Mobile>(this, TargetFlags.Harmful);
        }

        public void Target(Mobile m)
        {
            Caster.LocalOverheadMessage(MessageType.Regular, 0x22, true, "Not Implemented yet...");
        }
    }
}
