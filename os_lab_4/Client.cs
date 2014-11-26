using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
namespace os_lab_4
{
    class Client
    {
        int num;
        Label lblMess;
        private bool haveMessage=false;
        int d = 0;
        Thread myThread;
        ProgressBar pb;      

        public Client(Label mess,ProgressBar _p, int number)
        {
            lblMess =mess;
            num = number;
            pb=_p;
        }
        public bool HaveMessage
        {
            get { return haveMessage; }
        }
        private int getdest() 
        {
            Random r = new Random();
            int res = r.Next(6)+1;
            while (res==num)
            {
                res = r.Next(6) + 1; 
            }
            return res;
        }
        public void generateMessage()
        {                    
          d = getdest();
          SetText(num.ToString() + Environment.NewLine + "to " + d.ToString());
          haveMessage = true;                                
        }

        public void run(int time)
        {   
            pb.Value = 0;
            pb.Step = 100 / time;
            myThread = new Thread(start);
            myThread.Start(time);            
        }
        
        private void start(object time)
        {            
            for (int i = 1; i < (int)time; i++)
            {
                SetValue(i * pb.Step);
                Thread.Sleep(1000);
            }
            SetValue(100);            
            generateMessage();
        }
        public void waitMarker(Marker m)
        {
            if (m.State == MarkerState.Free)
            {
                if (haveMessage)
                {
                    m.State = MarkerState.Captured;
                    m.AddresSource = num;
                    m.Destination = d;
                    m.printState("Маркер несет сообщение:" + m.Message);
                }
                else
                   m.printState("Маркер свободен");
            }
            else
            {
                if (m.AddresSource == num) //если маркер вернулся
                {
                    m.State = MarkerState.Free;
                    lblMess.Text = num.ToString();
                    m.printState("Маркер свободен");
                    Random r = new Random();
                    int time = r.Next(7, 20);                    
                    run(time);
                }
                else
                    if (m.Destination == num)//если доставил
                    {
                        m.printMessage();
                        m.printState("Маркер доставил сообщение");
                    }
            }
        }

        delegate void SetTextCallback(string text);
        private void SetText(string text)
        {            
            if (lblMess.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                lblMess.Invoke(d, new object[] { text });
            }
            else
            {
                lblMess.Text = text;
            }
        }
        delegate void SetValueProgres(int value);
        private void SetValue(int value)
        {
            if (pb.InvokeRequired)
            {
                SetValueProgres d = new SetValueProgres(SetValue);
                pb.Invoke(d, new object[] { value });
            }
            else
            {
                pb.Value = value;
            }
        }
    }
}
