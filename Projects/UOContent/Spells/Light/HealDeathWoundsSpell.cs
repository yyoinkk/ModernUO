using Server.Targeting;

namespace Server.Spells.Light
{
    public class HealDeathWoundsSpell : LightSpell, ITargetingSpell<Mobile>
    {
        private static readonly SpellInfo _info = new(
            "Heal Death Wounds",
            "Vas In Mani",
            203,
            9051,
            Reagent.Ginseng,
            Reagent.Garlic,
            Reagent.SulfurousAsh
        );

        public HealDeathWoundsSpell(Mobile caster, Item scroll) : base(caster, scroll, _info)
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
