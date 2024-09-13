using ModernUO.Serialization;

namespace Server.Items;

[SerializationGenerator(0, false)]
public partial class ManaPotion : BaseManaPotion
{
    [Constructible]
    public ManaPotion() : base(PotionEffect.Mana)
    {
        Hue = 1266;
        Name = "Mana Potion";
    }

    public override int MinMana => 16;
    public override int MaxMana => 22;
    public override double Delay => 15;
}
