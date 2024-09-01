using Server.Targeting;

namespace Server.Spells.Dark
{
    public class RaiseDeadSpell : DarkSpell, ITargetingSpell<Item>
    {
        private static readonly SpellInfo _info = new(
            "Raise Dead",
            "Vas An Corp",
            203,
            9051,
            Reagent.Bone,
            Reagent.Bloodmoss,
            Reagent.BatWing
        );

        public RaiseDeadSpell(Mobile caster, Item scroll) : base(caster, scroll, _info)
        {
        }

        public override SpellCircle Circle => SpellCircle.Seventh;

        public override void OnCast()
        {
            Caster.Target = new SpellTarget<Item>(this, TargetFlags.None);
        }

        public void Target(Item item)
        {
            Caster.LocalOverheadMessage(MessageType.Regular, 0x22, true, "Not Implemented yet...");
        }
    }
}
