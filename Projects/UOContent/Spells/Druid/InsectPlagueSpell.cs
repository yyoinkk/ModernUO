using Server.Targeting;

namespace Server.Spells.Druid
{
    public class InsectPlagueSpell : DruidSpell, ITargetingSpell<Mobile>
    {
        private static readonly SpellInfo _info = new(
            "Insect Plague",
            "Vas Kal Jux Xen",
            203,
            9051,
            Reagent.BlackPearl,
            Reagent.Bloodmoss
        );

        public InsectPlagueSpell(Mobile caster, Item scroll) : base(caster, scroll, _info)
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
