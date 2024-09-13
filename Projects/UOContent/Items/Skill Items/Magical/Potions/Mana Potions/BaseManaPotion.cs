using System;
using ModernUO.Serialization;

namespace Server.Items;

[SerializationGenerator(0, false)]
public abstract partial class BaseManaPotion : BasePotion
{
    public BaseManaPotion(PotionEffect effect) : base(0xF0E, effect)
    {
    }

    public abstract int MinMana { get; }
    public abstract int MaxMana { get; }
    public abstract double Delay { get; }

    public void DoMana(Mobile from)
    {
        var min = Scale(from, MinMana);
        var max = Scale(from, MaxMana);

        from.Mana += Utility.RandomMinMax(min, max);
    }

    public override bool CanDrink(Mobile from)
    {
        if (!base.CanDrink(from))
        {
            return false;
        }

        if (from.Mana >= from.ManaMax)
        {
            from.LocalOverheadMessage(MessageType.Regular, 0x22, false, "You are already at full mana.");
            return false;
        }

        if (!from.BeginAction<BaseHealPotion>())
        {
            from.LocalOverheadMessage(MessageType.Regular, 0x22, false, "You can`t drink this potion yet.");
            return false;
        }

        return true;
    }

    public override void Drink(Mobile from)
    {
        DoMana(from);

        PlayDrinkEffect(from, 0x50);

        Timer.StartTimer(TimeSpan.FromSeconds(Delay), from.EndAction<BaseHealPotion>);
    }
}
