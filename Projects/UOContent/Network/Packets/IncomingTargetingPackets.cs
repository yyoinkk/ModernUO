/*************************************************************************
 * ModernUO                                                              *
 * Copyright 2019-2023 - ModernUO Development Team                       *
 * Email: hi@modernuo.com                                                *
 * File: IncomingTargetingPackets.cs                                     *
 *                                                                       *
 * This program is free software: you can redistribute it and/or modify  *
 * it under the terms of the GNU General Public License as published by  *
 * the Free Software Foundation, either version 3 of the License, or     *
 * (at your option) any later version.                                   *
 *                                                                       *
 * You should have received a copy of the GNU General Public License     *
 * along with this program.  If not, see <http://www.gnu.org/licenses/>. *
 *************************************************************************/

using System.Buffers;
using Server.Targeting;

namespace Server.Network;

public static class IncomingTargetingPackets
{
    public static unsafe void Configure()
    {
        IncomingPackets.Register(0x6C, 19, true, &TargetResponse);
    }

    public static void TargetResponse(NetState state, SpanReader reader)
    {
        int type = reader.ReadByte();
        var targetID = reader.ReadInt32();
        int flags = reader.ReadByte();
        var serial = (Serial)reader.ReadUInt32();
        int x = reader.ReadInt16();
        int y = reader.ReadInt16();
        reader.ReadByte();
        int z = reader.ReadSByte();
        int graphic = reader.ReadUInt16();

        if (targetID == unchecked((int)0xDEADBEEF))
        {
            return;
        }

        var from = state.Mobile;

        var t = from.Target;

        if (t == null)
        {
            return;
        }

        if (x == -1 && y == -1 && !serial.IsValid)
        {
            // User pressed escape
            t.Cancel(from, TargetCancelType.Canceled);
        }
        else if (t.TargetID == targetID)
        {
            object toTarget;

            if (type == 1)
            {
                if (graphic == 0)
                {
                    toTarget = new LandTarget(new Point3D(x, y, z), from.Map);
                }
                else
                {
                    var map = from.Map;

                    if (map == null || map == Map.Internal)
                    {
                        t.Cancel(from, TargetCancelType.Canceled);
                        return;
                    }
                    else
                    {
                        if (state.HighSeas)
                        {
                            var id = TileData.ItemTable[graphic & TileData.MaxItemValue];
                            if (id.Surface)
                            {
                                z -= id.Height;
                            }
                        }

                        int hue = 0;
                        var valid = false;

                        var eable = t.DisallowMultis
                            ? map.Tiles.GetStaticTiles(x, y)
                            : map.Tiles.GetStaticAndMultiTiles(x, y);

                        foreach (var tile in eable)
                        {
                            if (tile.Z == z && tile.ID == graphic)
                            {
                                valid = true;
                                hue = tile.Hue;
                                break;
                            }
                        }

                        if (!valid)
                        {
                            t.Cancel(from, TargetCancelType.Canceled);
                            return;
                        }

                        toTarget = new StaticTarget(new Point3D(x, y, z), graphic, hue);
                    }
                }
            }
            else if (serial.IsMobile)
            {
                toTarget = World.FindMobile(serial);
            }
            else if (serial.IsItem)
            {
                toTarget = World.FindItem(serial);
            }
            else
            {
                t.Cancel(from, TargetCancelType.Canceled);
                return;
            }

            t.Invoke(from, toTarget);
        }
    }
}
