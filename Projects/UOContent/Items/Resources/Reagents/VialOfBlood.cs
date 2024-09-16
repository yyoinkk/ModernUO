using ModernUO.Serialization;

namespace Server.Items;

[SerializationGenerator(0, false)]
public partial class VialOfBlood : BaseReagent
{
    [Constructible]
    public VialOfBlood(int amount = 1) : base(0x0E24, amount)
    {
        Hue = 0x26;
        Name = "Vial of blood";
    }
}
