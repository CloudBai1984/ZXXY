using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using XYJClient.Model;


public delegate object GeneralDelegate(object obj);
public delegate object GeneralDelegate2(object obj1, object obje2);

public class Global
{
    static Global()
    {
        USER_ID = "/obj/user#10712327";
        new Thread(() =>
        {
            Thread.Sleep(1000);
            if (BUSY_TIME >= 0)
                BUSY_TIME = BUSY_TIME - 1;
            if (BUSY_TIME < 0) BUSY_TIME = 0;
        }).Start();
    }
    internal static List<Inv> INV_LIST;
    internal static List<MyPal> MYPAL_LIST;
    internal static Room CURRENT_ROOM;
    internal static Room LAST_ROOM;
    internal static int BUSY_TIME;
    internal static string USER_ID;
    internal static Form MAIN_FORM;




}

