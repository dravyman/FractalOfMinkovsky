using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        int speed;
        List<Point> myPoints = new List<Point>();
        bool isGo = false;
        int depth = 0;
        Thread t;
        public Form1()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (t == null || !t.IsAlive)
            {
                t = new Thread(changePoints);
                t.Start();
                depth++;
                label1.Text = isGo + "   " + depth;
            }
            else if(t.ThreadState == ThreadState.Suspended)
                t.Resume();
        }
         
        private void changePoints()
        {
            for (int i = 0; i < myPoints.Count - 1;)
            {
                Point a1 = myPoints[i];
                Point a2 = myPoints[i + 1];
                List<Point> clearList = new List<Point>();
                clearList.Add(a1);
                clearList.Add(a2);
                int sizeX = (a2.X - a1.X) / 4;
                int sizeY = (a2.Y - a1.Y) / 4;
                if (sizeY == 0)
                {
                    myPoints.Insert(i+1,new Point(a1.X + sizeX, a1.Y));
                    myPoints.Insert(i+2,new Point(a1.X + sizeX, a1.Y - sizeX));
                    myPoints.Insert(i+3,new Point(a1.X + 2 * sizeX, a1.Y - sizeX));
                    myPoints.Insert(i+4,new Point(a1.X + 2 * sizeX, a1.Y));
                    myPoints.Insert(i + 5, new Point(a1.X + 2 * sizeX, a1.Y + sizeX));
                    myPoints.Insert(i + 6, new Point(a1.X + 3 * sizeX, a1.Y + sizeX));
                    myPoints.Insert(i+7,new Point(a1.X + 3 * sizeX, a1.Y));
                }
                else
                {
                    myPoints.Insert(i + 1, new Point(a1.X, a1.Y + sizeY));
                    myPoints.Insert(i + 2, new Point(a1.X + sizeY, a1.Y + sizeY));
                    myPoints.Insert(i + 3, new Point(a1.X + sizeY, a1.Y + 2 * sizeY));
                    myPoints.Insert(i + 4, new Point(a1.X, a1.Y + 2 * sizeY));
                    myPoints.Insert(i + 5, new Point(a1.X - sizeY, a1.Y + 2 * sizeY));
                    myPoints.Insert(i + 6, new Point(a1.X - sizeY, a1.Y + 3 * sizeY));
                    myPoints.Insert(i + 7, new Point(a1.X, a1.Y + 3 * sizeY));
                }
                drawFrac(clearList,0,1,Pens.White);
                drawFrac(myPoints, i, i+8);
                i += 8;
                pictureBox1.Invalidate();
                Thread.Sleep(speed);
                
            
            }
        }

        private void drawFrac(List<Point> myPoints,int start, int stop, Pen pen = null)
        {
            if (pen == null)
                pen = Pens.Red;
            Image img = pictureBox1.Image;
            Graphics gr = Graphics.FromImage(img);

            for (int i = start; i < stop; i++)
            {
                gr.DrawLine(pen, myPoints[i], myPoints[i + 1]);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (isGo)
            {
                isGo = false;
                timer1.Stop();
                if (t.ThreadState == ThreadState.Running || t.ThreadState == ThreadState.WaitSleepJoin)
                    t.Suspend();
            }
            else
            {
                isGo = true;
                timer1.Start();
                //MessageBox.Show(Thread.CurrentThread.ManagedThreadId.ToString());
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            label1.Text = isGo + "   " + depth;
            myPoints.Add(new Point(100, pictureBox1.ClientSize.Height / 2));
            myPoints.Add(new Point(pictureBox1.ClientSize.Width - 100, pictureBox1.ClientSize.Height / 2));
            pictureBox1.Image = new Bitmap(pictureBox1.ClientSize.Width, pictureBox1.ClientSize.Height);
            drawFrac(myPoints,0,1);
            t = new Thread(changePoints);
            speed = trackBar1.Maximum;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            isGo = false;
            pictureBox1.Image = new Bitmap(pictureBox1.ClientSize.Width, pictureBox1.ClientSize.Height);
            myPoints.Clear();
            myPoints.Add(new Point(100, pictureBox1.ClientSize.Height / 2));
            myPoints.Add(new Point(pictureBox1.ClientSize.Width - 100, pictureBox1.ClientSize.Height / 2));
            depth = 0;
            isGo = false;
            drawFrac(myPoints,0,1);
            label1.Text = isGo + "   " + depth;
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            speed = trackBar1.Maximum - trackBar1.Value; 
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (t != null && t.ThreadState == ThreadState.Suspended)
            {
                t.Resume();
            }
            t.Abort();


        }
    }
}
