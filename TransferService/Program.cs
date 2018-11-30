using SuperSocket.SocketBase;
using SuperWebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebSocket4Net;
using SuperSocket.ClientEngine;

namespace TransferService
{
    class Program
    {
        static WebSocketSession sess;
        static WebSocketSession sess2;
        static WebSocket4Net.WebSocket websocket;
        static void Main(string[] args)
        {
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

            var appServer2 = new WebSocketServer();
            if (!appServer2.Setup(8081)) //Setup with listening port
            {
                Console.WriteLine("Failed to setup!");
                Console.ReadKey();
                return;
            }
            appServer2.NewMessageReceived += new SessionHandler<WebSocketSession, string>(appServer2_NewMessageReceived);


            if (!appServer2.Start())
            {
                Console.WriteLine("Failed to start!");
                Console.ReadKey();
                return;
            }

            while (true)
            {
                System.Threading.Thread.Sleep(2000);
            }
        }



        #region xyjclient
        private static void appServer2_NewMessageReceived(WebSocketSession session, string value)
        {
            sess2 = session;
            if (value.Length > 1)
            {
                websocket.Send(value);
                Console.WriteLine("模拟命令:" + value);
            }
                
        }
        #endregion

        #region palmmud
        static void Websocket_Error(object sender, SuperSocket.ClientEngine.ErrorEventArgs e)
        {
            throw new NotImplementedException();
        }
        static void appServer_NewMessageReceived(WebSocketSession session, string message)
        {

            try
            {
                sess = session;
                websocket.Send(message);
                Console.WriteLine("客户端发送消息" + message);
                if (sess2 != null)
                    sess2.Send("客户端发送消息" + message + "\r\n");
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }


        }
        static void websocket_Error(object sender, SuperSocket.ClientEngine.ErrorEventArgs e)
        {
            websocket = null;
            websocket = new WebSocket("ws://www.palmmud.com:8080");
            websocket.Opened += new EventHandler(websocket_Opened);
            websocket.Error += new EventHandler<SuperSocket.ClientEngine.ErrorEventArgs>(websocket_Error);
            websocket.Closed += new EventHandler(websocket_Closed);
            websocket.MessageReceived += new EventHandler<MessageReceivedEventArgs>(websocket_MessageReceived);
            websocket.Open();

        }
        static void websocket_Closed(object sender, EventArgs e)
        {
            Console.WriteLine("连接中断");
            if (sess2 != null)
                sess2.Send("连接中断\r\n");
        }
        static void websocket_MessageReceived(object sender, EventArgs e)
        {
            var e1 = (MessageReceivedEventArgs)e;
            Console.WriteLine("服务器发送消息：" + e1.Message);
            if (sess2 != null)
                sess2.Send("服务器发送消息：" + e1.Message + "\r\n");

            if (sess != null)
                sess.Send(e1.Message);

        }
        static void websocket_Opened(object sender, EventArgs e)
        {
            Console.WriteLine("已连接websocket");
            if (sess2 != null)
                sess2.Send("已连接websocket\r\n");

        }
        #endregion
    }
}
