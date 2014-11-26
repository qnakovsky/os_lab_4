using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace os_lab_4
{
    public partial class Form1 : Form
    {
        private Point pole;//центр окружности
        private Point center;//центр маркера
        private System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();//таймер
        private Pen pen = new Pen(Color.Black, 2);//Перо для рисования
        private float rMarker = 10f;//Диаметр маркера
        SolidBrush sb = new SolidBrush(Color.Blue);
        private float phi;//угол поворота (в радианах)
        int radius = 100;//радиус окружности

        int size = 6;
        Client[] clients;
        Marker marker;
        int indexClient;
        Label[] lbls;
        public Form1()
        {
            InitializeComponent();
            pole = new Point(this.ClientRectangle.Width / 2, this.ClientRectangle.Height / 2);
            timer.Tick += new EventHandler(timer_tick);
            this.Paint += new PaintEventHandler(Form1_Paint);
            timer.Interval = 1000;         
            phi =(float) -Math.PI / 3;
            center = new Point( (int)(radius * Math.Cos(phi))+pole.X, (int)(radius * Math.Sin(phi))+pole.Y);
            clients = new Client[size];            
            indexClient = 0;                  
            marker = new Marker(txbxMarker,lblMarkerState);
            lbls = new Label[] { label1, label2, label3, label4, label5, label6 };
            for (int i = 0; i < lbls.Length; i++)
            {
                clients[i] = new Client(lbls[i], i + 1);
            }
        }

        private void timer_tick(object sender, EventArgs e)
        {
            center = new Point();
            center.X = (int)(radius * Math.Cos(phi))+pole.X;
            center.Y = (int)(radius * Math.Sin(phi))+pole.Y;            
            clients[indexClient].waitMarker(marker);
            if (marker.State == MarkerState.Captured)
                sb.Color = Color.Red;
            else
                sb.Color = Color.Blue;          
            Thread t = new Thread(generate);
            t.Start();
            indexClient++;
            indexClient %= size;            
            this.Refresh();
            phi+=(float)Math.PI/3f;              
        }
        public void generate()
        {
            Random r = new Random();           
            int t = r.Next(6);
            if(!clients[t].HaveMessage)
	        {
                clients[t].generateMessage();
	        }
            
        }
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (timer.Enabled)
                using (Graphics g = e.Graphics)
                {  
                    //рисование окружности в заданных координатах
                    g.DrawEllipse(pen, pole.X - radius, pole.Y - radius, 2 * radius, 2 * radius);
                    g.FillEllipse(sb, center.X - rMarker, center.Y - rMarker, 2 * rMarker, 2 * rMarker);                                    
                }
        }

        private void btnGoStop_Click(object sender, EventArgs e)
        {
            timer.Enabled =!timer.Enabled;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            int x = 0;
            int y = 0;
            float phi = -(float)Math.PI / 3;
            for (int i = 0; i < 6; i++)
            {
                x = (int)((radius + 40) * Math.Cos(phi)) + pole.X;
                y = (int)((radius + 40) * Math.Sin(phi)) + pole.Y;
                lbls[i].Location = new Point(x-label1.Width/2, y-label1.Height/2);                
                phi += (float)Math.PI / 3;
            }
            
        }
    }
}
