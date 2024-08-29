namespace Server.Spells.Sixth
{
    public class MassCurseSpell : MagerySpell, ITargetingSpell<IPoint3D>
    {
        private static readonly SpellInfo _info = new(
            "Mass Curse",
            "Vas Des Sanct",
            218,
            9031,
            false,
            Reagent.Garlic,
            Reagent.Nightshade,
            Reagent.MandrakeRoot,
            Reagent.SulfurousAsh
        );

        public MassCurseSpell(Mobile caster, Item scroll = null) : base(caster, scroll, _info)
        {
        }

        public override SpellCircle Circle => SpellCircle.Sixth;

        public void Target(IPoint3D p)
        {
            if (SpellHelper.CheckTown(p, Caster) && CheckSequence())
            {
                SpellHelper.Turn(Caster, p);

                SpellHelper.GetSurfaceTop(ref p);

                var map = Caster.Map;

                if (map != null)
                {
                    foreach (var m in map.GetMobilesInRange(new Point3D(p), 2))
                    {
                        if (Core.AOS && (m == Caster || !SpellHelper.ValidIndirectTarget(Caster, m) || !Caster.CanSee(m) ||
                                         !Caster.CanBeHarmful(m, false)))
                        {
                            continue;
                        }

                        Caster.DoHarmful(m);

                        var length = SpellHelper.GetDuration(Caster, m, curse: true);
                        SpellHelper.AddStatCurse(Caster, m, StatType.Str, length, false);
                        SpellHelper.AddStatCurse(Caster, m, StatType.Dex, length);
                        SpellHelper.AddStatCurse(Caster, m, StatType.Int, length);

                        m.FixedParticles(0x374A, 10, 15, 5028, EffectLayer.Waist);
                        m.PlaySound(0x1FB);

                        HarmfulSpell(m);
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
