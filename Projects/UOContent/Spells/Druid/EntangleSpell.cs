using Server.Targeting;

namespace Server.Spells.Druid
{
    public class EntangleSpell : DruidSpell, ITargetingSpell<Mobile>
    {
        private static readonly SpellInfo _info = new(
            "Entangle",
            "An Por",
            203,
            9051,
            Reagent.Ginseng,
            Reagent.BlackPearl,
            Reagent.MandrakeRoot
        );

        public EntangleSpell(Mobile caster, Item scroll) : base(caster, scroll, _info)
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
