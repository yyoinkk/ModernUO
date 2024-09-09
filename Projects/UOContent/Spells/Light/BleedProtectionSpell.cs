using Server.Targeting;

namespace Server.Spells.Light
{
    public class BleedProtectionSpell : LightSpell, ITargetingSpell<Mobile>
    {
        private static readonly SpellInfo _info = new(
            "Bleed Protection",
            "Des Corp Ylem",
            203,
            9051,
            Reagent.Ginseng,
            Reagent.Garlic
        );

        public BleedProtectionSpell(Mobile caster, Item scroll) : base(caster, scroll, _info)
        {
        }

        public override SpellCircle Circle => SpellCircle.First;

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
