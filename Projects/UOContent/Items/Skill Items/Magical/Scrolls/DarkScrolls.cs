using ModernUO.Serialization;

namespace Server.Items;

[SerializationGenerator(0, false)]
public partial class DeadFleshScroll : SpellScroll
{
    [Constructible]
    public DeadFleshScroll(int amount = 1) : base(700, 0x1F6A, amount)
    {
        Name = "Dead Flesh";
        Hue = 0x026;
    }
}

[SerializationGenerator(0, false)]
public partial class DarknessScroll : SpellScroll
{
    [Constructible]
    public DarknessScroll(int amount = 1) : base(701, 0x1F6A, amount)
    {
        Name = "Darkness";
        Hue = 0x026;
    }
}

[SerializationGenerator(0, false)]
public partial class DrainLifeScroll : SpellScroll
{
    [Constructible]
    public DrainLifeScroll(int amount = 1) : base(702, 0x1F6A, amount)
    {
        Name = "Drain Life";
        Hue = 0x026;
    }
}

[SerializationGenerator(0, false)]
public partial class LowerResistanceScroll : SpellScroll
{
    [Constructible]
    public LowerResistanceScroll(int amount = 1) : base(703, 0x1F6A, amount)
    {
        Name = "Lower Resistance";
        Hue = 0x026;
    }
}

[SerializationGenerator(0, false)]
public partial class ImpaleScroll : SpellScroll
{
    [Constructible]
    public ImpaleScroll(int amount = 1) : base(704, 0x1F60, amount)
    {
        Name = "Impale";
        Hue = 0x026;
    }
}

[SerializationGenerator(0, false)]
public partial class VulnerabilityScroll : SpellScroll
{
    [Constructible]
    public VulnerabilityScroll(int amount = 1) : base(705, 0x1F6A, amount)
    {
        Name = "Vulnerability";
        Hue = 0x026;
    }
}

[SerializationGenerator(0, false)]
public partial class GhoulTouchScroll : SpellScroll
{
    [Constructible]
    public GhoulTouchScroll(int amount = 1) : base(706, 0x1F60, amount)
    {
        Name = "Ghoul Touch";
        Hue = 0x026;
    }
}

[SerializationGenerator(0, false)]
public partial class PainReflectionScroll : SpellScroll
{
    [Constructible]
    public PainReflectionScroll(int amount = 1) : base(707, 0x1F6A, amount)
    {
        Name = "Pain Reflection";
        Hue = 0x026;
    }
}

[SerializationGenerator(0, false)]
public partial class RegenerationScroll : SpellScroll
{
    [Constructible]
    public RegenerationScroll(int amount = 1) : base(708, 0x1F59, amount)
    {
        Name = "Regeneration";
        Hue = 0x026;
    }
}

[SerializationGenerator(0, false)]
public partial class UnholySpiritismScroll : SpellScroll
{
    [Constructible]
    public UnholySpiritismScroll(int amount = 1) : base(709, 0x1F59, amount)
    {
        Name = "Unholy Spiritism";
        Hue = 0x026;
    }
}

[SerializationGenerator(0, false)]
public partial class WraithAuraScroll : SpellScroll
{
    [Constructible]
    public WraithAuraScroll(int amount = 1) : base(710, 0x1F5F, amount)
    {
        Name = "Wraith Aura";
        Hue = 0x026;
    }
}

[SerializationGenerator(0, false)]
public partial class GreatHarmScroll : SpellScroll
{
    [Constructible]
    public GreatHarmScroll(int amount = 1) : base(711, 0x1F5F, amount)
    {
        Name = "Great Harm";
        Hue = 0x026;
    }
}

[SerializationGenerator(0, false)]
public partial class RaiseDeadScroll : SpellScroll
{
    [Constructible]
    public RaiseDeadScroll(int amount = 1) : base(712, 0x1F59, amount)
    {
        Name = "Raise Dead";
        Hue = 0x026;
    }
}

[SerializationGenerator(0, false)]
public partial class SacrificeScroll : SpellScroll
{
    [Constructible]
    public SacrificeScroll(int amount = 1) : base(713, 0x1F5F, amount)
    {
        Name = "Sacrifice";
        Hue = 0x026;
    }
}

[SerializationGenerator(0, false)]
public partial class GhostDragonScroll : SpellScroll
{
    [Constructible]
    public GhostDragonScroll(int amount = 1) : base(714, 0x1F59, amount)
    {
        Name = "Ghost Dragon";
        Hue = 0x026;
    }
}

[SerializationGenerator(0, false)]
public partial class KillScroll : SpellScroll
{
    [Constructible]
    public KillScroll(int amount = 1) : base(715, 0x1F5F, amount)
    {
        Name = "Kill";
        Hue = 0x026;
    }
}
