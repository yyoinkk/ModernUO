using System;
using System.Collections.Generic;

namespace Server.Spells
{
    public static class SpellRegistry
    {
        private static readonly Type[] m_Types = new Type[1000];
        private static int m_Count;

        private static readonly Dictionary<Type, int> m_IDsFromTypes = new(m_Types.Length);

        private static readonly object[] m_Params = new object[2];

        private static readonly string[] m_CircleNames =
        {
            "First",
            "Second",
            "Third",
            "Fourth",
            "Fifth",
            "Sixth",
            "Seventh",
            "Eighth",
            "Necromancy",
            "Chivalry",
            "Bushido",
            "Ninjitsu",
            "Spellweaving"
        };

        private static readonly string[] m_darkSpell = new string[16];
        private static readonly string[] m_druidSpell = new string[16];
        private static readonly string[] m_lightSpell = new string[16];

        public static Type[] Types
        {
            get
            {
                m_Count = -1;
                return m_Types;
            }
        }

        // What IS this used for anyways.
        public static int Count
        {
            get
            {
                if (m_Count == -1)
                {
                    m_Count = 0;

                    for (var i = 0; i < m_Types.Length; ++i)
                    {
                        if (m_Types[i] != null)
                        {
                            ++m_Count;
                        }
                    }
                }

                return m_Count;
            }
        }

        public static Dictionary<int, SpecialMove> SpecialMoves { get; } = new();

        public static int GetRegistryNumber(ISpell s) => GetRegistryNumber(s.GetType());

        public static int GetRegistryNumber(SpecialMove s) => GetRegistryNumber(s.GetType());

        public static int GetRegistryNumber(Type type) => m_IDsFromTypes.TryGetValue(type, out var value) ? value : -1;

        public static void Register(int spellID, Type type)
        {
            if (spellID < 0 || spellID >= m_Types.Length)
            {
                return;
            }

            if (m_Types[spellID] == null)
            {
                ++m_Count;
            }

            m_Types[spellID] = type;

            m_IDsFromTypes.TryAdd(type, spellID);

            if (type.IsSubclassOf(typeof(SpecialMove)))
            {
                SpecialMove spm = null;

                try
                {
                    spm = type.CreateInstance<SpecialMove>();
                }
                catch
                {
                    // ignored
                }

                if (spm != null)
                {
                    SpecialMoves.Add(spellID, spm);
                }
            }

            if (700 <= spellID && spellID < 800)
            {
                m_darkSpell[spellID - 700] = type.Name.ToLower();
            }
            else if (800 <= spellID && spellID < 900)
            {
                m_druidSpell[spellID - 800] = type.Name.ToLower();
            }
            else if (900 <= spellID && spellID < 1000)
            {
                m_lightSpell[spellID - 900] = type.Name.ToLower();
            }
        }

        public static SpecialMove GetSpecialMove(int spellID)
        {
            if (spellID < 0 || spellID >= m_Types.Length)
            {
                return null;
            }

            var t = m_Types[spellID];

            if (t?.IsSubclassOf(typeof(SpecialMove)) != true)
            {
                return null;
            }

            SpecialMoves.TryGetValue(spellID, out var move);
            return move;
        }

        public static Spell NewSpell(int spellID, Mobile caster, Item scroll)
        {
            if (spellID < 0 || spellID >= m_Types.Length)
            {
                return null;
            }

            var t = m_Types[spellID];

            if (t?.IsSubclassOf(typeof(SpecialMove)) == false)
            {
                m_Params[0] = caster;
                m_Params[1] = scroll;

                try
                {
                    return t.CreateInstance<Spell>(m_Params);
                }
                catch
                {
                    // ignored
                }
            }

            return null;
        }

        public static Spell NewSpell(string name, Mobile caster, Item scroll)
        {
            name = name.RemoveOrdinal(" ");

            m_Params[0] = caster;
            m_Params[1] = scroll;

            string? color = null;

            if (Array.IndexOf(m_druidSpell, $"{name}spell") > -1)
            {
                color = "Druid";
            }
            else if (Array.IndexOf(m_darkSpell, $"{name}spell") > -1)
            {
                color = "Dark";
            }
            else if (Array.IndexOf(m_lightSpell, $"{name}spell") > -1)
            {
                color = "Light";
            }

            if (color != null)
            {
                var tt = AssemblyHandler.FindTypeByFullName($"Server.Spells.{color}.{name}Spell");
                try
                {
                    return tt.CreateInstance<Spell>(m_Params);
                }
                catch
                {
                    // ignored
                }
            }

            for (var i = 0; i < m_CircleNames.Length; ++i)
            {
                var t = name.InsensitiveEndsWith("spell") ?
                    AssemblyHandler.FindTypeByFullName($"Server.Spells.{m_CircleNames[i]}.{name}") :
                    AssemblyHandler.FindTypeByFullName($"Server.Spells.{m_CircleNames[i]}.{name}Spell");

                if (t?.IsSubclassOf(typeof(SpecialMove)) == false)
                {
                    try
                    {
                        return t.CreateInstance<Spell>(m_Params);
                    }
                    catch
                    {
                        // ignored
                    }
                }
            }

            return null;
        }
    }
}
