using ModernUO.Serialization;

namespace Server.Items;

[SerializationGenerator(0, false)]
public partial class LesserHealPotion : BaseHealPotion
{
    [Constructible]
    public LesserHealPotion() : base(PotionEffect.HealLesser)
    {
    }

    public override int MinHeal => 8;
    public override int MaxHeal => 12;
    public override double Delay => 5;
}
