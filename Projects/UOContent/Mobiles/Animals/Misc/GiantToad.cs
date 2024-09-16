using ModernUO.Serialization;

namespace Server.Mobiles
{
    [TypeAlias("Server.Mobiles.Gianttoad")]
    [SerializationGenerator(0, false)]
    public partial class GiantToad : BaseCreature
    {
        [Constructible]
        public GiantToad() : base(AIType.AI_Melee)
        {
            Body = 80;
            BaseSoundID = 0x26B;

            SetStr(76, 100);
            SetDex(6, 25);
            SetInt(11, 20);

            SetHits(46, 60);
            SetMana(0);

            SetDamage(5, 17);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 20, 25);
            SetResistance(ResistanceType.Fire, 5, 10);
            SetResistance(ResistanceType.Energy, 5, 10);

            SetSkill(SkillName.MagicResist, 25.1, 40.0);
            SetSkill(SkillName.Tactics, 40.1, 60.0);
            SetSkill(SkillName.Wrestling, 40.1, 60.0);

            Fame = 750;
            Karma = -750;

            VirtualArmor = 24;

            Tamable = true;
            ControlSlots = 1;
            MinTameSkill = 77.1;
        }

        public override string CorpseName => "a giant toad corpse";
        public override string DefaultName => "a giant toad";

        public override int Blood => 1;
        public override int Hides => 2;
        public override HideType HideType => HideType.Spined;
        public override FoodType FavoriteFood => FoodType.Fish | FoodType.Meat;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Poor);
        }
    }
}
