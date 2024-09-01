
namespace Server.Spells.Druid
{
    public class CallWoodlandSpell : DruidSpell
    {
        private static readonly SpellInfo _info = new(
            "Call Woodland",
            "Kal Ex Xen",
            203,
            9051,
            Reagent.BlackPearl,
            Reagent.Garlic
        );

        public CallWoodlandSpell(Mobile caster, Item scroll) : base(caster, scroll, _info)
        {
        }

        public override SpellCircle Circle => SpellCircle.Second;

        public override void OnCast()
        {
            Caster.LocalOverheadMessage(MessageType.Regular, 0x22, true, "Not Implemented yet...");
        }
    }
}
