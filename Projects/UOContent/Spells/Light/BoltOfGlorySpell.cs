using Server.Collections;
using Server.Targeting;
using System;

namespace Server.Spells.Light
{
    public class BoltOfGlorySpell : LightSpell, ITargetingSpell<Mobile>
    {
        private static readonly SpellInfo _info = new(
            "Bolt Of Glory",
            "Vas Por Grav",
            203,
            9051,
            Reagent.SulfurousAsh,
            Reagent.MandrakeRoot
        );

        public BoltOfGlorySpell(Mobile caster, Item scroll) : base(caster, scroll, _info)
        {
        }

        public override SpellCircle Circle => SpellCircle.Eighth;

        public override void OnCast()
        {
            Caster.Target = new SpellTarget<Mobile>(this, TargetFlags.Harmful);
        }

        public void Target(Mobile m)
        {
            if (CheckHSequence(m))
            {
                SpellHelper.Turn(Caster, m);

                SpellHelper.CheckReflect((int)Circle, Caster, ref m);

                double damage = GetNewAosDamage(43, 2, 5, m);

                DoEffects(m);
                DoDamage(Caster, m, damage);
            }
        }

        private void DoDamage(Mobile caster, Mobile target, double damage)
        {
            if (target.Map == null || caster.Map == null)
            {
                return;
            }

            using var queue = PooledRefQueue<Mobile>.Create();
            foreach (var m in target.GetMobilesInRange(1))
            {
                if (target != m && SpellHelper.ValidIndirectTarget(caster, m, ignoreNotoriety: true) && caster.CanBeHarmful(m, false) &&
                    (!Core.AOS || target.InLOS(m)))
                {
                    queue.Enqueue(m);
                }
            }

            SpellHelper.Damage(TimeSpan.Zero, target, caster, damage, 0, 50, 0, 0, 50);
            while (queue.Count > 0)
            {
                var m = queue.Dequeue();
                caster.DoHarmful(m);
                SpellHelper.Damage(TimeSpan.Zero, m, caster, damage / 2, 0, 50, 0, 0, 50);
            }
        }

        private void DoEffects(Mobile m)
        {
            m.BoltEffect(2049);
            //m.FixedParticles(0x36FE, 20, 10, 0, EffectLayer.Waist);
            m.PlaySound(0x309);
            m.FixedParticles(0x36B0, 10, 20, 4033, EffectLayer.Waist);
            m.FixedParticles(0x36BD, 5, 10, 0, EffectLayer.Waist);
            //m.FixedParticles(0xA7E3, 4, 10, 0, EffectLayer.Waist);

            //for (int x = m.X - 1; x <= m.X + 1; x++)
            //{
            //    for (int y = m.Y - 1; y <= m.Y + 1; y++)
            //    {
            //        if (x == m.X && y == m.Y)
            //        {
            //            continue;
            //        }
            //        Effects.SendLocationEffect(new Point3D(x, y, m.Z + 1), m.Map, 0x36CB, 13);
            //    }
            //}
        }
    }
}
