using Server.Targeting;

namespace Server.Spells.Druid
{
    public class BurnedHandsSpell : DruidSpell, ITargetingSpell<Mobile>
    {
        private static readonly SpellInfo _info = new(
            "Burned Hands",
            "Uus Flam Grav",
            203,
            9051,
            Reagent.SpidersSilk,
            Reagent.SulfurousAsh
        );

        public BurnedHandsSpell(Mobile caster, Item scroll) : base(caster, scroll, _info)
        {
        }

        public override SpellCircle Circle => SpellCircle.Fourth;

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
