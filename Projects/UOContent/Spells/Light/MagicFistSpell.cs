using Server.Targeting;

namespace Server.Spells.Light
{
    public class MagicFistSpell : LightSpell, ITargetingSpell<Mobile>
    {
        private static readonly SpellInfo _info = new(
            "Magic Fist",
            "Por Ort Ylem",
            203,
            9051,
            Reagent.SulfurousAsh,
            Reagent.MandrakeRoot
        );

        public MagicFistSpell(Mobile caster, Item scroll) : base(caster, scroll, _info)
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
