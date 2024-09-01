using Server.Gumps;
using Server.Multis;
using Server.Network;
using Server.Spells;

namespace Server.Items;

public class ColorBookGump : DynamicGump
{
    private readonly Spellbook _book;
    private readonly int _gumpId;
    private readonly int _buttonGumpId;

    public override bool Singleton => true;

    public ColorBookGump(Spellbook book) : base(150, 200)
    {
        //TODO: clean up this
        if (book is DarkMagicSpellBook)
        {
            _gumpId = 11008;
            _buttonGumpId = 2360;
        }
        else if (book is DruidSpellBook)
        {
            _gumpId = 11055;
            _buttonGumpId = 2361;
        }
        else if (book is LightMagicSpellbook)
        {
            _gumpId = 11009;
            _buttonGumpId = 2362;
        }

        _book = book;
    }

    protected override void BuildLayout(ref DynamicGumpBuilder builder)
    {
        builder.AddPage();
        builder.AddImage(0, 0, _gumpId);

        var spells = GetSpells();

        int topOffset = 10;
        int leftSideRow = 0;
        for (int i = 0; i < 8; i++)
        {
            if (spells[i] != null)
            {
                leftSideRow++;
                builder.AddButton(65, topOffset + leftSideRow * 20, _buttonGumpId, _buttonGumpId, i + _book.BookOffset);
                builder.AddHtml(85, topOffset + leftSideRow * 20 - 3, 115, 17, $"{spells[i]}");
            }
        }

        int rightSideRow = 0;
        for (int i = 8; i < _book.BookCount; i++)
        {
            if (spells[i] != null)
            {
                rightSideRow++;
                builder.AddButton(220, topOffset + rightSideRow * 20, _buttonGumpId, _buttonGumpId, i + _book.BookOffset);
                builder.AddHtml(240, topOffset + rightSideRow * 20 - 3, 115, 17, $"{spells[i]}");
            }
        }
    }

    private string[] GetSpells()
    {
        var spells = new string[_book.BookCount];

        for (int i = 0; i < _book.BookCount; i++)
        {
            int spellID = i + _book.BookOffset;
            if ((_book.Content & ((ulong)1 << (i))) != 0)
            {
                var spell = SpellRegistry.NewSpell(spellID, null, null);
                spells[i] = spell?.Name;
            }
        }

        return spells;
    }

    public override void OnResponse(NetState state, in RelayInfo info)
    {
        var from = state.Mobile;
        var pack = from.Backpack;

        if (_book.Deleted || !(_book.Parent == from || pack != null && _book.Parent == pack) || !DesignContext.Check(from))
        {
            return;
        }

        int buttonID = info.ButtonID;

        if (buttonID >= _book.BookOffset && buttonID < _book.BookOffset + _book.BookCount)
        {
            var spell = SpellRegistry.NewSpell(buttonID, from, null);
            spell?.Cast();
        }
    }
}
