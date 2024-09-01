using Server.Targeting;

namespace Server.Spells.Druid
{
    public class MiscastMagicSpell : DruidSpell, ITargetingSpell<Mobile>
    {
        private static readonly SpellInfo _info = new(
            "Miscast Magic",
            "Rel Ort Wis",
            203,
            9051,
            Reagent.BlackPearl,
            Reagent.Bloodmoss
        );

        public MiscastMagicSpell(Mobile caster, Item scroll) : base(caster, scroll, _info)
        {
        }

        public override SpellCircle Circle => SpellCircle.Sixth;

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
