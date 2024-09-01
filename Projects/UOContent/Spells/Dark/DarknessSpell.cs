using Server.Targeting;

namespace Server.Spells.Dark
{
    public class DarknessSpell : DarkSpell, ITargetingSpell<Mobile>
    {
        private static readonly SpellInfo _info = new(
            "Darkness",
            "An Lor",
            203,
            9051,
            Reagent.SpidersSilk,
            Reagent.FertileDirt
        );

        public DarknessSpell(Mobile caster, Item scroll) : base(caster, scroll, _info)
        {
        }

        public override SpellCircle Circle => SpellCircle.First;

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
