using Server.Targeting;

namespace Server.Spells.Druid
{
    public class IceStrikeSpell : DruidSpell, ITargetingSpell<Mobile>
    {
        private static readonly SpellInfo _info = new(
            "Ice Strike",
            "An Flam Grav",
            203,
            9051,
            Reagent.MandrakeRoot,
            Reagent.SulfurousAsh
        );

        public IceStrikeSpell(Mobile caster, Item scroll) : base(caster, scroll, _info)
        {
        }

        public override SpellCircle Circle => SpellCircle.Seventh;

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
