using ModernUO.Serialization;
using Server.Collections;
using Server.Items;
using Server.Regions;
using Server.Targeting;
using System;
using System.Collections.Generic;

namespace Server.Spells.Light
{
    public class GlyphOfGuardingSpell : LightSpell, ITargetingSpell<IPoint3D>
    {
        private static readonly SpellInfo _info = new(
            "Glyph Of Guarding",
            "Kal Ort Ylem",
            203,
            9051,
            Reagent.SulfurousAsh,
            Reagent.SpidersSilk,
            Reagent.Nightshade
        );

        public GlyphOfGuardingSpell(Mobile caster, Item scroll) : base(caster, scroll, _info)
        {
        }

        private static readonly Dictionary<Mobile, GlyphTrap> _table = new();

        public override SpellCircle Circle => SpellCircle.Seventh;

        public override void OnCast()
        {
            Caster.Target = new SpellTarget<IPoint3D>(this, allowGround:true, TargetFlags.None);
        }

        public void Target(IPoint3D p)
        {
            var map = Caster.Map;

            SpellHelper.GetSurfaceTop(ref p);

            var point = new Point3D(p);

            if (SpellHelper.CheckMulti(point, map))
            {
                Caster.SendLocalizedMessage(502831); // Cannot teleport to that spot.
            }
            else if (Region.Find(point, map).IsPartOf<HouseRegion>())
            {
                Caster.SendLocalizedMessage(502829); // Cannot teleport to that spot.
            }
            else if (CheckSequence())
            {
                DeleteGlyph(Caster);

                //glyphs randomization here

                GlyphTrap glyph = new GlyphTrap(0x3196, this, Caster, point, 30);
                _table[Caster] = glyph;
            }
        }

        public static void DeleteGlyph(Mobile caster)
        {
            if (_table.Remove(caster, out var glyph))
            {
                glyph.Delete();
            }
        }
    }

    [SerializationGenerator(0, false)]
    public partial class GlyphTrap : BaseTrap
    {
        private readonly Spell _spell;
        private readonly Mobile _caster;
        private readonly TimeSpan _duration;
        private readonly TimerExecutionToken _executionToken;
        private readonly Point3D _location;

        public GlyphTrap(int itemID, Spell spell, Mobile caster, Point3D location, int duration) : base(itemID)
        {
            _spell = spell;
            _caster = caster;
            _location = location;
            _duration = TimeSpan.FromSeconds(duration);

            MoveToWorld(_location, _caster.Map);

            Timer.StartTimer(_duration, Delete, out _executionToken);
        }

        public override void OnAfterDelete()
        {
            base.OnAfterDelete();
            _executionToken.Cancel();
        }

        public override int PassiveTriggerRange => 2;
        public override TimeSpan ResetDelay => TimeSpan.Zero;

        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (m.Location == oldLocation)
            {
                return;
            }

            if (Utility.InRange(GetWorldLocation(), m.Location, PassiveTriggerRange) || Utility.InRange(GetWorldLocation(), oldLocation, PassiveTriggerRange))
            {
                OnTrigger(m);
            }
        }

        public override void OnTrigger(Mobile from)
        {
            if (Map == null || from == null || !from.Alive)
            {
                return;
            }

            int damage = _spell.GetNewAosDamage(40, 1, 5, from);

            using var queue = PooledRefQueue<Mobile>.Create();
            foreach (var m in GetMobilesInRange(PassiveTriggerRange))
            {
                if (SpellHelper.ValidIndirectTarget(_caster, m, ignoreNotoriety: true) && _caster.CanBeHarmful(m, false))
                {
                    queue.Enqueue(m);
                }
            }

            while (queue.Count > 0)
            {
                var m = queue.Dequeue();
                _caster.DoHarmful(m);
                SpellHelper.Damage(TimeSpan.Zero, m, _caster, damage, 0, 100, 0, 0, 0);

                m.FixedParticles(0x36BD, 20, 10, 5044, EffectLayer.Head);
                m.PlaySound(0x307);
            }

            Effects.SendLocationParticles(
                EffectItem.Create(_location, Map, EffectItem.DefaultDuration),
                0x36B0,
                8,
                20,
                5042
            );

            GlyphOfGuardingSpell.DeleteGlyph(_caster);
        }
    }
}
