using System;
using Server.Engines.BulkOrders;
using Server.Items;
using Server.Spells;

namespace Server.Engines.Craft;

public class DefInscription : CraftSystem
{
    private static readonly Type typeofSpellScroll = typeof(SpellScroll);

    private static readonly Type[] regTypes =
    {
        typeof(BlackPearl),
        typeof(Bloodmoss),
        typeof(Garlic),
        typeof(Ginseng),
        typeof(MandrakeRoot),
        typeof(Nightshade),
        typeof(SulfurousAsh),
        typeof(SpidersSilk)
    };

    private int _circle, _mana;
    private int _index;

    public static void Initialize()
    {
        CraftSystem = new DefInscription();
    }

    private DefInscription() : base(1, 1, 1.25)
    {
    }

    public override SkillName MainSkill => SkillName.Inscribe;

    public override TextDefinition GumpTitle { get; } = 1044009;

    public static CraftSystem CraftSystem { get; private set; }

    public override double GetChanceAtMin(CraftItem item) => 0.0;

    public override int CanCraft(Mobile from, BaseTool tool, Type typeItem)
    {
        if (tool?.Deleted != false || tool.UsesRemaining < 0)
        {
            return 1044038; // You have worn out your tool!
        }

        if (!BaseTool.CheckAccessible(tool, from))
        {
            return 1044263; // The tool must be on your person to use.
        }

        var scroll = typeItem?.CreateEntityInstance<SpellScroll>();

        if (scroll != null)
        {
            var hasSpell = Spellbook.Find(from, scroll.SpellID)?.HasSpell(scroll.SpellID) == true;

            scroll.Delete();

            return hasSpell ? 0 : 1042404; // null : You don't have that spell!
        }

        return 0;
    }

    public override void PlayCraftEffect(Mobile from)
    {
        from.PlaySound(0x249);
    }

    public override int PlayEndingEffect(
        Mobile from, bool failed, bool lostMaterial, bool toolBroken, int quality,
        bool makersMark, CraftItem item
    )
    {
        if (toolBroken)
        {
            from.SendLocalizedMessage(1044038); // You have worn out your tool
        }

        if (!typeofSpellScroll.IsAssignableFrom(item.ItemType)) // not a scroll
        {
            if (failed)
            {
                if (lostMaterial)
                {
                    return 1044043; // You failed to create the item, and some of your materials are lost.
                }

                return 1044157; // You failed to create the item, but no materials were lost.
            }

            if (quality == 0)
            {
                return 502785; // You were barely able to make this item.  It's quality is below average.
            }

            if (makersMark && quality == 2)
            {
                return 1044156; // You create an exceptional quality item and affix your maker's mark.
            }

            if (quality == 2)
            {
                return 1044155; // You create an exceptional quality item.
            }

            return 1044154; // You create the item.
        }

        if (failed)
        {
            return 501630; // You fail to inscribe the scroll, and the scroll is ruined.
        }

        return 501629; // You inscribe the spell and put the scroll in your backpack.
    }

    private void AddSpell(Type type, params Reg[] regs)
    {
        double minSkill, maxSkill;

        switch (_circle)
        {
            default:
                {
                    minSkill = -25.0;
                    maxSkill = 25.0;
                    break;
                }
            case 1:
                {
                    minSkill = -10.8;
                    maxSkill = 39.2;
                    break;
                }
            case 2:
                {
                    minSkill = 03.5;
                    maxSkill = 53.5;
                    break;
                }
            case 3:
                {
                    minSkill = 17.8;
                    maxSkill = 67.8;
                    break;
                }
            case 4:
                {
                    minSkill = 32.1;
                    maxSkill = 82.1;
                    break;
                }
            case 5:
                {
                    minSkill = 46.4;
                    maxSkill = 96.4;
                    break;
                }
            case 6:
                {
                    minSkill = 60.7;
                    maxSkill = 110.7;
                    break;
                }
            case 7:
                {
                    minSkill = 75.0;
                    maxSkill = 125.0;
                    break;
                }
        }

        var index = AddCraft(
            type,
            1044369 + _circle,
            1044381 + _index++,
            minSkill,
            maxSkill,
            regTypes[(int)regs[0]],
            1044353 + (int)regs[0],
            1,
            1044361 + (int)regs[0]
        );

        for (var i = 1; i < regs.Length; ++i)
        {
            AddRes(index, regTypes[(int)regs[i]], 1044353 + (int)regs[i], 1, 1044361 + (int)regs[i]);
        }

        AddRes(index, typeof(BlankScroll), 1044377, 1, 1044378);

        SetManaReq(index, _mana, 1042403);
    }

