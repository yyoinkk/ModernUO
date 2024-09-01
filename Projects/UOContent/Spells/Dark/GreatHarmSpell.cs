using Server.Targeting;

namespace Server.Spells.Dark
{
    public class GreatHarmSpell : DarkSpell, ITargetingSpell<Mobile>
    {
        private static readonly SpellInfo _info = new(
            "Great Harm",
            "An Vas Mani",
            203,
            9051,
            Reagent.BlackPearl,
            Reagent.SulfurousAsh,
            Reagent.SpidersSilk,
            Reagent.BatWing
        );

        public GreatHarmSpell(Mobile caster, Item scroll) : base(caster, scroll, _info)
        {
        }

        public override SpellCircle Circle => SpellCircle.Sixth;

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
