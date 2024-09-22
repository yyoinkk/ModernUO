using ModernUO.Serialization;

namespace Server.Items;

[SerializationGenerator(2, false)]
public abstract partial class BaseIngot : Item, ICommodity
{
    public BaseIngot(CraftResource resource, int amount = 1) : base(0x1BF2)
    {
        Stackable = true;
        Amount = amount;
        Hue = CraftResources.GetHue(resource);

        _resource = resource;
    }

    public override double DefaultWeight => 0.1;

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

    public override int LabelNumber
    {
        get
        {
            if (_resource >= CraftResource.DullCopper && _resource <= CraftResource.Valorite)
            {
                return 1042684 + (_resource - CraftResource.DullCopper);
            }

            return 1042692;
        }
    }

    int ICommodity.DescriptionNumber => LabelNumber;
    bool ICommodity.IsDeedable => true;

    private void Deserialize(IGenericReader reader, int version)
    {
        _resource = (CraftResource)reader.ReadInt();
    }

    public override void AddNameProperty(IPropertyList list)
    {
        if (Amount > 1)
        {
            list.Add(1050039, $"{Amount}\t{1027154:#}"); // ~1_NUMBER~ ~2_ITEMNAME~
        }
        else
        {
            list.Add(1027154); // ingots
        }
    }

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
}

[SerializationGenerator(0, false)]
[Flippable(0x1BF2, 0x1BEF)]
public partial class IronIngot : BaseIngot
{
    [Constructible]
    public IronIngot(int amount = 1) : base(CraftResource.Iron, amount)
    {
    }
}

[SerializationGenerator(0, false)]
[Flippable(0x1BF2, 0x1BEF)]
public partial class DullCopperIngot : BaseIngot
{
    [Constructible]
    public DullCopperIngot(int amount = 1) : base(CraftResource.DullCopper, amount)
    {
    }
}

[SerializationGenerator(0, false)]
[Flippable(0x1BF2, 0x1BEF)]
public partial class ShadowIronIngot : BaseIngot
{
    [Constructible]
    public ShadowIronIngot(int amount = 1) : base(CraftResource.ShadowIron, amount)
    {
    }
}

[SerializationGenerator(0, false)]
[Flippable(0x1BF2, 0x1BEF)]
public partial class CopperIngot : BaseIngot
{
    [Constructible]
    public CopperIngot(int amount = 1) : base(CraftResource.Copper, amount)
    {
    }
}

[SerializationGenerator(0, false)]
[Flippable(0x1BF2, 0x1BEF)]
public partial class BronzeIngot : BaseIngot
{
    [Constructible]
    public BronzeIngot(int amount = 1) : base(CraftResource.Bronze, amount)
    {
    }
}

[SerializationGenerator(0, false)]
[Flippable(0x1BF2, 0x1BEF)]
public partial class GoldIngot : BaseIngot
{
    [Constructible]
    public GoldIngot(int amount = 1) : base(CraftResource.Gold, amount)
    {
    }
}

[SerializationGenerator(0, false)]
[Flippable(0x1BF2, 0x1BEF)]
public partial class AgapiteIngot : BaseIngot
{
    [Constructible]
    public AgapiteIngot(int amount = 1) : base(CraftResource.Agapite, amount)
    {
    }
}

[SerializationGenerator(0, false)]
[Flippable(0x1BF2, 0x1BEF)]
public partial class VeriteIngot : BaseIngot
{
    [Constructible]
    public VeriteIngot(int amount = 1) : base(CraftResource.Verite, amount)
    {
    }
}

[SerializationGenerator(0, false)]
[Flippable(0x1BF2, 0x1BEF)]
public partial class ValoriteIngot : BaseIngot
{
    [Constructible]
    public ValoriteIngot(int amount = 1) : base(CraftResource.Valorite, amount)
    {
    }
}

[SerializationGenerator(0, false)]
[Flippable(0x1BF2, 0x1BEF)]
public partial class MythrilIngot : BaseIngot
{
    [Constructible]
    public MythrilIngot(int amount = 1) : base(CraftResource.Mythril, amount)
    {
    }
}

[SerializationGenerator(0, false)]
[Flippable(0x1BF2, 0x1BEF)]
public partial class AdamantIngot : BaseIngot
{
    [Constructible]
    public AdamantIngot(int amount = 1) : base(CraftResource.Adamant, amount)
    {
    }
}

[SerializationGenerator(0, false)]
[Flippable(0x1BF2, 0x1BEF)]
public partial class DeepOceanIngot : BaseIngot
{
    [Constructible]
    public DeepOceanIngot(int amount = 1) : base(CraftResource.DeepOcean, amount)
    {
    }
}

[SerializationGenerator(0, false)]
[Flippable(0x1BF2, 0x1BEF)]
public partial class AquaIngot : BaseIngot
{
    [Constructible]
    public AquaIngot(int amount = 1) : base(CraftResource.Aqua, amount)
    {
    }
}

[SerializationGenerator(0, false)]
[Flippable(0x1BF2, 0x1BEF)]
public partial class AirIngot : BaseIngot
{
    [Constructible]
    public AirIngot(int amount = 1) : base(CraftResource.Air, amount)
    {
    }
}

[SerializationGenerator(0, false)]
[Flippable(0x1BF2, 0x1BEF)]
public partial class SunshineIngot : BaseIngot
{
    [Constructible]
    public SunshineIngot(int amount = 1) : base(CraftResource.Sunshine, amount)
    {
    }
}

[SerializationGenerator(0, false)]
[Flippable(0x1BF2, 0x1BEF)]
public partial class PureTitaniumIngot : BaseIngot
{
    [Constructible]
    public PureTitaniumIngot(int amount = 1) : base(CraftResource.PureTitanium, amount)
    {
    }
}

[SerializationGenerator(0, false)]
[Flippable(0x1BF2, 0x1BEF)]
public partial class DruidSilverIngot : BaseIngot
{
    [Constructible]
    public DruidSilverIngot(int amount = 1) : base(CraftResource.DruidSilver, amount)
    {
    }
}

[SerializationGenerator(0, false)]
[Flippable(0x1BF2, 0x1BEF)]
public partial class PurpleCrystalIngot : BaseIngot
{
    [Constructible]
    public PurpleCrystalIngot(int amount = 1) : base(CraftResource.PurpleCrystal, amount)
    {
    }
}

[SerializationGenerator(0, false)]
[Flippable(0x1BF2, 0x1BEF)]
public partial class WyrmEyeIngot : BaseIngot
{
    [Constructible]
    public WyrmEyeIngot(int amount = 1) : base(CraftResource.WyrmEye, amount)
    {
    }
}

[SerializationGenerator(0, false)]
[Flippable(0x1BF2, 0x1BEF)]
public partial class BloodRockIngot : BaseIngot
{
    [Constructible]
    public BloodRockIngot(int amount = 1) : base(CraftResource.BloodRock, amount)
    {
    }
}
