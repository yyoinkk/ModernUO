using Server.Regions;
using Server.Targeting;

namespace Server.Spells.Druid
{
    public class GustSpell : DruidSpell, ITargetingSpell<Mobile>
    {
        private static readonly SpellInfo _info = new(
            "Gust",
            "In Hur Por",
            203,
            9051,
            Reagent.SpidersSilk,
            Reagent.Bloodmoss
        );

        public GustSpell(Mobile caster, Item scroll) : base(caster, scroll, _info)
        {
        }

        public override SpellCircle Circle => SpellCircle.Fifth;

        public override void OnCast()
        {
            Caster.Target = new SpellTarget<Mobile>(this, TargetFlags.Harmful);
        }

        public void Target(Mobile m)
        {
            if (CheckHSequence(m))
            {
                var dir = Caster.GetDirectionTo(m);
                var oppositeDir = m.GetDirectionTo(Caster);
                SpellHelper.Turn(Caster, m);

                SpellHelper.CheckReflect((int)Circle, Caster, ref m);

                double damage = GetNewAosDamage(23, 1, 4, m);

                m.FixedParticles(0x37CC, 10, 30, 5052, EffectLayer.LeftFoot);
                m.PlaySound(0x212);

                var direction = Caster == m ? oppositeDir : dir;
                m.Location = GetNewPoint(m, direction);

                SpellHelper.Damage(this, m, damage, 0, 50, 0, 0, 50);
            }
        }

        private Point3D GetNewPoint(Mobile m, Direction dir)
        {
            int range = 3;

            var oldLoc = m.Location;
            var newLoc = m.Location;

            for (int i = 1; i <= range; i++)
            {
                var newValidLoc = oldLoc;
                Movement.Movement.Offset(dir, ref newValidLoc, i);
                if (IsValidLocation(newValidLoc, m))
                {
                    newLoc = newValidLoc;
                }
                else
                {
                    return newLoc;
                }
            }

            return newLoc;
        }

        private bool IsValidLocation(Point3D loc, Mobile m)
        {
            Map map = m.Map;
            if (map == null)
            {
                return false;
            }
            else if (!map.CanSpawnMobile(loc.X, loc.Y, loc.Z, checkMobiles: false))
            {
                return false;
            }
            else if (SpellHelper.CheckMulti(loc, map))
            {
                return false;
            }
            else if (Region.Find(loc, map).IsPartOf<HouseRegion>())
            {
                return false;
            }

            return true;
        }
    }
}
