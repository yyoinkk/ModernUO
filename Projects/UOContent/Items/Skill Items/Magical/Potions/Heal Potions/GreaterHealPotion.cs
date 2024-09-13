using ModernUO.Serialization;

namespace Server.Items;

[SerializationGenerator(0, false)]
public partial class GreaterHealPotion : BaseHealPotion
{
    [Constructible]
    public GreaterHealPotion() : base(PotionEffect.HealGreater)
    {
    }

    public override int MinHeal => 35;
    public override int MaxHeal => 45;
    public override double Delay => 30.0;
}
