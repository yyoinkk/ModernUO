using Server.Targeting;

namespace Server.Spells.Druid
{
    public class AcidCloudSpell : DruidSpell, ITargetingSpell<IPoint3D>
    {
        private static readonly SpellInfo _info = new(
            "Acid Cloud",
            "Vas Ort Nox",
            203,
            9051,
            Reagent.Nightshade,
            Reagent.Nightshade
        );

        public AcidCloudSpell(Mobile caster, Item scroll) : base(caster, scroll, _info)
        {
        }

        public override SpellCircle Circle => SpellCircle.Eighth;

        public override void OnCast()
        {
            Caster.Target = new SpellTarget<IPoint3D>(this, allowGround: true);
        }

        public void Target(IPoint3D target)
        {
            Caster.LocalOverheadMessage(MessageType.Regular, 0x22, true, "Not Implemented yet...");
        }
    }
}
