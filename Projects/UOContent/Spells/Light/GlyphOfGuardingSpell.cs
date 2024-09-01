using Server.Targeting;

namespace Server.Spells.Light
{
    public class GlyphOfGuardingSpell : LightSpell, ITargetingSpell<IPoint3D>
    {
        private static readonly SpellInfo _info = new(
            "Glyph Of Guarding",
            "Kal Ort Ylem",
            203,
            9051,
            Reagent.SulfurousAsh,
            Reagent.SpidersSilk,
            Reagent.Nightshade
        );

        public GlyphOfGuardingSpell(Mobile caster, Item scroll) : base(caster, scroll, _info)
        {
        }

        public override SpellCircle Circle => SpellCircle.Seventh;

        public override void OnCast()
        {
            Caster.Target = new SpellTarget<IPoint3D>(this, allowGround:true, TargetFlags.None);
        }

        public void Target(IPoint3D point)
        {
            Caster.LocalOverheadMessage(MessageType.Regular, 0x22, true, "Not Implemented yet...");
        }
    }
}