    private void AddNecroSpell(int spell, int mana, double minSkill, Type type, params Type[] regs)
    {
        var index = AddCraft(
            type,
            1061677,
            1060509 + spell,
            minSkill,
            minSkill + 1.0, // Yes, on OSI it's only 1.0 skill diff'.  Don't blame me, blame OSI.
            regs[0],
            null,
            1,
            501627
        );

        for (var i = 1; i < regs.Length; ++i)
        {
            AddRes(index, regs[i], null, 1, 501627);
        }

        AddRes(index, typeof(BlankScroll), 1044377, 1, 1044378);
        SetManaReq(index, mana, 1042403);
    }

    private void AddDarkSpell(string spell, int circle, Type type, params Type[] regs) => AddColorSpell("Dark Magic", spell, circle, type, regs);
    private void AddDruidSpell(string spell, int circle, Type type, params Type[] regs) => AddColorSpell("Druid Magic", spell, circle, type, regs);
    private void AddLightSpell(string spell, int circle, Type type, params Type[] regs) => AddColorSpell("Light Magic", spell, circle, type, regs);

    private void AddColorSpell(string color, string spell, int circle, Type type, params Type[] regs)
    {
        int mana = circle switch
        {
            1 => 4,
            2 => 6,
            3 => 9,
            4 => 11,
            5 => 14,
            6 => 20,
            7 => 40,
            8 => 50,
        };

        var index = AddCraft(
            type,
            color,
            spell,
            circle * 10,
            circle * 10 + 45.0,
            regs[0],
            null,
            1,
            501627
        );

        for (var i = 1; i < regs.Length; ++i)
        {
            AddRes(index, regs[i], null, 1, 501627);
        }

        AddRes(index, typeof(BlankScroll), 1044377, 1, 1044378);
        SetManaReq(index, mana, 1042403);
    }

    private void AddMysticismSpell(int spell, int mana, double minSkill, double maxSkill, Type type, params Type[] regs)
    {
        var index = AddCraft(
            type,
            1111671,
            1031678 + spell,
            minSkill,
            maxSkill,
            regs[0],
            null,
            1,
            501627
        );

        for (var i = 1; i < regs.Length; ++i)
        {
            AddRes(index, regs[i], null, 1, 501627);
        }

        AddRes(index, typeof(BlankScroll), 1044377, 1, 1044378);
        SetManaReq(index, mana, 1042403);
    }

