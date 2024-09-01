using Server.Targeting;

namespace Server.Spells.Druid
{
    public class PoisonProtectionSpell : DruidSpell, ITargetingSpell<Mobile>
    {
        private static readonly SpellInfo _info = new(
            "Poison Protection",
            "Des Nox",
            203,
            9051,
            Reagent.Garlic
        );

        public PoisonProtectionSpell(Mobile caster, Item scroll) : base(caster, scroll, _info)
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
