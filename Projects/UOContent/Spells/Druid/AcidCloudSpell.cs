using Server.Collections;
using Server.Targeting;
using System;

namespace Server.Spells.Druid
{
    public class AcidCloudSpell : DruidSpell, ITargetingSpell<IPoint3D>
    {
        private static readonly SpellInfo _info = new(
            "Acid Cloud",
            "Vas Ort Nox",
            203,
            9051,
            Reagent.Nightshade,
            Reagent.Nightshade
        );

        public AcidCloudSpell(Mobile caster, Item scroll) : base(caster, scroll, _info)
        {
        }

        public override SpellCircle Circle => SpellCircle.Eighth;

        public override void OnCast()
        {
            Caster.Target = new SpellTarget<IPoint3D>(this, allowGround: true);
        }

        public void Target(IPoint3D p)
        {
            var loc = (p as Item)?.GetWorldLocation() ?? new Point3D(p);
            if (CheckSequence())
            {
                SpellHelper.Turn(Caster, p);

                var map = Caster.Map;

                if (map != null)
                {
                    var pvp = false;
                    using var pool = PooledRefQueue<Mobile>.Create();
                    foreach (var m in map.GetMobilesInRange(loc, 3))
                    {
                        if (Core.AOS && (m == Caster || !Caster.InLOS(m)) ||
                            !SpellHelper.ValidIndirectTarget(Caster, m, ignoreNotoriety: true) ||
                            !Caster.CanBeHarmful(m, false))
                        {
                            continue;
                        }

                        if (m.Player)
                        {
                            pvp = true;
                        }

                        pool.Enqueue(m);
                    }

                    if (pool.Count > 0)
                    {
                        double damage = Core.AOS
                            ? GetNewAosDamage(41, 1, 5, pvp)
                            : Utility.Random(27, 22);

                        while (pool.Count > 0)
                        {
                            var toDeal = damage;
                            var m = pool.Dequeue();

                            toDeal *= GetDamageScalar(m);

                            m.FixedParticles(0x113A, 10, 15, 5021, EffectLayer.Waist);
                            m.PlaySound(0x246);

                            Caster.DoHarmful(m);
                            SpellHelper.Damage(this, m, toDeal, 0, 0, 0, 100, 0);

                            if (m.Alive)
                            {
                                int level = PoisonImpl.GetPoisonLevel(Caster, m);

                                m.ApplyPoison(Caster, Poison.GetPoison(Math.Max(level, 0)));
                            }
                        }
                    }
                }
            }
        }
    }
}