    public override void InitCraftList()
    {
        _circle = 0;
        _mana = 4;

        AddSpell(typeof(ReactiveArmorScroll), Reg.Garlic, Reg.SpidersSilk, Reg.SulfurousAsh);
        AddSpell(typeof(ClumsyScroll), Reg.Bloodmoss, Reg.Nightshade);
        AddSpell(typeof(CreateFoodScroll), Reg.Garlic, Reg.Ginseng, Reg.MandrakeRoot);
        AddSpell(typeof(FeeblemindScroll), Reg.Nightshade, Reg.Ginseng);
        AddSpell(typeof(HealScroll), Reg.Garlic, Reg.Ginseng, Reg.SpidersSilk);
        AddSpell(typeof(MagicArrowScroll), Reg.SulfurousAsh);
        AddSpell(typeof(NightSightScroll), Reg.SpidersSilk, Reg.SulfurousAsh);
        AddSpell(typeof(WeakenScroll), Reg.Garlic, Reg.Nightshade);

        _circle = 1;
        _mana = 6;

        AddSpell(typeof(AgilityScroll), Reg.Bloodmoss, Reg.MandrakeRoot);
        AddSpell(typeof(CunningScroll), Reg.Nightshade, Reg.MandrakeRoot);
        AddSpell(typeof(CureScroll), Reg.Garlic, Reg.Ginseng);
        AddSpell(typeof(HarmScroll), Reg.Nightshade, Reg.SpidersSilk);
        AddSpell(typeof(MagicTrapScroll), Reg.Garlic, Reg.SpidersSilk, Reg.SulfurousAsh);
        AddSpell(typeof(MagicUnTrapScroll), Reg.Bloodmoss, Reg.SulfurousAsh);
        AddSpell(typeof(ProtectionScroll), Reg.Garlic, Reg.Ginseng, Reg.SulfurousAsh);
        AddSpell(typeof(StrengthScroll), Reg.Nightshade, Reg.MandrakeRoot);

        _circle = 2;
        _mana = 9;

        AddSpell(typeof(BlessScroll), Reg.Garlic, Reg.MandrakeRoot);
        AddSpell(typeof(FireballScroll), Reg.BlackPearl);
        AddSpell(typeof(MagicLockScroll), Reg.Bloodmoss, Reg.Garlic, Reg.SulfurousAsh);
        AddSpell(typeof(PoisonScroll), Reg.Nightshade);
        AddSpell(typeof(TelekinesisScroll), Reg.Bloodmoss, Reg.MandrakeRoot);
        AddSpell(typeof(TeleportScroll), Reg.Bloodmoss, Reg.MandrakeRoot);
        AddSpell(typeof(UnlockScroll), Reg.Bloodmoss, Reg.SulfurousAsh);
        AddSpell(typeof(WallOfStoneScroll), Reg.Bloodmoss, Reg.Garlic);

        _circle = 3;
        _mana = 11;

        AddSpell(typeof(ArchCureScroll), Reg.Garlic, Reg.Ginseng, Reg.MandrakeRoot);
        AddSpell(typeof(ArchProtectionScroll), Reg.Garlic, Reg.Ginseng, Reg.MandrakeRoot, Reg.SulfurousAsh);
        AddSpell(typeof(CurseScroll), Reg.Garlic, Reg.Nightshade, Reg.SulfurousAsh);
        AddSpell(typeof(FireFieldScroll), Reg.BlackPearl, Reg.SpidersSilk, Reg.SulfurousAsh);
        AddSpell(typeof(GreaterHealScroll), Reg.Garlic, Reg.SpidersSilk, Reg.MandrakeRoot, Reg.Ginseng);
        AddSpell(typeof(LightningScroll), Reg.MandrakeRoot, Reg.SulfurousAsh);
        AddSpell(typeof(ManaDrainScroll), Reg.BlackPearl, Reg.SpidersSilk, Reg.MandrakeRoot);
        AddSpell(typeof(RecallScroll), Reg.BlackPearl, Reg.Bloodmoss, Reg.MandrakeRoot);

        _circle = 4;
        _mana = 14;

        AddSpell(typeof(BladeSpiritsScroll), Reg.BlackPearl, Reg.Nightshade, Reg.MandrakeRoot);
        AddSpell(typeof(DispelFieldScroll), Reg.BlackPearl, Reg.Garlic, Reg.SpidersSilk, Reg.SulfurousAsh);
        AddSpell(typeof(IncognitoScroll), Reg.Bloodmoss, Reg.Garlic, Reg.Nightshade);
        AddSpell(typeof(MagicReflectScroll), Reg.Garlic, Reg.MandrakeRoot, Reg.SpidersSilk);
        AddSpell(typeof(MindBlastScroll), Reg.BlackPearl, Reg.MandrakeRoot, Reg.Nightshade, Reg.SulfurousAsh);
        AddSpell(typeof(ParalyzeScroll), Reg.Garlic, Reg.MandrakeRoot, Reg.SpidersSilk);
        AddSpell(typeof(PoisonFieldScroll), Reg.BlackPearl, Reg.Nightshade, Reg.SpidersSilk);
        AddSpell(typeof(SummonCreatureScroll), Reg.Bloodmoss, Reg.MandrakeRoot, Reg.SpidersSilk);

        _circle = 5;
        _mana = 20;

        AddSpell(typeof(DispelScroll), Reg.Garlic, Reg.MandrakeRoot, Reg.SulfurousAsh);
        AddSpell(typeof(EnergyBoltScroll), Reg.BlackPearl, Reg.Nightshade);
        AddSpell(typeof(ExplosionScroll), Reg.Bloodmoss, Reg.MandrakeRoot);
        AddSpell(typeof(InvisibilityScroll), Reg.Bloodmoss, Reg.Nightshade);
        AddSpell(typeof(MarkScroll), Reg.Bloodmoss, Reg.BlackPearl, Reg.MandrakeRoot);
        AddSpell(typeof(MassCurseScroll), Reg.Garlic, Reg.MandrakeRoot, Reg.Nightshade, Reg.SulfurousAsh);
        AddSpell(typeof(ParalyzeFieldScroll), Reg.BlackPearl, Reg.Ginseng, Reg.SpidersSilk);
        AddSpell(typeof(RevealScroll), Reg.Bloodmoss, Reg.SulfurousAsh);

        _circle = 6;
        _mana = 40;

        AddSpell(typeof(ChainLightningScroll), Reg.BlackPearl, Reg.Bloodmoss, Reg.MandrakeRoot, Reg.SulfurousAsh);
        AddSpell(typeof(EnergyFieldScroll), Reg.BlackPearl, Reg.MandrakeRoot, Reg.SpidersSilk, Reg.SulfurousAsh);
        AddSpell(typeof(FlamestrikeScroll), Reg.SpidersSilk, Reg.SulfurousAsh);
        AddSpell(typeof(GateTravelScroll), Reg.BlackPearl, Reg.MandrakeRoot, Reg.SulfurousAsh);
        AddSpell(typeof(ManaVampireScroll), Reg.BlackPearl, Reg.Bloodmoss, Reg.MandrakeRoot, Reg.SpidersSilk);
        AddSpell(typeof(MassDispelScroll), Reg.BlackPearl, Reg.Garlic, Reg.MandrakeRoot, Reg.SulfurousAsh);
        AddSpell(typeof(MeteorSwarmScroll), Reg.Bloodmoss, Reg.MandrakeRoot, Reg.SulfurousAsh, Reg.SpidersSilk);
        AddSpell(typeof(PolymorphScroll), Reg.Bloodmoss, Reg.MandrakeRoot, Reg.SpidersSilk);

        _circle = 7;
        _mana = 50;

        AddSpell(typeof(EarthquakeScroll), Reg.Bloodmoss, Reg.MandrakeRoot, Reg.Ginseng, Reg.SulfurousAsh);
        AddSpell(typeof(EnergyVortexScroll), Reg.BlackPearl, Reg.Bloodmoss, Reg.MandrakeRoot, Reg.Nightshade);
        AddSpell(typeof(ResurrectionScroll), Reg.Bloodmoss, Reg.Garlic, Reg.Ginseng);
        AddSpell(typeof(SummonAirElementalScroll), Reg.Bloodmoss, Reg.MandrakeRoot, Reg.SpidersSilk);
        AddSpell(typeof(SummonDaemonScroll), Reg.Bloodmoss, Reg.MandrakeRoot, Reg.SpidersSilk, Reg.SulfurousAsh);
        AddSpell(typeof(SummonEarthElementalScroll), Reg.Bloodmoss, Reg.MandrakeRoot, Reg.SpidersSilk);
        AddSpell(typeof(SummonFireElementalScroll), Reg.Bloodmoss, Reg.MandrakeRoot, Reg.SpidersSilk, Reg.SulfurousAsh);
        AddSpell(typeof(SummonWaterElementalScroll), Reg.Bloodmoss, Reg.MandrakeRoot, Reg.SpidersSilk);

        if (Core.SE)
        {
            AddNecroSpell(0, 23, 39.6, typeof(AnimateDeadScroll), Reagent.GraveDust, Reagent.DaemonBlood);
            AddNecroSpell(1, 13, 19.6, typeof(BloodOathScroll), Reagent.DaemonBlood);
            AddNecroSpell(2, 11, 19.6, typeof(CorpseSkinScroll), Reagent.BatWing, Reagent.GraveDust);
            AddNecroSpell(3, 7, 19.6, typeof(CurseWeaponScroll), Reagent.PigIron);
            AddNecroSpell(4, 11, 19.6, typeof(EvilOmenScroll), Reagent.BatWing, Reagent.NoxCrystal);
            AddNecroSpell(5, 11, 39.6, typeof(HorrificBeastScroll), Reagent.BatWing, Reagent.DaemonBlood);
            AddNecroSpell(
                6,
                23,
                69.6,
                typeof(LichFormScroll),
                Reagent.GraveDust,
                Reagent.DaemonBlood,
                Reagent.NoxCrystal
            );
            AddNecroSpell(7, 17, 29.6, typeof(MindRotScroll), Reagent.BatWing, Reagent.DaemonBlood, Reagent.PigIron);
            AddNecroSpell(8, 5, 19.6, typeof(PainSpikeScroll), Reagent.GraveDust, Reagent.PigIron);
            AddNecroSpell(9, 17, 49.6, typeof(PoisonStrikeScroll), Reagent.NoxCrystal);
            AddNecroSpell(10, 29, 64.6, typeof(StrangleScroll), Reagent.DaemonBlood, Reagent.NoxCrystal);
            AddNecroSpell(
                11,
                17,
                29.6,
                typeof(SummonFamiliarScroll),
                Reagent.BatWing,
                Reagent.GraveDust,
                Reagent.DaemonBlood
            );
            AddNecroSpell(
                12,
                23,
                98.6,
                typeof(VampiricEmbraceScroll),
                Reagent.BatWing,
                Reagent.NoxCrystal,
                Reagent.PigIron
            );
            AddNecroSpell(
                13,
                41,
                79.6,
                typeof(VengefulSpiritScroll),
                Reagent.BatWing,
                Reagent.GraveDust,
                Reagent.PigIron
            );
            AddNecroSpell(14, 23, 59.6, typeof(WitherScroll), Reagent.GraveDust, Reagent.NoxCrystal, Reagent.PigIron);
            AddNecroSpell(15, 17, 79.6, typeof(WraithFormScroll), Reagent.NoxCrystal, Reagent.PigIron);
            AddNecroSpell(16, 40, 79.6, typeof(ExorcismScroll), Reagent.NoxCrystal, Reagent.GraveDust);
        }

        //AddDarkSpell("Dead Flesh", 1, typeof(DeadFleshScroll), Reagent.MandrakeRoot);
        AddDarkSpell("Darkness", 1, typeof(DarknessScroll), Reagent.SpidersSilk);
        AddDarkSpell("Drain Life", 2, typeof(DrainLifeScroll), Reagent.Ginseng, Reagent.Bone);
        AddDarkSpell("Lower Resistance", 2, typeof(LowerResistanceScroll), Reagent.SpidersSilk, Reagent.BlackPearl);
        
        AddDarkSpell("Impale", 3, typeof(ImpaleScroll), Reagent.BlackPearl, Reagent.Bone);
        AddDarkSpell("Vulnerability", 3, typeof(VulnerabilityScroll), Reagent.Bloodmoss, Reagent.FertileDirt);
        AddDarkSpell("Ghoul Touch", 4, typeof(GhoulTouchScroll), Reagent.Nightshade, Reagent.Bone);
        AddDarkSpell("Pain Reflection", 4, typeof(PainReflectionScroll), Reagent.SpidersSilk, Reagent.Bone, Reagent.VialOfBlood);
        
        AddDarkSpell("Regeneration", 5, typeof(RegenerationScroll), Reagent.BlackPearl, Reagent.Ginseng, Reagent.Bloodmoss);
        AddDarkSpell("Unholy Spiritism", 5, typeof(UnholySpiritismScroll), Reagent.BlackPearl, Reagent.Bone);
        AddDarkSpell("Wraith Aura", 6, typeof(WraithAuraScroll), Reagent.BlackPearl, Reagent.FertileDirt, Reagent.Bone);
        AddDarkSpell("Great Harm", 6, typeof(GreatHarmScroll), Reagent.SulfurousAsh, Reagent.SpidersSilk, Reagent.VialOfBlood, Reagent.BlackPearl);
        
        //AddDarkSpell("Raise Dead", 7, typeof(RaiseDeadScroll), Reagent.Bone, Reagent.VialOfBlood);
        AddDarkSpell("Sacrifice", 7, typeof(SacrificeScroll), Reagent.MandrakeRoot, Reagent.Bone, Reagent.VialOfBlood);
        AddDarkSpell("Ghost Dragon", 8, typeof(GhostDragonScroll), Reagent.Bone, Reagent.Nightshade, Reagent.VialOfBlood, Reagent.FertileDirt);
        AddDarkSpell("Kill", 8, typeof(KillScroll), Reagent.SpidersSilk, Reagent.SulfurousAsh, Reagent.Bloodmoss, Reagent.VialOfBlood);

        
        AddDruidSpell("Poison Protection", 1, typeof(PoisonProtectionScroll), Reagent.Garlic);
        AddDruidSpell("Disease", 1, typeof(DiseaseScroll), Reagent.Nightshade, Reagent.BlackPearl);
        //AddDruidSpell("Call Woodland", 2, typeof(CallWoodlandScroll), Reagent.BlackPearl, Reagent.Garlic);
        AddDruidSpell("Stone Skin", 2, typeof(StoneSkinScroll), Reagent.SpidersSilk, Reagent.Bloodmoss);
        
        AddDruidSpell("Call Lightning", 3, typeof(CallLightningScroll), Reagent.BlackPearl);
        AddDruidSpell("Remove Magic", 3, typeof(RemoveMagicScroll), Reagent.SpidersSilk, Reagent.Bloodmoss);
        AddDruidSpell("Burned Hands", 4, typeof(BurnedHandsScroll), Reagent.SpidersSilk, Reagent.SulfurousAsh);
        AddDruidSpell("Entangle", 4, typeof(EntangleScroll), Reagent.Ginseng, Reagent.BlackPearl, Reagent.MandrakeRoot);
        
        AddDruidSpell("Nature Blessing", 5, typeof(NatureBlessingScroll), Reagent.Ginseng, Reagent.Bloodmoss, Reagent.BlackPearl);
        AddDruidSpell("Gust", 5, typeof(GustScroll), Reagent.SpidersSilk, Reagent.Bloodmoss);
        AddDruidSpell("Miscast Magic", 6, typeof(MiscastMagicScroll), Reagent.BlackPearl, Reagent.Bloodmoss);
        AddDruidSpell("Insect Plague", 6, typeof(InsectPlagueScroll), Reagent.BlackPearl, Reagent.Bloodmoss);
        
        //AddDruidSpell("Turn To Stone", 7, typeof(TurnToStoneScroll), Reagent.MandrakeRoot, Reagent.Bloodmoss, Reagent.SpidersSilk);
        AddDruidSpell("Ice Strike", 7, typeof(IceStrikeScroll), Reagent.MandrakeRoot, Reagent.SulfurousAsh);
        AddDruidSpell("Acid Cloud", 8, typeof(AcidCloudScroll), Reagent.Nightshade, Reagent.Nightshade);
        AddDruidSpell("Wyvern Call", 8, typeof(WyvernCallScroll), Reagent.MandrakeRoot, Reagent.SpidersSilk, Reagent.Bloodmoss);

                
        //AddLightSpell("Bleed Protection", 1, typeof(BleedProtectionScroll), Reagent.Ginseng, Reagent.Garlic);
        AddLightSpell("Sacred Weapon", 1, typeof(SacredWeaponScroll), Reagent.Ginseng, Reagent.SulfurousAsh);
        AddLightSpell("Spirit Armor", 2, typeof(SpiritArmorScroll), Reagent.Garlic, Reagent.SpidersSilk);
        AddLightSpell("Celestial Power", 2, typeof(CelestialPowerScroll), Reagent.SpidersSilk, Reagent.SulfurousAsh);
        
        AddLightSpell("Free Action", 3, typeof(FreeActionScroll), Reagent.BlackPearl, Reagent.Ginseng);
        AddLightSpell("Ray Of Light", 3, typeof(RayOfLightScroll), Reagent.SulfurousAsh, Reagent.BlackPearl);
        AddLightSpell("Magic Fist", 4, typeof(MagicFistScroll), Reagent.SulfurousAsh, Reagent.MandrakeRoot);
        AddLightSpell("Prayer", 4, typeof(PrayerScroll), Reagent.Garlic, Reagent.SpidersSilk, Reagent.MandrakeRoot);
        
        //AddLightSpell("Heal Death Wounds", 5, typeof(HealDeathWoundsScroll), Reagent.Ginseng, Reagent.Garlic, Reagent.SulfurousAsh);
        AddLightSpell("Day Of Gods", 5, typeof(DayOfGodsScroll), Reagent.MandrakeRoot, Reagent.SulfurousAsh, Reagent.SpidersSilk);
        //AddLightSpell("Greater Cure", 6, typeof(GreaterCureScroll), Reagent.SulfurousAsh, Reagent.Garlic);
        AddLightSpell("Holy Wrath", 6, typeof(HolyWrathScroll), Reagent.SulfurousAsh, Reagent.BlackPearl);
        
        AddLightSpell("Glyph Of Guarding", 7, typeof(GlyphOfGuardingScroll), Reagent.SulfurousAsh, Reagent.SpidersSilk, Reagent.Nightshade);
        AddLightSpell("Light Elemental", 7, typeof(LightElementalScroll), Reagent.BlackPearl, Reagent.SulfurousAsh, Reagent.MandrakeRoot);
        AddLightSpell("Bolt Of Glory", 8, typeof(BoltOfGloryScroll), Reagent.SulfurousAsh, Reagent.MandrakeRoot);
        AddLightSpell("Divine Intervention", 8, typeof(DivineInterventionScroll), Reagent.Ginseng, Reagent.Garlic, Reagent.MandrakeRoot, Reagent.SpidersSilk);

        int index;

        if (Core.ML)
        {
            index = AddCraft(
                typeof(EnchantedSwitch),
                1044294,
                1072893,
                45.0,
                95.0,
                typeof(BlankScroll),
                1044377,
                1,
                1044378
            );
            AddRes(index, typeof(SpidersSilk), 1044360, 1, 1044253);
            AddRes(index, typeof(BlackPearl), 1044353, 1, 1044253);
            AddRes(index, typeof(SwitchItem), 1073464, 1, 1044253);
            ForceNonExceptional(index);
            SetNeededExpansion(index, Expansion.ML);

            index = AddCraft(typeof(RunedPrism), 1044294, 1073465, 45.0, 95.0, typeof(BlankScroll), 1044377, 1, 1044378);
            AddRes(index, typeof(SpidersSilk), 1044360, 1, 1044253);
            AddRes(index, typeof(BlackPearl), 1044353, 1, 1044253);
            AddRes(index, typeof(HollowPrism), 1072895, 1, 1044253);
            ForceNonExceptional(index);
            SetNeededExpansion(index, Expansion.ML);
        }

        // Runebook
        index = AddCraft(typeof(Runebook), 1044294, 1041267, 45.0, 95.0, typeof(BlankScroll), 1044377, 8, 1044378);
        AddRes(index, typeof(RecallScroll), 1044445, 1, 1044253);
        AddRes(index, typeof(GateTravelScroll), 1044446, 1, 1044253);

        if (Core.AOS)
        {
            AddCraft(typeof(BulkOrderBook), 1044294, 1028793, 65.0, 115.0, typeof(BlankScroll), 1044377, 10, 1044378);
        }

        if (Core.SE)
        {
            AddCraft(typeof(Spellbook), 1044294, 1023834, 50.0, 126, typeof(BlankScroll), 1044377, 10, 1044378);
        }

        /* TODO
        if (Core.ML)
        {
          index = AddCraft( typeof( ScrappersCompendium ), 1044294, 1072940, 75.0, 125.0, typeof( BlankScroll ), 1044377, 100, 1044378 );
          AddRes( index, typeof( DreadHornMane ), 1032682, 1, 1044253 );
          AddRes( index, typeof( Taint ), 1032679, 10, 1044253 );
          AddRes( index, typeof( Corruption ), 1032676, 10, 1044253 );
          AddRareRecipe( index, 400 );
          ForceNonExceptional( index );
          SetNeededExpansion( index, Expansion.ML );
        }
        */

        if (Core.SA)
        {
            AddCraft(typeof(MysticSpellbook), 1044294, 1031677, 50.0, 150.0, typeof(BlankScroll), 1044377, 10, 1044378);

            AddMysticismSpell(0, 4, -25.0, 25.0, typeof(NetherBoltScroll), Reagent.BlackPearl, Reagent.SulfurousAsh);
            AddMysticismSpell(
                1,
                4,
                -25.0,
                25.0,
                typeof(HealingStoneScroll),
                Reagent.Bone,
                Reagent.Garlic,
                Reagent.Ginseng,
                Reagent.SpidersSilk
            );
            AddMysticismSpell(
                2,
                6,
                -10.8,
                39.2,
                typeof(PurgeMagicScroll),
                Reagent.FertileDirt,
                Reagent.Garlic,
                Reagent.MandrakeRoot,
                Reagent.SulfurousAsh
            );
            AddMysticismSpell(
                3,
                6,
                -10.8,
                39.2,
                typeof(EnchantScroll),
                Reagent.SpidersSilk,
                Reagent.MandrakeRoot,
                Reagent.SulfurousAsh
            );
            AddMysticismSpell(
                4,
                9,
                3.5,
                53.5,
                typeof(SleepScroll),
                Reagent.Nightshade,
                Reagent.SpidersSilk,
                Reagent.BlackPearl
            );
            AddMysticismSpell(
                5,
                9,
                3.5,
                53.5,
                typeof(EagleStrikeScroll),
                Reagent.Bloodmoss,
                Reagent.Bone,
                Reagent.SpidersSilk,
                Reagent.MandrakeRoot
            );
            AddMysticismSpell(
                6,
                11,
                17.8,
                67.8,
                typeof(AnimatedWeaponScroll),
                Reagent.Bone,
                Reagent.BlackPearl,
                Reagent.MandrakeRoot,
                Reagent.Nightshade
            );
            AddMysticismSpell(
                7,
                11,
                17.8,
                67.8,
                typeof(StoneFormScroll),
                Reagent.Bloodmoss,
                Reagent.FertileDirt,
                Reagent.Garlic
            );
            AddMysticismSpell(
                8,
                14,
                32.1,
                82.1,
                typeof(SpellTriggerScroll),
                Reagent.DragonsBlood,
                Reagent.Garlic,
                Reagent.MandrakeRoot,
                Reagent.SpidersSilk
            );
            AddMysticismSpell(
                9,
                14,
                32.1,
                82.1,
                typeof(MassSleepScroll),
                Reagent.Ginseng,
                Reagent.Nightshade,
                Reagent.SpidersSilk
            );
            AddMysticismSpell(
                10,
                20,
                46.4,
                96.4,
                typeof(CleansingWindsScroll),
                Reagent.DragonsBlood,
                Reagent.Garlic,
                Reagent.Ginseng,
                Reagent.MandrakeRoot
            );
            AddMysticismSpell(
                11,
                20,
                46.4,
                96.4,
                typeof(BombardScroll),
                Reagent.Bloodmoss,
                Reagent.DragonsBlood,
                Reagent.Garlic,
                Reagent.SulfurousAsh
            );
            AddMysticismSpell(
                12,
                40,
                60.7,
                110.7,
                typeof(SpellPlagueScroll),
                Reagent.DaemonBone,
                Reagent.DragonsBlood,
                Reagent.Nightshade,
                Reagent.SulfurousAsh
            );
            AddMysticismSpell(
                13,
                40,
                60.7,
                110.7,
                typeof(HailStormScroll),
                Reagent.DragonsBlood,
                Reagent.Bloodmoss,
                Reagent.BlackPearl,
                Reagent.MandrakeRoot
            );
            AddMysticismSpell(
                14,
                50,
                75.0,
                125.0,
                typeof(NetherCycloneScroll),
                Reagent.MandrakeRoot,
                Reagent.Nightshade,
                Reagent.SulfurousAsh,
                Reagent.Bloodmoss
            );
            AddMysticismSpell(
                15,
                50,
                75.0,
                125.0,
                typeof(RisingColossusScroll),
                Reagent.DaemonBone,
                Reagent.DragonsBlood,
                Reagent.FertileDirt,
                Reagent.Nightshade
            );
        }

        MarkOption = true;
    }

    private enum Reg
    {
        BlackPearl,
        Bloodmoss,
        Garlic,
        Ginseng,
        MandrakeRoot,
        Nightshade,
        SulfurousAsh,
        SpidersSilk
    }
}
