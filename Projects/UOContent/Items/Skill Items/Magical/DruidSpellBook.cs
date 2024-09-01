using ModernUO.Serialization;
using Server.Gumps;

namespace Server.Items;

[SerializationGenerator(0)]
public partial class DruidSpellBook : Spellbook
{
    [Constructible]
    public DruidSpellBook(ulong content = 0) : base(content, 0x2D50)
    {
        Name = "Druid Book";
        Hue = 0x051;
        Layer = Layer.OneHanded;
    }

    public override SpellbookType SpellbookType => SpellbookType.Druid;
    public override int BookOffset => 800;
    public override int BookCount => 16;

    public override void OnDoubleClick(Mobile from)
    {
        var pack = from.Backpack;

        if (Parent == from || pack != null && Parent == pack)
        {
            from.SendGump(new ColorBookGump(this), true);
        }
        else
        {
            from.SendLocalizedMessage(
                500207
            ); // The spellbook must be in your backpack (and not in a container within) to open.
        }
    }
}
