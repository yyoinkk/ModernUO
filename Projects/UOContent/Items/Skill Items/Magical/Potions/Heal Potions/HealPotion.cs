using ModernUO.Serialization;

namespace Server.Items;

[SerializationGenerator(0, false)]
public partial class HealPotion : BaseHealPotion
{
    [Constructible]
    public HealPotion() : base(PotionEffect.Heal)
    {
    }

    public override int MinHeal => 16;
    public override int MaxHeal => 22;
    public override double Delay => 15;
}
