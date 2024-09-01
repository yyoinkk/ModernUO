using ModernUO.Serialization;
using Server.Gumps;

namespace Server.Items;

[SerializationGenerator(0, false)]
public partial class DarkMagicSpellBook : Spellbook
{
    [Constructible]
    public DarkMagicSpellBook(ulong content = 0) : base(content, 0x2253)
    {
        Name = "DarkMagic Book";
        Hue = 0x026;
        Layer = Core.ML ? Layer.OneHanded : Layer.Invalid;
    }

    public override SpellbookType SpellbookType => SpellbookType.Dark;
    public override int BookOffset => 700;
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
