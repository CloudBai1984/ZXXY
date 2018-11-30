using SuperSocket.SocketBase;
using SuperWebSocket;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using WebSocket4Net;
using XYJClient.Robots;
using XYJClient.Util;

namespace XYJClient
{


    public partial class Form1 : Form
    {
        Queue<string> msgQueue = new Queue<string>();

        WebSocket4Net.WebSocket websocket;
        public Form1()
        {
            InitializeComponent();
            Global.MAIN_FORM = this;
        }

        private void btnSend_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            websocket = new WebSocket("ws://127.0.0.1:8081");
            websocket.Opened += new EventHandler(websocket_Opened);
            websocket.Error += new EventHandler<SuperSocket.ClientEngine.ErrorEventArgs>(websocket_Error);
            websocket.Closed += new EventHandler(websocket_Closed);
            websocket.MessageReceived += new EventHandler<MessageReceivedEventArgs>(websocket_MessageReceived);
            websocket.Open();
        }

        private void Websocket_Error(object sender, SuperSocket.ClientEngine.ErrorEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void websocket_Error(object sender, SuperSocket.ClientEngine.ErrorEventArgs e)
        {

            DispayMessage("与中间服务通讯出现错误:" + e.Exception.Message);

            websocket.Open();
        }

        private void websocket_Closed(object sender, EventArgs e)
        {

            DispayMessage("与中间服务连接中断");


        }

        private void websocket_MessageReceived(object sender, EventArgs e)
        {
            var e1 = (MessageReceivedEventArgs)e;

            DispayMessage(e1.Message);
            msgQueue.Enqueue(e1.Message);
            try
            {
                MessageParser.Parser(e1.Message);
            }
            catch (Exception e2)
            {
                DispayMessage("解析报文错误：" + e2.Message);
            }

        }

        private void websocket_Opened(object sender, EventArgs e)
        {


            DispayMessage("已连接中间服务");

        }

        private void tbxCMD_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13 && !string.IsNullOrEmpty(tbxCMD.Text))
            {


                websocket.Send(tbxCMD.Text);
                DispayMessage("发送模拟命令：" + tbxCMD.Text);
                tbxCMD.Text = string.Empty;
            }
        }

        private void btnJJC_Click(object sender, EventArgs e)
        {
            if (websocket.State == WebSocketState.Open)
            {

                StartRobot("JJC");

            }
        }

        private void btnDFFB_Click(object sender, EventArgs e)
        {
            if (websocket.State == WebSocketState.Open)
            {

                StartRobot("DFFB");

            }
        }

        private void btnMY_Click(object sender, EventArgs e)
        {
            if (websocket.State == WebSocketState.Open)
            {
                StartRobot("MY");

            }
        }

        private object Robot_SendQuickMessageEvent(object obj)
        {
            websocket.Send(obj.ToString());
            return null;
        }

        object locker = new object();
        private object Robot_SendMessageEvent(object obj)
        {
            lock (locker)
            {
                msgQueue.Clear();
                websocket.Send(obj.ToString());
                Thread.Sleep(1000);
                string[] msgs = new string[msgQueue.Count];
                for (int i = 0; i < msgs.Length; i++)
                {
                    msgs[i] = msgQueue.Dequeue();
                }
                return msgs;
            }
        }

        
        private object Robot_SendWaitMessageEvent(object obj, object watitime)
        {

            websocket.Send(obj.ToString());
            Thread.Sleep((int)watitime);
            return null;
        }


        void DispayMessage(string message)
        {
            new Thread(() =>
            {
                this.Invoke(new Action(() =>
                {
                    tbxContent.Text = message + "\r\n" + tbxContent.Text;
                    if (tbxContent.Text.Length > 5000)
                    {
                        string msg = tbxContent.Text;
                        tbxContent.Text = msg.Substring(0, 1000);
                    }
                }));

            }).Start();


        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            UtilFunction.ProcessMathQuestion("       5  加上   三   + 4");
        }


        Robot currRot;
        void StartRobot(string robotname)
        {

            btnDFFB.Enabled = false;
            btnJJC.Enabled = false;
            btnMY.Enabled = false;
            new Thread(() =>
            {
                Robot rbt = RobotFactory.GetRobot(robotname);
                currRot = rbt;

                websocket.MessageReceived += new EventHandler<MessageReceivedEventArgs>(rbt.websocket_MessageReceived);
                rbt.SendMessageEvent += Robot_SendMessageEvent;
                rbt.SendQuickMessageEvent += Robot_SendQuickMessageEvent;
                rbt.SendWaitMessageEvent += Robot_SendWaitMessageEvent;
                rbt.IsContinue = true;
                while (rbt.IsContinue)
                    rbt.Run();
                rbt.Run();
                websocket.MessageReceived -= new EventHandler<MessageReceivedEventArgs>(rbt.websocket_MessageReceived);
                this.Invoke(new Action(() =>
                {
                    btnDFFB.Enabled = true;
                    btnJJC.Enabled = true;
                    btnMY.Enabled = true;
                }));
            }).Start();

        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (currRot != null)
                currRot.IsContinue = false;
        }
    }
}
