using Server.Targeting;

namespace Server.Spells.Light
{
    public class BoltOfGlorySpell : LightSpell, ITargetingSpell<Mobile>
    {
        private static readonly SpellInfo _info = new(
            "Bolt Of Glory",
            "Vas Por Grav",
            203,
            9051,
            Reagent.SulfurousAsh,
            Reagent.MandrakeRoot
        );

        public BoltOfGlorySpell(Mobile caster, Item scroll) : base(caster, scroll, _info)
        {
        }

        public override SpellCircle Circle => SpellCircle.Eighth;

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
