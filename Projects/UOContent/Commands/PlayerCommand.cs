using System;
using Server.Engines.ConPVP;
using Server.Items;

namespace Server.Commands;

public static class PlayerCommand
{
    public static void Initialize()
    {
        //CommandHandlers.Register("Cap", AccessLevel.Player, CapStat_OnCommand);
        CommandHandlers.Register("Bandageself", AccessLevel.Player, BandageSelf_OnCommand);
        CommandHandlers.Register("Bandage", AccessLevel.Player, Bandage_OnCommand);

        HelpInfo.FillTable();
    }

    //[Usage("cap <str/dex/int value")]
    //[Description("Limits selected stat to choosen value. See class restrictions")]
    //public static void CapStat_OnCommand(CommandEventArgs e)
    //{
    //    var m = e.Mobile;
    //    var args = e.Arguments;
    //    if (args.Length == 2)
    //    {
    //        string stat = e.GetString(0).ToLower();
    //        int val = e.GetInt32(1);
    //        val = Math.Clamp(val, 0, 300);

    //        if (stat == "str")
    //        {
    //            m.StrCap = val;
    //        }
    //        else if (stat == "dex")
    //        {
    //            m.DexCap = val;
    //        }
    //        else if (stat == "int")
    //        {
    //            m.IntCap = val;
    //        }
    //        else
    //        {
    //            m.SendMessage("Format: cap str 123");
    //            return;
    //        }
    //        m.SendMessage($"StatCap cnanged. Its now Str:{m.StrCap} Dex:{m.DexCap} Int:{m.IntCap}");
    //    }
    //    else
    //    {
    //       m.SendMessage("Format: cap str 123");
    //    }
    //}


    [Usage("bandageself")]
    [Description("Heal self with bandages if you have any")]
    public static void BandageSelf_OnCommand(CommandEventArgs e)
    {
        Mobile m = e.Mobile;

        if (!m.CheckAlive(true))
        {
            return;
        }

        Bandage bandages = m.Backpack.FindItemByType<Bandage>(false);

        if (bandages != null)
        {
            if (!(BandageContext.BeginHeal(m, m) == null || DuelContext.IsFreeConsume(m)))
            {
                bandages.Consume();
            }
            else
            {
                m.SendLocalizedMessage(1049616); // You are too busy to do that at the moment.
            }
        }
        else
        {
            m.SendLocalizedMessage(1114309); // You must carry a stack of bandages before using the bandage macro.
        }
    }

    [Usage("bandage")]
    [Description("Macro command to use bandages")]
    public static void Bandage_OnCommand(CommandEventArgs e)
    {
        Mobile m = e.Mobile;

        if (!m.CheckAlive(true))
        {
            return;
        }

        Bandage bandages = m.Backpack.FindItemByType<Bandage>(false);

        if (bandages != null)
        {
            bandages.OnDoubleClick(m);
        }
        else
        {
            m.SendLocalizedMessage(1114309); // You must carry a stack of bandages before using the bandage macro.
        }
    }
}
