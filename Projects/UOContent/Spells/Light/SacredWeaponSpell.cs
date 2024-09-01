using Server.Targeting;

namespace Server.Spells.Light
{
    public class SacredWeaponSpell : LightSpell, ITargetingSpell<Item>
    {
        private static readonly SpellInfo _info = new(
            "Sacred Weapon",
            "In Ort",
            203,
            9051,
            Reagent.Ginseng,
            Reagent.Garlic
        );

        public SacredWeaponSpell(Mobile caster, Item scroll) : base(caster, scroll, _info)
        {
        }

        public override SpellCircle Circle => SpellCircle.First;

        public override void OnCast()
        {
            Caster.Target = new SpellTarget<Item>(this, TargetFlags.None);
        }

        public void Target(Item weap)
        {
            Caster.LocalOverheadMessage(MessageType.Regular, 0x22, true, "Not Implemented yet...");
        }
    }
}
