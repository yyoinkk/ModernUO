using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class SummonedWyvern : BaseCreature
    {
        [Constructible]
        public SummonedWyvern() : base(AIType.AI_Melee)
        {
            Body = Utility.RandomList(60, 61);
            Hue = 2072;
            BaseSoundID = 362;

            SetStr(202, 240);
            SetDex(153, 172);
            SetInt(51, 90);

            SetHits(125, 141);

            SetDamage(8, 19);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Poison, 70);

            SetResistance(ResistanceType.Physical, 35, 45);
            SetResistance(ResistanceType.Fire, 30, 40);
            SetResistance(ResistanceType.Cold, 20, 30);
            SetResistance(ResistanceType.Poison, 90, 100);
            SetResistance(ResistanceType.Energy, 30, 40);

            SetSkill(SkillName.Poisoning, 70.1, 80.0);
            SetSkill(SkillName.MagicResist, 65.1, 80.0);
            SetSkill(SkillName.Tactics, 65.1, 90.0);
            SetSkill(SkillName.Wrestling, 65.1, 80.0);

            VirtualArmor = 40;
            ControlSlots = 4;
        }

        public override string DefaultName => "a lesser wyvern";
        public override bool ReacquireOnMovement => true;
        public override Poison PoisonImmune => Poison.Deadly;
        public override Poison HitPoison => Poison.Greater;
        public override bool BleedImmune => false;
        public override bool DeleteCorpseOnDeath => true;

        private static MonsterAbility[] _abilities = { MonsterAbilities.PoisonGasCounter };
        public override MonsterAbility[] GetMonsterAbilities() => _abilities;
        public override bool CanFly => true;

        public override int GetAttackSound() => 713;

        public override int GetAngerSound() => 718;

        public override int GetDeathSound() => 716;

        public override int GetHurtSound() => 721;

        public override int GetIdleSound() => 725;
    }
}
