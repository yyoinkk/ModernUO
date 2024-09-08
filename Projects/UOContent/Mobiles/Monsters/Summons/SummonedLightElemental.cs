using ModernUO.Serialization;
using System;
using Server.Engines.Plants;
using Server.Ethics;
using Server.Factions;
using Server.Items;
using Server.Misc;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class SummonedLightElemental : BaseCreature
    {
        [Constructible]
        public SummonedLightElemental() : base(AIType.AI_Mage)
        {
            Body = 58;
            BaseSoundID = 466;
            Hue = 0x810;

            SetStr(196, 225);
            SetDex(196, 225);
            SetInt(196, 225);

            SetHits(168, 175);

            SetDamage(11, 16);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Energy, 50);

            SetResistance(ResistanceType.Physical, 35, 45);
            SetResistance(ResistanceType.Fire, 20, 40);
            SetResistance(ResistanceType.Cold, 10, 30);
            SetResistance(ResistanceType.Poison, 5, 10);
            SetResistance(ResistanceType.Energy, 50, 70);

            SetSkill(SkillName.EvalInt, 80.0);
            SetSkill(SkillName.Magery, 80.0);
            SetSkill(SkillName.MagicResist, 80.0);
            SetSkill(SkillName.Tactics, 80.0);
            SetSkill(SkillName.Wrestling, 80.0);

            VirtualArmor = 40;
            ControlSlots = 4;
        }

        public override string DefaultName => "a Light Elemental";

        public override bool BleedImmune => true;
        public override Poison PoisonImmune => Poison.Greater;

        public override bool DeleteCorpseOnDeath => true;

        public override bool CanFly => true;

        private static MonsterAbility[] _abilities = { MonsterAbilities.EnergyBoltCounter };
        public override MonsterAbility[] GetMonsterAbilities() => _abilities;
    }
}
