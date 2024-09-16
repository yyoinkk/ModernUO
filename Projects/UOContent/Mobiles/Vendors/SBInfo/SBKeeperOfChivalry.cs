using System.Collections.Generic;
using Server.Items;

namespace Server.Mobiles
{
    public class SBKeeperOfChivalry : SBInfo
    {
        public override IShopSellInfo SellInfo { get; } = new InternalSellInfo();

        public override List<GenericBuyInfo> BuyInfo { get; } = new InternalBuyInfo();

        public class InternalBuyInfo : List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                //Add(new GenericBuyInfo(typeof(BookOfChivalry), 140, 20, 0x2252, 0));
            }
        }

        public class InternalSellInfo : GenericSellInfo
        {
        }
    }
}
