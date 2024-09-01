using Server.Targeting;

namespace Server.Spells.Light
{
    public class RayOfLightSpell : LightSpell, ITargetingSpell<Mobile>
    {
        private static readonly SpellInfo _info = new(
            "Ray Of Light",
            "Lor Jux",
            203,
            9051,
            Reagent.SulfurousAsh,
            Reagent.BlackPearl
        );

        public RayOfLightSpell(Mobile caster, Item scroll) : base(caster, scroll, _info)
        {
        }

        public override SpellCircle Circle => SpellCircle.Third;

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
