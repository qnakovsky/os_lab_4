using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
namespace os_lab_4
{
    class Client
    {
        int num;
        Label lblMess;
        private bool haveMessage=false;
        int d = 0;
        public Client(Label mess,int number)
        {
            lblMess =mess;
            num = number;
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
                this.lblMess.Text = text;
            }
        }
    }
}
