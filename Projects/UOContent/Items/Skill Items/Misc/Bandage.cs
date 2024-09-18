using System;
using System.Collections.Generic;
using ModernUO.Serialization;
using Server.Engines.ConPVP;
using Server.Factions;
using Server.Gumps;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Items;

[SerializationGenerator(0, false)]
public partial class Bandage : Item
{
    public static int Range = 2;

    [Constructible]
    public Bandage(int amount = 1) : base(0xE21)
    {
        Stackable = true;
        Amount = amount;
    }

    public override double DefaultWeight => 0.05;

    public override void OnDoubleClick(Mobile from)
    {
        if (BandageContext.GetContext(from) != null)
        {
            from.SendLocalizedMessage(1049616); // You are too busy to do that at the moment.
            return;
        }

        if (from.InRange(GetWorldLocation(), Range))
        {
            if (from.Skills[SkillName.Hiding].Value < 100)
            {
                from.RevealingAction();
            }

            from.SendLocalizedMessage(500948); // Who will you use the bandages on?

            from.Target = new InternalTarget(this);
        }
        else
        {
            from.SendLocalizedMessage(500295); // You are too far away to do that.
        }
    }

    public static void BandageTargetRequest(Mobile from, Item item, Mobile target)
    {
        if (item is not Bandage b || b.Deleted)
        {
            return;
        }

        if (BandageContext.GetContext(from) != null)
        {
            from.SendLocalizedMessage(1049616); // You are too busy to do that at the moment.
            return;
        }

        if (!from.InRange(b.GetWorldLocation(), Range))
        {
            from.SendLocalizedMessage(500295); // You are too far away to do that.
            return;
        }

        if (from.Target != null)
        {
            Target.Cancel(from);
            from.Target = null;
        }

        if (from.Skills[SkillName.Hiding].Value < 100)
        {
            from.RevealingAction();
        }

        from.SendLocalizedMessage(500948); // Who will you use the bandages on?

        new InternalTarget(b).Invoke(from, target);
    }

    private class InternalTarget : Target
    {
        private readonly Bandage _bandage;

        public InternalTarget(Bandage bandage) : base(Bandage.Range, false, TargetFlags.Beneficial) =>
            _bandage = bandage;

        protected override void OnTarget(Mobile from, object targeted)
        {
            if (_bandage.Deleted)
            {
                return;
            }

            if (targeted is Mobile mobile)
            {
                if (from.InRange(_bandage.GetWorldLocation(), Bandage.Range))
                {
                    if (!(BandageContext.BeginHeal(from, mobile) == null || DuelContext.IsFreeConsume(from)))
                    {
                        _bandage.Consume();
                    }
                }
                else
                {
                    from.SendLocalizedMessage(500295); // You are too far away to do that.
                }

                return;
            }

            if (targeted is PlagueBeastInnard innard)
            {
                if (innard.OnBandage(from))
                {
                    _bandage.Consume();
                }

                return;
            }

            from.SendLocalizedMessage(500970); // Bandages can not be used on that.
        }

        protected override void OnNonlocalTarget(Mobile from, object targeted)
        {
            if (targeted is PlagueBeastInnard innard)
            {
                if (innard.OnBandage(from))
                {
                    _bandage.Consume();
                }
            }
            else
            {
                base.OnNonlocalTarget(from, targeted);
            }
        }
    }
}

public class BandageContext : Timer
{
    private static readonly Dictionary<Mobile, BandageContext> _table = new();

    public BandageContext(Mobile healer, Mobile patient, TimeSpan healingTime) : base(TimeSpan.FromSeconds(0.1), TimeSpan.FromSeconds(_interval))
    {
        Healer = healer;
        Patient = patient;
        _endTime = Core.Now + healingTime;
        _isRessurect = !patient.Alive;
        _primary = GetPrimarySkill(patient);
        _secondary = GetSecondarySkill(patient);
        _healed = 0;
    }

    private static readonly double _interval = 0.9;

    public Mobile Healer { get; }

    public Mobile Patient { get; }

    private readonly DateTime _endTime;
    private readonly bool _isRessurect;
    private readonly SkillName _primary;
    private readonly SkillName _secondary;
    private int _healed;

    public int Slips { get; set; }

    public void Slip()
    {
        Healer.SendLocalizedMessage(500961); // Your fingers slip!
        ++Slips;
    }

    public void StopHeal()
    {
        if (_healed > 0)
        {
            Healer.SendMessage($"Healed: {_healed}hp.");
        }
        else if (!_isRessurect)
        {
            Healer.SendLocalizedMessage(500968);
        }

        BuffInfo.RemoveBuff(Healer, BuffIcon.Healing);

        _table.Remove(Healer);
        Stop();
    }

