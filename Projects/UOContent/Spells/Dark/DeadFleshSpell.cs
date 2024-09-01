using Server.Targeting;

namespace Server.Spells.Dark
{
    public class DeadFleshSpell : DarkSpell, ITargetingSpell<Item>
    {
        private static readonly SpellInfo _info = new(
            "Dead Flesh",
            "Wis Corp Ylem",
            203,
            9051,
            Reagent.MandrakeRoot
        );

        public DeadFleshSpell(Mobile caster, Item scroll) : base(caster, scroll, _info)
        {
        }

        public override SpellCircle Circle => SpellCircle.First;

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
