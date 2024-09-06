using ModernUO.CodeGeneratedEvents;
using ModernUO.Serialization;
using Server.Items;
using System.Collections.Generic;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class SummonedGhostDragon : BaseCreature
    {
        [Constructible]
        public SummonedGhostDragon() : base(AIType.AI_Mage)
        {
            Body = Utility.RandomList(60, 61);
            BaseSoundID = 362;
            Hue = 0x4001;

            SetStr(201, 230);
            SetDex(133, 152);
            SetInt(101, 140);

            SetHits(241, 258);

            SetDamage(11, 17);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Cold, 30);
            SetDamageType(ResistanceType.Energy, 20);

            SetResistance(ResistanceType.Physical, 45, 50);
            SetResistance(ResistanceType.Fire, 50, 60);
            SetResistance(ResistanceType.Cold, 40, 50);
            SetResistance(ResistanceType.Poison, 20, 30);
            SetResistance(ResistanceType.Energy, 30, 40);

            SetSkill(SkillName.MagicResist, 65.1, 80.0);
            SetSkill(SkillName.Tactics, 65.1, 90.0);
            SetSkill(SkillName.Wrestling, 65.1, 80.0);

            VirtualArmor = 40;
            ControlSlots = 4;

            AddItem(new LightSource());
        }

        public override void OnDeath(Container c)
        {
            Effects.PlaySound(this, GetDeathSound());
            base.OnDeath(c);
        }

        public override string DefaultName => "a ghost dragon";
        public override bool BleedImmune => true;
        public override Poison PoisonImmune => Poison.Lethal;

        public override bool DeleteCorpseOnDeath => true;

        public override bool CanFly => true;

        private static MonsterAbility[] _abilities = { MonsterAbilities.DrainLifeAttack };
        public override MonsterAbility[] GetMonsterAbilities() => _abilities;
    }
}
