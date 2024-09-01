using Server.Targeting;

namespace Server.Spells.Druid
{
    public class RemoveMagicSpell : DruidSpell, ITargetingSpell<Mobile>
    {
        private static readonly SpellInfo _info = new(
            "Remove Magic",
            "An Vas Ort",
            203,
            9051,
            Reagent.SpidersSilk,
            Reagent.Bloodmoss
        );

        public RemoveMagicSpell(Mobile caster, Item scroll) : base(caster, scroll, _info)
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
