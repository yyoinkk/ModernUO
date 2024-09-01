using Server.Targeting;

namespace Server.Spells.Dark
{
    public class RegenerationSpell : DarkSpell, ITargetingSpell<Mobile>
    {
        private static readonly SpellInfo _info = new(
            "Regeneration",
            "Tym Rel Mani",
            203,
            9051,
            Reagent.BlackPearl,
            Reagent.Bone
        );

        public RegenerationSpell(Mobile caster, Item scroll) : base(caster, scroll, _info)
        {
        }

        public override SpellCircle Circle => SpellCircle.Fifth;

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
