using Server.Collections;

namespace Server.Spells.Seventh
{
    public class ChainLightningSpell : MagerySpell, ITargetingSpell<IPoint3D>
    {
        private static readonly SpellInfo _info = new(
            "Chain Lightning",
            "Vas Ort Grav",
            209,
            9022,
            false,
            Reagent.BlackPearl,
            Reagent.Bloodmoss,
            Reagent.MandrakeRoot,
            Reagent.SulfurousAsh
        );

        public ChainLightningSpell(Mobile caster, Item scroll = null) : base(caster, scroll, _info)
        {
        }

        public override SpellCircle Circle => SpellCircle.Seventh;

        public override bool DelayedDamage => true;

        public void Target(IPoint3D p)
        {
            var loc = (p as Item)?.GetWorldLocation() ?? new Point3D(p);
            if (SpellHelper.CheckTown(loc, Caster) && CheckSequence())
            {
                SpellHelper.Turn(Caster, p);

                var map = Caster.Map;

                if (map != null)
                {
                    var pvp = false;
                    using var pool = PooledRefQueue<Mobile>.Create();
                    foreach (var m in map.GetMobilesInRange(loc, 2))
                    {
                        if (Core.AOS && (m == Caster || !Caster.InLOS(m)) ||
                            !SpellHelper.ValidIndirectTarget(Caster, m) ||
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
                            ? GetNewAosDamage(51, 1, 5, pvp)
                            : Utility.Random(27, 22);

                        if (pool.Count > 2)
                        {
                            if (Core.AOS)
                            {
                                damage *= 2;
                            }

                            damage /= pool.Count;
                        }

                        while (pool.Count > 0)
                        {
                            var toDeal = damage;
                            var m = pool.Dequeue();

                            if (!Core.AOS && CheckResisted(m))
                            {
                                toDeal *= 0.5;

                                m.SendLocalizedMessage(501783); // You feel yourself resisting magical energy.
                            }

                            toDeal *= GetDamageScalar(m);
                            Caster.DoHarmful(m);
                            SpellHelper.Damage(this, m, toDeal, 0, 0, 0, 0, 100);

                            m.BoltEffect(0);
                        }
                    }
                    else
                    {
                        Caster.PlaySound(0x29);
                    }
                }
            }
        }

        public override void OnCast()
        {
            Caster.Target = new SpellTarget<IPoint3D>(this, allowGround: true);
        }
    }
}
