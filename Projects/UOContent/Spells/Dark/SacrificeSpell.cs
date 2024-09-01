using Server.Targeting;

namespace Server.Spells.Dark
{
    public class SacrificeSpell : DarkSpell, ITargetingSpell<Mobile>
    {
        private static readonly SpellInfo _info = new(
            "Sacrifice",
            "Vas Rel Mani",
            203,
            9051,
            Reagent.MandrakeRoot,
            Reagent.SulfurousAsh,
            Reagent.Bone,
            Reagent.FertileDirt
        );

        public SacrificeSpell(Mobile caster, Item scroll) : base(caster, scroll, _info)
        {
        }

        public override SpellCircle Circle => SpellCircle.Seventh;

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
