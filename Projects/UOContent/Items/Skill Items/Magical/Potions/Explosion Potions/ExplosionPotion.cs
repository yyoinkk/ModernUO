using ModernUO.Serialization;

namespace Server.Items;

[SerializationGenerator(0, false)]
public partial class ExplosionPotion : BaseExplosionPotion
{
    [Constructible]
    public ExplosionPotion() : base(PotionEffect.Explosion)
    {
    }

    public override int MinDamage => 15;
    public override int MaxDamage => 25;
}
