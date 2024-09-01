using Server.Targeting;

namespace Server.Spells.Dark
{
    public class KillSpell : DarkSpell, ITargetingSpell<Mobile>
    {
        private static readonly SpellInfo _info = new(
            "Kill",
            "Corp",
            203,
            9051,
            Reagent.SpidersSilk,
            Reagent.SulfurousAsh,
            Reagent.Bloodmoss,
            Reagent.FertileDirt
        );

        public KillSpell(Mobile caster, Item scroll) : base(caster, scroll, _info)
        {
        }

        public override SpellCircle Circle => SpellCircle.Eighth;

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
