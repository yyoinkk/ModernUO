using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles
{
    [TypeAlias("Server.Mobiles.Iceserpant")]
    [SerializationGenerator(0, false)]
    public partial class IceSerpent : BaseCreature
    {
        [Constructible]
        public IceSerpent() : base(AIType.AI_Melee)
        {
            Body = 89;
            BaseSoundID = 219;

            SetStr(216, 245);
            SetDex(26, 50);
            SetInt(66, 85);

            SetHits(130, 147);
            SetMana(0);

            SetDamage(7, 17);

            SetDamageType(ResistanceType.Physical, 10);
            SetDamageType(ResistanceType.Cold, 90);

            SetResistance(ResistanceType.Physical, 30, 35);
            SetResistance(ResistanceType.Cold, 80, 90);
            SetResistance(ResistanceType.Poison, 15, 25);
            SetResistance(ResistanceType.Energy, 10, 20);

            SetSkill(SkillName.Anatomy, 27.5, 50.0);
            SetSkill(SkillName.MagicResist, 25.1, 40.0);
            SetSkill(SkillName.Tactics, 75.1, 80.0);
            SetSkill(SkillName.Wrestling, 60.1, 80.0);

            Fame = 3500;
            Karma = -3500;

            VirtualArmor = 32;

            PackItem(Loot.RandomArmorOrShieldOrWeapon());

            PackItem(
                Utility.Random(10) switch
                {
                    0 => new LeftArm(),
                    1 => new RightArm(),
                    2 => new Torso(),
                    3 => new Bone(),
                    4 => new RibCage(),
                    5 => new RibCage(),
                    _ => new BonePile() // 6-9
                }
            );

            if (Utility.RandomDouble() < 0.025)
            {
                PackItem(new GlacialStaff());
            }
        }

        public override string CorpseName => "an ice serpent corpse";
        public override string DefaultName => "a giant ice serpent";

        public override bool DeathAdderCharmable => true;

        public override int Blood => 1;
        public override int Meat => 2;
        public override int Hides => 5;
        public override HideType HideType => HideType.Spined;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Meager);
        }
    }
}