    public static BandageContext GetContext(Mobile healer)
    {
        _table.TryGetValue(healer, out var bc);
        return bc;
    }

    public static SkillName GetPrimarySkill(Mobile m)
    {
        if (!m.Player && (m.Body.IsMonster || m.Body.IsAnimal))
        {
            return SkillName.Veterinary;
        }

        return SkillName.Healing;
    }

    public static SkillName GetSecondarySkill(Mobile m)
    {
        if (!m.Player && (m.Body.IsMonster || m.Body.IsAnimal))
        {
            return SkillName.AnimalLore;
        }

        return SkillName.Anatomy;
    }

    protected override void OnTick()
    {
        int healerNumber = -1;
        int patientNumber = -1;
        var playSound = false;
        bool checkSkills = false;

        var petPatient = Patient as BaseCreature;

        if (!Healer.Alive)
        {
            Healer.SendLocalizedMessage(500962); // You were unable to finish your work before you died.
            StopHeal();
            return;
        }

        if (!Healer.InRange(Patient, Bandage.Range))
        {
            Healer.SendLocalizedMessage(500963); // You did not stay close enough to heal your target.
            StopHeal();
            return;
        }

        var primary = Healer.Skills[_primary].Value;
        var secondary = Healer.Skills[_secondary].Value;
        // Regular tick
        if (Core.Now < _endTime)
        {
            if (_isRessurect)
            {
                return;
            }

            if (Patient.Poisoned && Utility.RandomBool())
            {
                var chance = Math.Clamp((primary - 40.0) / 70.0 - Patient.Poison.Level * 0.1 - Slips * 0.1, 0.10, 0.55);

                //Healer.SendMessage($"PoisonLvl: {Patient.Poison.Level}. Cure chance {chance}");

                if (chance < Utility.RandomDouble())
                {
                    return;
                }

                if (Patient.CurePoison(Healer))
                {
                    healerNumber = Healer == Patient ? -1 : 1010058; // You have cured the target of all poisons.
                    patientNumber = 1010059;                         // You have been cured of all poisons.
                }
            }
            else
            {
                if (BleedAttack.IsBleeding(Patient))
                {
                    Healer.SendLocalizedMessage(1060088);  // You bind the wound and stop the bleeding
                    Patient.SendLocalizedMessage(1060167); // The bleeding wounds have healed, you are no longer bleeding!

                    // TODO: probably with chance
                    BleedAttack.EndBleed(Patient, false);
                    return;
                }

                if (MortalStrike.IsWounded(Patient))
                {
                    if (Healer == Patient)
                    {
                        Healer.SendLocalizedMessage(1005000); // You cannot heal yourself in your current state.
                    }
                    else
                    {
                        Healer.SendLocalizedMessage(1010398); // You cannot heal that target in their current state.
                    }

                    return;
                }

                if (Patient.Hits == Patient.HitsMax)
                {
                    Healer.SendLocalizedMessage(1010395); // You heal what little damage your patient had.
                    StopHeal();
                }
                else
                {
                    checkSkills = false;

                    var chance = (primary + 10.0) / 100.0 - Slips * 0.1;

                    if (chance > Utility.RandomDouble())
                    {
                        double min, max;

                        min = secondary / 50.0 + primary / 30.0;
                        max = secondary / 42.0 + primary / 22.0 + 1;

                        var toHeal = min + Utility.RandomDouble() * (max - min);

                        if (Patient.Body.IsMonster || Patient.Body.IsAnimal)
                        {
                            toHeal += Patient.HitsMax / 100.0;
                        }

                        toHeal -= toHeal * Slips * 0.2;

                        if (Patient.Poisoned)
                        {
                            toHeal *= 0.8;
                        }

                        if (toHeal < 1)
                        {
                            toHeal = 1;
                        }

                        _healed += (int)toHeal;

                        Patient.Heal((int)toHeal, Healer, false);

                        if (Patient.Hits == Patient.HitsMax)
                        {
                            checkSkills = true;
                            healerNumber = 500969; // You finish applying the bandages.
                            StopHeal();
                        }
                    }
                }
            }
        }
        // Healing time over, trying resurrect
        else if (Core.Now > _endTime && _isRessurect)
        {
            StopHeal();
            if (!Patient.Alive || petPatient?.IsDeadPet == true)
            {
                if (Patient.Map?.CanFit(Patient.Location, 16, false, false) != true)
                {
                    Healer.SendLocalizedMessage(501042);  // Target can not be resurrected at that location.
                    Patient.SendLocalizedMessage(502391); // Thou can not be resurrected there!
                    return;
                }

                if (Patient.Region?.IsPartOf("Khaldun") == true)
                {
                    // The veil of death in this area is too strong and resists thy efforts to restore life.
                    Healer.SendLocalizedMessage(1010395);
                    return;
                }

                var chance = (primary - 68.0) / 50.0;
                checkSkills = primary >= 95.0 && secondary >= 80.0;

                //Healer.SendMessage($"Res chance {chance}");

                // TODO: Dbl check doesn't check for faction of the horse here?
                if (!(checkSkills && chance > Utility.RandomDouble() && Slips == 0)
                    && (!Core.SE || petPatient is not FactionWarHorse || petPatient.ControlMaster != Healer))
                {
                    if (petPatient?.IsDeadPet == true)
                    {
                        Healer.SendLocalizedMessage(503256); // You fail to resurrect the creature.
                    }
                    else
                    {
                        Healer.SendLocalizedMessage(500966); // You are unable to resurrect your patient.
                    }

                    return;
                }

                healerNumber = 500965; // You are able to resurrect your patient.

                Patient.PlaySound(0x214);
                Patient.FixedEffect(0x376A, 10, 16);

                if (petPatient?.IsDeadPet == true)
                {
                    var master = petPatient.ControlMaster;

                    if (master != null && Healer == master)
                    {
                        petPatient.ResurrectPet();

                        for (var i = 0; i < petPatient.Skills.Length; ++i)
                        {
                            petPatient.Skills[i].Base -= 0.1;
                        }
                    }
                    else if (master?.InRange(petPatient, 3) == true)
                    {
                        healerNumber = 503255; // You are able to resurrect the creature.

                        master.SendGump(new PetResurrectGump(Healer, petPatient));
                    }
                    else
                    {
                        var found = false;

                        var friends = petPatient.Friends;

                        for (var i = 0; i < friends?.Count; ++i)
                        {
                            var friend = friends[i];

                            if (friend.InRange(petPatient, 3))
                            {
                                healerNumber = 503255; // You are able to resurrect the creature.

                                friend.SendGump(new PetResurrectGump(Healer, petPatient));

                                found = true;
                                break;
                            }
                        }

                        if (!found)
                        {
                            healerNumber = 1049670; // The pet's owner must be nearby to attempt resurrection.
                        }
                    }
                }
                else
                {
                    Patient.SendGump(new ResurrectGump(Healer));
                }
            }
            playSound = true;
        }
        // Healing over
        else
        {
            StopHeal();
            healerNumber = 500969; // You finish applying the bandages.
            checkSkills = true;
        }

        if (healerNumber != -1)
        {
            Healer.SendLocalizedMessage(healerNumber);
        }

        if (patientNumber != -1)
        {
            Patient.SendLocalizedMessage(patientNumber);
        }

        if (playSound)
        {
            Patient.PlaySound(0x57);
        }

        if (checkSkills)
        {
            Healer.CheckSkill(_primary, 10.0, 140.0);
        }
    }

