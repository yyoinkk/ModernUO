using ModernUO.Serialization;
using Server.Gumps;

namespace Server.Items;

[SerializationGenerator(0)]
public partial class LightMagicSpellbook : Spellbook
{
    [Constructible]
    public LightMagicSpellbook(ulong content = 0) : base(content, 0x2252)
    {
        Name = "LightMagic Book";
        Hue = 0x065;
        Layer = Layer.OneHanded;
    }

    public override SpellbookType SpellbookType => SpellbookType.Light;
    public override int BookOffset => 900;
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
