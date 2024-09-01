namespace Server.Spells.Dark;

public abstract class DarkSpell : MagerySpell
{
    protected DarkSpell(Mobile caster, Item scroll, SpellInfo info) : base(caster, scroll, info)
    {
    }
}
