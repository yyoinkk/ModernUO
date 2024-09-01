
namespace Server.Spells.Dark
{
    public class PainReflectionSpell : DarkSpell
    {
        private static readonly SpellInfo _info = new(
            "Pain Reflection",
            "Rel Corp Jux",
            203,
            9051,
            Reagent.SpidersSilk,
            Reagent.Bone,
            Reagent.Ginseng
        );

        public PainReflectionSpell(Mobile caster, Item scroll) : base(caster, scroll, _info)
        {
        }

        public override SpellCircle Circle => SpellCircle.Fourth;

        public override void OnCast()
        {
            Caster.LocalOverheadMessage(MessageType.Regular, 0x22, true, "Not Implemented yet...");
        }
    }
}
