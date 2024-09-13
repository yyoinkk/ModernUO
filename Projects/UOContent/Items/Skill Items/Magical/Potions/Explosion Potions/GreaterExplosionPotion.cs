using ModernUO.Serialization;

namespace Server.Items;

[SerializationGenerator(0, false)]
public partial class GreaterExplosionPotion : BaseExplosionPotion
{
    [Constructible]
    public GreaterExplosionPotion() : base(PotionEffect.ExplosionGreater)
    {
    }

    public override int MinDamage => 25;
    public override int MaxDamage => 40;
}
