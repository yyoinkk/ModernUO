using ModernUO.Serialization;

namespace Server.Items;

[SerializationGenerator(0, false)]
public partial class Sand : Item, ICommodity
{
    [Constructible]
    public Sand(int amount = 1) : base(0x11EA)
    {
        Stackable = true;
        Weight = 0.2;
    }

    public override int LabelNumber => 1044626; // sand
    int ICommodity.DescriptionNumber => LabelNumber;
    bool ICommodity.IsDeedable => true;
}
