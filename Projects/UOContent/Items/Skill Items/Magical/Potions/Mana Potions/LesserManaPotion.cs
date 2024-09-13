using ModernUO.Serialization;

namespace Server.Items;

[SerializationGenerator(0, false)]
public partial class LesserManaPotion : BaseManaPotion
{
    [Constructible]
    public LesserManaPotion() : base(PotionEffect.ManaLesser)
    {
        Hue = 91;
        Name = "Lesser Mana Potion";
    }

    public override int MinMana => 8;
    public override int MaxMana => 12;
    public override double Delay => 5;
}
