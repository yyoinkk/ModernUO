/*************************************************************************
 * ModernUO                                                              *
 * Copyright 2019-2024 - ModernUO Development Team                       *
 * Email: hi@modernuo.com                                                *
 * File: IEntity.cs                                                      *
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

namespace Server;

public interface IEntity : IPoint3D, ISerializable
{
    Point3D Location { get; set; }
    Map Map { get; }
    void MoveToWorld(Point3D location, Map map);

    void ProcessDelta();

    bool InRange(Point2D p, int range);

    bool InRange(Point3D p, int range);

    void RemoveItem(Item item);

    bool OnMoveOff(Mobile m);

    bool OnMoveOver(Mobile m);

    public void OnMovement(Mobile m, Point3D oldLocation);
}

public class Entity : IEntity
{
    public Entity(Serial serial, Point3D loc, Map map) : this(serial)
    {
        Location = loc;
        Map = map;
        Deleted = false;
    }

    public Entity(Serial serial) => Serial = serial;

    public DateTime Created { get; set; } = Core.Now;

    public Serial Serial { get; }

    public Point3D Location { get; set; }

    public int X => Location.X;

    public int Y => Location.Y;

    public int Z => Location.Z;

    public Map Map { get; private set; }

    public void MoveToWorld(Point3D newLocation, Map map)
    {
        Location = newLocation;
        Map = map;
    }

    public bool Deleted { get; }

    public void Delete()
    {
    }

    public void ProcessDelta()
    {
    }

    public void RemoveItem(Item item)
    {
    }

    public bool OnMoveOff(Mobile m) => true;

    public bool OnMoveOver(Mobile m) => true;

    public void OnMovement(Mobile m, Point3D oldLocation)
    {
    }

    public bool InRange(Point2D p, int range) =>
        p.m_X >= Location.m_X - range
        && p.m_X <= Location.m_X + range
        && p.m_Y >= Location.m_Y - range
        && p.m_Y <= Location.m_Y + range;

    public bool InRange(Point3D p, int range) =>
        p.m_X >= Location.m_X - range
        && p.m_X <= Location.m_X + range
        && p.m_Y >= Location.m_Y - range
        && p.m_Y <= Location.m_Y + range;

    public bool InRange(IPoint2D p, int range) =>
        p.X >= Location.m_X - range
        && p.X <= Location.m_X + range
        && p.Y >= Location.m_Y - range
        && p.Y <= Location.m_Y + range;

    public void Deserialize(IGenericReader reader)
    {
        // Should not actually be saved
        Timer.StartTimer(Delete);
    }

    public byte SerializedThread { get; set; }
    public int SerializedPosition { get; set; }
    public int SerializedLength { get; set; }

    public void Serialize(IGenericWriter writer)
    {
    }
}
