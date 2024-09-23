using ModernUO.Serialization;

namespace Server.Items;

[Flippable(0x1BD7, 0x1BDA)]
[SerializationGenerator(4, false)]
public partial class Board : Item, ICommodity
{
    [Constructible]
    public Board(int amount = 1) : this(CraftResource.RegularWood, amount)
    {
    }

    [Constructible]
    public Board(CraftResource resource, int amount = 1) : base(0x1BD7)
    {
        Stackable = true;
        Amount = amount;

        _resource = resource;
        Hue = CraftResources.GetHue(resource);
    }

    [SerializableProperty(0)]
    [CommandProperty(AccessLevel.GameMaster)]
    public CraftResource Resource
    {
        get => _resource;
        set
        {
            if (_resource != value)
            {
                _resource = value;
                Hue = CraftResources.GetHue(value);

                InvalidateProperties();
                this.MarkDirty();
            }
        }
    }

    int ICommodity.DescriptionNumber
    {
        get
        {
            if (_resource >= CraftResource.OakWood && _resource <= CraftResource.YewWood)
            {
                return 1075052 + ((int)_resource - (int)CraftResource.OakWood);
            }

            return _resource switch
            {
                CraftResource.Bloodwood => 1075055,
                CraftResource.Frostwood => 1075056,
                CraftResource.Heartwood => 1075062, // WHY Osi.  Why?
                _                       => LabelNumber
            };
        }
    }

    bool ICommodity.IsDeedable => true;

    public override void GetProperties(IPropertyList list)
    {
        base.GetProperties(list);

        if (!CraftResources.IsStandard(_resource))
        {
            var num = CraftResources.GetLocalizationNumber(_resource);

            if (num > 0)
            {
                list.Add(num);
            }
            else
            {
                list.Add(CraftResources.GetName(_resource));
            }
        }
    }

    private void Deserialize(IGenericReader reader, int version)
    {
        _resource = (CraftResource)reader.ReadInt();
    }
}

[SerializationGenerator(0, false)]
public partial class HeartwoodBoard : Board
{
    [Constructible]
    public HeartwoodBoard(int amount = 1) : base(CraftResource.Heartwood, amount)
    {
    }
}

[SerializationGenerator(0, false)]
public partial class BloodwoodBoard : Board
{
    [Constructible]
    public BloodwoodBoard(int amount = 1) : base(CraftResource.Bloodwood, amount)
    {
    }
}

[SerializationGenerator(0, false)]
public partial class FrostwoodBoard : Board
{
    [Constructible]
    public FrostwoodBoard(int amount = 1) : base(CraftResource.Frostwood, amount)
    {
    }
}

[SerializationGenerator(0, false)]
public partial class OakBoard : Board
{
    [Constructible]
    public OakBoard(int amount = 1) : base(CraftResource.OakWood, amount)
    {
    }
}

[SerializationGenerator(0, false)]
public partial class AshBoard : Board
{
    [Constructible]
    public AshBoard(int amount = 1) : base(CraftResource.AshWood, amount)
    {
    }
}

[SerializationGenerator(0, false)]
public partial class YewBoard : Board
{
    [Constructible]
    public YewBoard(int amount = 1) : base(CraftResource.YewWood, amount)
    {
    }
}

[SerializationGenerator(0, false)]
public partial class AngerBoard : Board
{
    [Constructible]
    public AngerBoard(int amount = 1) : base(CraftResource.Angerwood, amount)
    {
    }
}

[SerializationGenerator(0, false)]
public partial class OwlvineBoard : Board
{
    [Constructible]
    public OwlvineBoard(int amount = 1) : base(CraftResource.Owlvinewood, amount)
    {
    }
}

[SerializationGenerator(0, false)]
public partial class FlamingBoard : Board
{
    [Constructible]
    public FlamingBoard(int amount = 1) : base(CraftResource.Flamingwood, amount)
    {
    }
}

[SerializationGenerator(0, false)]
public partial class JinxBoard : Board
{
    [Constructible]
    public JinxBoard(int amount = 1) : base(CraftResource.Jinxwood, amount)
    {
    }
}

[SerializationGenerator(0, false)]
public partial class BlackwoodBoard : Board
{
    [Constructible]
    public BlackwoodBoard(int amount = 1) : base(CraftResource.Blackwood, amount)
    {
    }
}
