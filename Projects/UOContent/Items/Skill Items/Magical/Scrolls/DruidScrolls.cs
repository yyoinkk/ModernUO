using ModernUO.Serialization;

namespace Server.Items;

[SerializationGenerator(0, false)]
public partial class PoisonProtectionScroll : SpellScroll
{
    [Constructible]
    public PoisonProtectionScroll(int amount = 1) : base(800, 0x1F5A, amount)
    {
        Name = "Poison Protection";
        Hue = 0x051;
    }
}

[SerializationGenerator(0, false)]
public partial class DiseaseScroll : SpellScroll
{
    [Constructible]
    public DiseaseScroll(int amount = 1) : base(801, 0x1F5A, amount)
    {
        Name = "Disease";
        Hue = 0x051;
    }
}

[SerializationGenerator(0, false)]
public partial class CallWoodlandScroll : SpellScroll
{
    [Constructible]
    public CallWoodlandScroll(int amount = 1) : base(802, 0x1F5A, amount)
    {
        Name = "Call Woodland";
        Hue = 0x051;
    }
}

[SerializationGenerator(0, false)]
public partial class StoneSkinScroll : SpellScroll
{
    [Constructible]
    public StoneSkinScroll(int amount = 1) : base(803, 0x1F5A, amount)
    {
        Name = "Stone Skin";
        Hue = 0x051;
    }
}

[SerializationGenerator(0, false)]
public partial class CallLightningScroll : SpellScroll
{
    [Constructible]
    public CallLightningScroll(int amount = 1) : base(804, 0x1F60, amount)
    {
        Name = "Call Lightning";
        Hue = 0x051;
    }
}

[SerializationGenerator(0, false)]
public partial class RemoveMagicScroll : SpellScroll
{
    [Constructible]
    public RemoveMagicScroll(int amount = 1) : base(805, 0x1F5A, amount)
    {
        Name = "Remove Magic";
        Hue = 0x051;
    }
}

[SerializationGenerator(0, false)]
public partial class BurnedHandsScroll : SpellScroll
{
    [Constructible]
    public BurnedHandsScroll(int amount = 1) : base(806, 0x1F5A, amount)
    {
        Name = "Burned Hands";
        Hue = 0x051;
    }
}

[SerializationGenerator(0, false)]
public partial class EntangleScroll : SpellScroll
{
    [Constructible]
    public EntangleScroll(int amount = 1) : base(807, 0x1F5A, amount)
    {
        Name = "Entangle";
        Hue = 0x051;
    }
}

[SerializationGenerator(0, false)]
public partial class NatureBlessingScroll : SpellScroll
{
    [Constructible]
    public NatureBlessingScroll(int amount = 1) : base(808, 0x1F59, amount)
    {
        Name = "Nature Blessing";
        Hue = 0x051;
    }
}

[SerializationGenerator(0, false)]
public partial class GustScroll : SpellScroll
{
    [Constructible]
    public GustScroll(int amount = 1) : base(809, 0x1F5F, amount)
    {
        Name = "Gust";
        Hue = 0x051;
    }
}

[SerializationGenerator(0, false)]
public partial class MiscastMagicScroll : SpellScroll
{
    [Constructible]
    public MiscastMagicScroll(int amount = 1) : base(810, 0x1F59, amount)
    {
        Name = "Miscast Magic";
        Hue = 0x051;
    }
}

[SerializationGenerator(0, false)]
public partial class InsectPlagueScroll : SpellScroll
{
    [Constructible]
    public InsectPlagueScroll(int amount = 1) : base(811, 0x1F5F, amount)
    {
        Name = "Insect Plague";
        Hue = 0x051;
    }
}

[SerializationGenerator(0, false)]
public partial class TurnToStoneScroll : SpellScroll
{
    [Constructible]
    public TurnToStoneScroll(int amount = 1) : base(812, 0x1F59, amount)
    {
        Name = "Turn To Stone";
        Hue = 0x051;
    }
}

[SerializationGenerator(0, false)]
public partial class IceStrikeScroll : SpellScroll
{
    [Constructible]
    public IceStrikeScroll(int amount = 1) : base(813, 0x1F5F, amount)
    {
        Name = "Ice Strike";
        Hue = 0x051;
    }
}

[SerializationGenerator(0, false)]
public partial class AcidCloudScroll : SpellScroll
{
    [Constructible]
    public AcidCloudScroll(int amount = 1) : base(814, 0x1F5F, amount)
    {
        Name = "Acid Cloud";
        Hue = 0x051;
    }
}

[SerializationGenerator(0, false)]
public partial class WyvernCallScroll : SpellScroll
{
    [Constructible]
    public WyvernCallScroll(int amount = 1) : base(815, 0x1F59, amount)
    {
        Name = "Wyvern Call";
        Hue = 0x051;
    }
}
