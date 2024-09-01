using Server.Targeting;

namespace Server.Spells.Light
{
    public class DivineInterventionSpell : LightSpell, ITargetingSpell<Mobile>
    {
        private static readonly SpellInfo _info = new(
            "Divine Intervention",
            "Vas Xen Ort Grav",
            203,
            9051,
            Reagent.Ginseng,
            Reagent.Garlic,
            Reagent.MandrakeRoot,
            Reagent.SpidersSilk
        );

        public DivineInterventionSpell(Mobile caster, Item scroll) : base(caster, scroll, _info)
        {
        }

        public override SpellCircle Circle => SpellCircle.Eighth;

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
