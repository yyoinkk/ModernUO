namespace Server.Spells.Druid;

public abstract class DruidSpell : MagerySpell
{
    protected DruidSpell(Mobile caster, Item scroll, SpellInfo info) : base(caster, scroll, info)
    {
    }
}
