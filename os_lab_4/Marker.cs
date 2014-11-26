using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace os_lab_4
{
    enum MarkerState
    {
        Free,Captured
    }
    class Marker
    {
        int addresFrom;
        int addresDest;
        TextBox message;
        Label l;
        MarkerState mState;        
        public Marker(TextBox _message,Label lblMess)
        {
            message = _message;
            mState = MarkerState.Free;
            l = lblMess;
        }
        public int AddresSource 
        {   get{return addresFrom;}
            set { addresFrom = value; }
        }
        public int Destination
        {
            get { return addresDest; }
            set { addresDest = value; }
        }
        public MarkerState State
        {
            get { return mState; }
            set { mState = value; }
        }
        public string Message
        {
            get { return addresFrom + ": HI! " + addresDest; }
        }
        public void printMessage()
        {
            message.Text += Message + Environment.NewLine;
        }
        public void printState(string r)
        {
            l.Text = r;
        }
         public override string ToString()
        {
            if (mState == MarkerState.Free)
                return "Маркер свободен!";
            return "Маркер несет сообщение: " + Message;
        }
    }
}
