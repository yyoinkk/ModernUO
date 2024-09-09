namespace Server.Spells.Light;

public abstract class LightSpell : MagerySpell
{
    protected LightSpell(Mobile caster, Item scroll, SpellInfo info) : base(caster, scroll, info)
    {
    }
}
