using ModernUO.Serialization;

namespace Server.Items;

[SerializationGenerator(0, false)]
public partial class BleedProtectionSpell : SpellScroll
{
    [Constructible]
    public BleedProtectionSpell(int amount = 1) : base(900, 0x1F5A, amount)
    {
        Name = "Bleed Protection";
        Hue = 0x065;
    }
}

[SerializationGenerator(0, false)]
public partial class SacredWeaponScroll : SpellScroll
{
    [Constructible]
    public SacredWeaponScroll(int amount = 1) : base(901, 0x1F5A, amount)
    {
        Name = "Sacred Weapon";
        Hue = 0x065;
    }
}

[SerializationGenerator(0, false)]
public partial class SpiritArmorScroll : SpellScroll
{
    [Constructible]
    public SpiritArmorScroll(int amount = 1) : base(902, 0x1F5A, amount)
    {
        Name = "Spirit Armor";
        Hue = 0x065;
    }
}

[SerializationGenerator(0, false)]
public partial class CelestialPowerScroll : SpellScroll
{
    [Constructible]
    public CelestialPowerScroll(int amount = 1) : base(903, 0x1F5A, amount)
    {
        Name = "Celestial Power";
        Hue = 0x065;
    }
}

[SerializationGenerator(0, false)]
public partial class FreeActionScroll : SpellScroll
{
    [Constructible]
    public FreeActionScroll(int amount = 1) : base(904, 0x1F5A, amount)
    {
        Name = "Free Action";
        Hue = 0x065;
    }
}

[SerializationGenerator(0, false)]
public partial class RayOfLightScroll : SpellScroll
{
    [Constructible]
    public RayOfLightScroll(int amount = 1) : base(905, 0x1F60, amount)
    {
        Name = "Ray Of Light";
        Hue = 0x065;
    }
}

[SerializationGenerator(0, false)]
public partial class MagicFistScroll : SpellScroll
{
    [Constructible]
    public MagicFistScroll(int amount = 1) : base(906, 0x1F60, amount)
    {
        Name = "Magic Fist";
        Hue = 0x065;
    }
}

[SerializationGenerator(0, false)]
public partial class PrayerScroll : SpellScroll
{
    [Constructible]
    public PrayerScroll(int amount = 1) : base(907, 0x1F5A, amount)
    {
        Name = "Prayer";
        Hue = 0x065;
    }
}

[SerializationGenerator(0, false)]
public partial class HealDeathWoundsScroll : SpellScroll
{
    [Constructible]
    public HealDeathWoundsScroll(int amount = 1) : base(908, 0x1F59, amount)
    {
        Name = "Heal Death Wounds";
        Hue = 0x065;
    }
}

[SerializationGenerator(0, false)]
public partial class DayOfGodsScroll : SpellScroll
{
    [Constructible]
    public DayOfGodsScroll(int amount = 1) : base(909, 0x1F59, amount)
    {
        Name = "Day Of Gods";
        Hue = 0x065;
    }
}

[SerializationGenerator(0, false)]
public partial class GreaterCureScroll : SpellScroll
{
    [Constructible]
    public GreaterCureScroll(int amount = 1) : base(910, 0x1F59, amount)
    {
        Name = "Greater Cure";
        Hue = 0x065;
    }
}

[SerializationGenerator(0, false)]
public partial class HolyWrathScroll : SpellScroll
{
    [Constructible]
    public HolyWrathScroll(int amount = 1) : base(911, 0x1F5F, amount)
    {
        Name = "Holy Wrath";
        Hue = 0x065;
    }
}

[SerializationGenerator(0, false)]
public partial class GlyphOfGuardingScroll : SpellScroll
{
    [Constructible]
    public GlyphOfGuardingScroll(int amount = 1) : base(912, 0x1F5F, amount)
    {
        Name = "Glyph Of Guarding";
        Hue = 0x065;
    }
}

[SerializationGenerator(0, false)]
public partial class LightElementalScroll : SpellScroll
{
    [Constructible]
    public LightElementalScroll(int amount = 1) : base(913, 0x1F59, amount)
    {
        Name = "Light Elemental";
        Hue = 0x065;
    }
}

[SerializationGenerator(0, false)]
public partial class BoltOfGloryScroll : SpellScroll
{
    [Constructible]
    public BoltOfGloryScroll(int amount = 1) : base(914, 0x1F5F, amount)
    {
        Name = "Bolt Of Glory";
        Hue = 0x065;
    }
}

[SerializationGenerator(0, false)]
public partial class DivineInterventionScroll : SpellScroll
{
    [Constructible]
    public DivineInterventionScroll(int amount = 1) : base(915, 0x1F59, amount)
    {
        Name = "Divine Intervention";
        Hue = 0x065;
    }
}
