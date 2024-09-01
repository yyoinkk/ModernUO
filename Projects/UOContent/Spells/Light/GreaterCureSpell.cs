using Server.Targeting;

namespace Server.Spells.Light
{
    public class GreaterCureSpell : LightSpell, ITargetingSpell<Mobile>
    {
        private static readonly SpellInfo _info = new(
            "Greater Cure",
            "An Vas Nox",
            203,
            9051,
            Reagent.SulfurousAsh,
            Reagent.Garlic
        );

        public GreaterCureSpell(Mobile caster, Item scroll) : base(caster, scroll, _info)
        {
        }

        public override SpellCircle Circle => SpellCircle.Sixth;

        public override void OnCast()
        {
            Caster.Target = new SpellTarget<Mobile>(this, TargetFlags.Beneficial);
        }

        public void Target(Mobile m)
        {
            Caster.LocalOverheadMessage(MessageType.Regular, 0x22, true, "Not Implemented yet...");
        }
    }
}
