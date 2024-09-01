using Server.Targeting;

namespace Server.Spells.Light
{
    public class HolyWrathSpell : LightSpell, ITargetingSpell<Mobile>
    {
        private static readonly SpellInfo _info = new(
            "Holy Wrath",
            "Wis Was Xen",
            203,
            9051,
            Reagent.SulfurousAsh,
            Reagent.BlackPearl
        );

        public HolyWrathSpell(Mobile caster, Item scroll) : base(caster, scroll, _info)
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