    public static BandageContext BeginHeal(Mobile healer, Mobile patient)
    {
        if (GetContext(healer) != null)
        {
            return null;
        }

        var creature = patient as BaseCreature;

        if (patient is Golem)
        {
            healer.SendLocalizedMessage(500970); // Bandages cannot be used on that.
        }
        else if (creature?.IsAnimatedDead == true)
        {
            healer.SendLocalizedMessage(500951); // You cannot heal that.
        }
        else if (!patient.Poisoned && patient.Hits == patient.HitsMax && !BleedAttack.IsBleeding(patient) &&
                 creature?.IsDeadPet != true)
        {
            healer.SendLocalizedMessage(500955); // That being is not damaged!
        }
        else if (!patient.Alive && patient.Map?.CanFit(patient.Location, 16, false, false) != true)
        {
            healer.SendLocalizedMessage(501042); // Target cannot be resurrected at that location.
        }
        else
        {
            if (healer.CanBeBeneficial(patient, true, true))
            { 
                healer.DoBeneficial(patient);

                var onSelf = healer == patient;

                var primary = GetPrimarySkill(patient);
                var secondary = GetSecondarySkill(patient);

                double seconds = patient.Alive ? healer.Skills[primary].Value / 11 : 8.0;

                healer.SendMessage($"Healing time: {(int)seconds}s");

                seconds *= 1000;
                var context = new BandageContext(healer, patient, TimeSpan.FromMilliseconds(seconds));

                _table[healer] = context;
                context.Start();

                if (!onSelf)
                {
                    patient.SendLocalizedMessage(1008078, false, healer.Name); // : Attempting to heal you.
                }

                BuffInfo.AddBuff(patient, new BuffInfo(BuffIcon.Healing, 1044077, 1151400, TimeSpan.FromMilliseconds(seconds), patient, patient.RawName)); //Healing | Target: name

                healer.SendLocalizedMessage(500956); // You begin applying the bandages.

                return context;
            }
        }

        return null;
    }
}
