using ModernUO.Serialization;

namespace Server.Items;

[SerializationGenerator(0, false)]
public partial class GreaterManaPotion : BaseManaPotion
{
    [Constructible]
    public GreaterManaPotion() : base(PotionEffect.ManaGreater)
    {
        Hue = 1927;
        Name = "Greater Mana Potion";
    }

    public override int MinMana => 35;
    public override int MaxMana => 45;
    public override double Delay => 30.0;
}
