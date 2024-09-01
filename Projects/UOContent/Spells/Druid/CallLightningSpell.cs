using Server.Targeting;

namespace Server.Spells.Druid
{
    public class CallLightningSpell : DruidSpell, ITargetingSpell<Mobile>
    {
        private static readonly SpellInfo _info = new(
            "Call Lightning",
            "Ort Grav",
            203,
            9051,
            Reagent.BlackPearl
        );

        public CallLightningSpell(Mobile caster, Item scroll) : base(caster, scroll, _info)
        {
        }

        public override SpellCircle Circle => SpellCircle.Third;

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
