/*************************************************************************
 * ModernUO                                                              *
 * Copyright 2019-2024 - ModernUO Development Team                       *
 * Email: hi@modernuo.com                                                *
 * File: Guild.cs                                                        *
 *                                                                       *
 * This program is free software: you can redistribute it and/or modify  *
 * it under the terms of the GNU General Public License as published by  *
 * the Free Software Foundation, either version 3 of the License, or     *
 * (at your option) any later version.                                   *
 *                                                                       *
 * You should have received a copy of the GNU General Public License     *
 * along with this program.  If not, see <http://www.gnu.org/licenses/>. *
 *************************************************************************/

using System;
using System.Collections.Generic;

namespace Server.Guilds;

public enum GuildType
{
    Regular,
    Chaos,
    Order
}

public abstract class BaseGuild : ISerializable
{
    protected BaseGuild()
    {
        Serial = World.NewGuild;
        World.AddGuild(this);
    }

    protected BaseGuild(Serial serial) => Serial = serial;

    public abstract string Abbreviation { get; set; }
    public abstract string Name { get; set; }
    public abstract GuildType Type { get; set; }
    public abstract bool Disbanded { get; }

    public abstract void Delete();

    public bool Deleted => Disbanded;

    [IgnoreDupe]
    [CommandProperty(AccessLevel.Counselor)]
    public Serial Serial { get; }

    [IgnoreDupe]
    [CommandProperty(AccessLevel.GameMaster, readOnly: true)]
    public DateTime Created { get; set; } = Core.Now;

    public byte SerializedThread { get; set; }
    public int SerializedPosition { get; set; }
    public int SerializedLength { get; set; }

    public abstract void Serialize(IGenericWriter writer);

    public abstract void Deserialize(IGenericReader reader);

    public abstract void OnDelete(Mobile mob);

    public static BaseGuild FindByName(string name)
    {
        foreach (var g in World.Guilds.Values)
        {
            if (g.Name.InsensitiveEquals(name))
            {
                return g;
            }
        }

        return null;
    }

    public static BaseGuild FindByAbbrev(string abbr)
    {
        foreach (var g in World.Guilds.Values)
        {
            if (g.Abbreviation.InsensitiveEquals(abbr))
            {
                return g;
            }
        }

        return null;
    }

    public static HashSet<BaseGuild> Search(string find)
    {
        var words = find.ToLower().Split(' ');
        var results = new HashSet<BaseGuild>();

        foreach (var g in World.Guilds.Values)
        {
            var name = g.Name;

            bool all = true;
            foreach (var t in words)
            {
                if (name.InsensitiveIndexOf(t) == -1)
                {
                    all = false;
                    break;
                }
            }

            if (all)
            {
                results.Add(g);
            }
        }

        return results;
    }

    public override string ToString() => $"{Serial} \"{Name} [{Abbreviation}]\"";
}
