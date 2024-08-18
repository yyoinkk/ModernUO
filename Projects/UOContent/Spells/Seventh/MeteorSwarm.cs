using Server.Collections;

namespace Server.Spells.Seventh
{
    public class MeteorSwarmSpell : MagerySpell, ITargetingSpell<IPoint3D>
    {
        private static readonly SpellInfo _info = new(
            "Meteor Swarm",
            "Flam Kal Des Ylem",
            233,
            9042,
            false,
            Reagent.Bloodmoss,
            Reagent.MandrakeRoot,
            Reagent.SulfurousAsh,
            Reagent.SpidersSilk
        );

        public MeteorSwarmSpell(Mobile caster, Item scroll = null) : base(caster, scroll, _info)
        {
        }

        public override SpellCircle Circle => SpellCircle.Seventh;

        public override bool DelayedDamage => true;

        public void Target(IPoint3D p)
        {
            if (SpellHelper.CheckTown(p, Caster) && CheckSequence())
            {
                SpellHelper.Turn(Caster, p);

                if (p is Item item)
                {
                    p = item.GetWorldLocation();
                }

                var map = Caster.Map;

                var playerVsPlayer = false;
                var loc = new Point3D(p);

                if (map != null)
                {
                    using var queue = PooledRefQueue<Mobile>.Create();
                    foreach (var m in map.GetMobilesInRange(loc, 2))
                    {
                        if (Caster == m || !SpellHelper.ValidIndirectTarget(Caster, m) ||
                            !Caster.CanBeHarmful(m, false) || Core.AOS && !Caster.InLOS(m))
                        {
                            continue;
                        }

                        if (m.Player)
                        {
                            playerVsPlayer = true;
                        }

                        queue.Enqueue(m);
                    }

                    double damage = Core.AOS
                        ? GetNewAosDamage(51, 1, 5, playerVsPlayer)
                        : Utility.Random(27, 22);

                    int count = queue.Count;

                    if (count > 0)
                    {
                        Effects.PlaySound(loc, Caster.Map, 0x160);

                        if (Core.AOS && count > 2)
                        {
                            damage = damage * 2 / count;
                        }
                        else if (!Core.AOS)
                        {
                            damage /= count;
                        }

                        while (queue.Count > 0)
                        {
                            var m = queue.Dequeue();

                            var toDeal = damage;

                            if (!Core.AOS && CheckResisted(m))
                            {
                                damage *= 0.5;

                                m.SendLocalizedMessage(501783); // You feel yourself resisting magical energy.
                            }

                            toDeal *= GetDamageScalar(m);
                            Caster.DoHarmful(m);
                            SpellHelper.Damage(this, m, toDeal, 0, 100, 0, 0, 0);

                            Caster.MovingParticles(m, 0x36D4, 7, 0, false, true, 9501, 1, 0, 0x100);
                        }
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
