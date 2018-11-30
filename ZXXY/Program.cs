using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Net;
using WebSocket4Net;
using SuperWebSocket;
using SuperSocket.SocketBase;

namespace ZXXY
{
    public struct RECT
    {
        public int left;
        public int top;
        public int right;
        public int bottom;
    }
    class Program
    {
        static WebSocket4Net.WebSocket websocket;
        static void Main(string[] args)
        {
            //IntPtr hWnd = new IntPtr(0x00340D5A);// GetMainWindowHandle(13716);
            //Bitmap bmp = GetWindowCapture(hWnd);
            //bmp.Save("D:\\1.bmp");


            var appServer = new WebSocketServer();
            if (!appServer.Setup(8080)) //Setup with listening port
            {
                Console.WriteLine("Failed to setup!");
                Console.ReadKey();
                return;
            }
            appServer.NewMessageReceived += new SessionHandler<WebSocketSession, string>(appServer_NewMessageReceived);


            if (!appServer.Start())
            {
                Console.WriteLine("Failed to start!");
                Console.ReadKey();
                return;
            }

            websocket = new WebSocket("ws://www.palmmud.com:8080");
            websocket.Opened += new EventHandler(websocket_Opened);
            websocket.Error += new EventHandler<SuperSocket.ClientEngine.ErrorEventArgs>(websocket_Error);
            websocket.Closed += new EventHandler(websocket_Closed);
            websocket.MessageReceived += new EventHandler<MessageReceivedEventArgs>(websocket_MessageReceived);
            websocket.Open();



            while (true)
            {
                System.Threading.Thread.Sleep(2000);
            }


        }

        private static void Websocket_Error(object sender, SuperSocket.ClientEngine.ErrorEventArgs e)
        {
            throw new NotImplementedException();
        }


        static WebSocketSession sess;

        static void appServer_NewMessageReceived(WebSocketSession session, string message)
        {
            //Send the received message back
            // session.Send("^{\"p\":\"660 - 629 - 301 - 366 - 243 - 432\",\r\n\"t\":\"idverify\"\r\n}^");

            try
            {
                sess = session;
                websocket.Send(message);
                Console.WriteLine("客户端发送消息：" + message);

            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }


        }

        private static void websocket_Error(object sender, SuperSocket.ClientEngine.ErrorEventArgs e)
        {
            throw new NotImplementedException();
        }

        private static void websocket_Closed(object sender, EventArgs e)
        {
            Console.WriteLine("连接中断");
        }
        static Queue<string> sendQueue = new Queue<string>();
        static Queue<string> rcvQueue = new Queue<string>();
        private static void websocket_MessageReceived(object sender, EventArgs e)
        {
            var e1 = (MessageReceivedEventArgs)e;
            Console.WriteLine("服务器发送消息：" + e1.Message);
            if (sess != null)
                sess.Send(e1.Message);

        }

        private static void websocket_Opened(object sender, EventArgs e)
        {
            //websocket.Send("server-5");
        }

        public static Bitmap GetWindowCapture(IntPtr hWnd)
        {
            IntPtr hscrdc = GetWindowDC(hWnd);
            RECT windowRect = new RECT();
            GetWindowRect(hWnd, ref windowRect);
            int width = 429;
            int height = 726;

            IntPtr hbitmap = CreateCompatibleBitmap(hscrdc, width, height);
            IntPtr hmemdc = CreateCompatibleDC(hscrdc);
            SelectObject(hmemdc, hbitmap);
            PrintWindow(hWnd, hmemdc, 0);
            Bitmap bmp = Bitmap.FromHbitmap(hbitmap);
            DeleteDC(hscrdc);//删除用过的对象
            DeleteDC(hmemdc);//删除用过的对象
            return bmp;
        }

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowRect(IntPtr hWnd, ref RECT rect);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateDC(
         string lpszDriver,         // driver name驱动名
         string lpszDevice,         // device name设备名
         string lpszOutput,         // not used; should be NULL
         IntPtr lpInitData   // optional printer data
         );
        [DllImport("gdi32.dll")]
        public static extern int BitBlt(
         IntPtr hdcDest, // handle to destination DC目标设备的句柄
         int nXDest,   // x-coord of destination upper-left corner目标对象的左上角的X坐标
         int nYDest,   // y-coord of destination upper-left corner目标对象的左上角的Y坐标
         int nWidth,   // width of destination rectangle目标对象的矩形宽度
         int nHeight, // height of destination rectangle目标对象的矩形长度
         IntPtr hdcSrc,   // handle to source DC源设备的句柄
         int nXSrc,    // x-coordinate of source upper-left corner源对象的左上角的X坐标
         int nYSrc,    // y-coordinate of source upper-left corner源对象的左上角的Y坐标
         UInt32 dwRop   // raster operation code光栅的操作值
         );

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleDC(
         IntPtr hdc // handle to DC
         );

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleBitmap(
         IntPtr hdc,         // handle to DC
         int nWidth,      // width of bitmap, in pixels
         int nHeight      // height of bitmap, in pixels
         );

        [DllImport("gdi32.dll")]
        public static extern IntPtr SelectObject(
         IntPtr hdc,           // handle to DC
         IntPtr hgdiobj    // handle to object
         );

        [DllImport("gdi32.dll")]
        public static extern int DeleteDC(
         IntPtr hdc           // handle to DC
         );

        [DllImport("user32.dll")]
        public static extern bool PrintWindow(
         IntPtr hwnd,                // Window to copy,Handle to the window that will be copied.
         IntPtr hdcBlt,              // HDC to print into,Handle to the device context.
         UInt32 nFlags               // Optional flags,Specifies the drawing options. It can be one of the following values.
         );

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowDC(
         IntPtr hwnd
         );

        public static IntPtr GetMainWindowHandle(int processId)
        {
            IntPtr MainWindowHandle = IntPtr.Zero;

            NativeMethods.EnumWindows(new NativeMethods.EnumWindowsProc((hWnd, lParam) =>
            {
                IntPtr PID;
                NativeMethods.GetWindowThreadProcessId(hWnd, out PID);

                if (PID == lParam &&
                    NativeMethods.IsWindowVisible(hWnd) &&
                    NativeMethods.GetWindow(hWnd, NativeMethods.GW_OWNER) == IntPtr.Zero)
                {
                    MainWindowHandle = hWnd;
                    return false;
                }

                return true;

            }), new IntPtr(processId));

            return MainWindowHandle;
        }

        private static class NativeMethods
        {
            internal const uint GW_OWNER = 4;

            internal delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

            [DllImport("User32.dll", CharSet = CharSet.Auto)]
            internal static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

            [DllImport("User32.dll", CharSet = CharSet.Auto)]
            internal static extern int GetWindowThreadProcessId(IntPtr hWnd, out IntPtr lpdwProcessId);

            [DllImport("User32.dll", CharSet = CharSet.Auto)]
            internal static extern IntPtr GetWindow(IntPtr hWnd, uint uCmd);

            [DllImport("User32.dll", CharSet = CharSet.Auto)]
            internal static extern bool IsWindowVisible(IntPtr hWnd);
        }
    }
}
