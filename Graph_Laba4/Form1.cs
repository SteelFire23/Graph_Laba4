using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Graph_Laba4
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Graphics g = pictureBox1.CreateGraphics();
            g.Clear(Color.White);
            printMain(g, pictureBox1.Width, pictureBox1.Height);
        }
        public static void printMain(Graphics g, float Width, float Height)
        {
            int[] temp = new int[31] { 5, 10, 12, 15, -2, -6, -10, 2, 8, 20, 21, 18, 10, 11, 4, -6, 3, 5, 9,
                                       0, -1, -7, 0, 5, 7, 8, 6, 12, 16, 20, 20 };
            Pen pen = new Pen(Color.Black, 1);
            Font font = new Font("Arial", 7, FontStyle.Regular);

            float axisY = (Height - 1) - ((Height - 1) / 3),
                  offsetY = ((Height - 1) / 3) / 4,
                  axisX = (Width - 1) / 12,
                  offsetX = (Width - 1) / 35;
            int tempPlus = 35,
                tempMinus = -5,
                day = 1;
            //Рисуем основу графика
            //
            g.DrawRectangle(pen, 0, 0, Width - 1, Height - 1);
            g.DrawLine(pen, axisX, 0, axisX, Height - 1);
            g.DrawLine(pen, 0, axisY, Width - 1, axisY);
            //
            //Рисуем информацию об оси температур 
            //
            g.DrawString("t,°C", font, Brushes.Black, ((Width - 1) / 12) - 30, 0);
            for (float i = 0; i < Height - 1; i += offsetY)
            {
                if (i != axisY)
                {
                    g.DrawLine(pen, ((Width - 1) / 12) - 5, i, ((Width - 1) / 12) + 5, i);
                    if (i < axisY)
                    {
                        if (i != 0 && tempPlus >= 5)
                        {
                            g.DrawString(tempPlus.ToString(), font, Brushes.Black, ((Width - 1) / 12) - 30, i - 5);
                            tempPlus -= 5;
                        }
                    }
                    else
                    {
                        g.DrawString(tempMinus.ToString(), font, Brushes.Black, ((Width - 1) / 12) - 30, i - 5);
                        tempMinus -= 5;
                    }
                }               
            }
            //
            //Рисуем информацию об оси дней
            //
            g.DrawString("Day", font, Brushes.Black, Width - 25, axisY);
            for (float i = axisX; i < Width - 1; i += offsetX)
            {
                if (day <= 31)
                {
                    g.DrawLine(pen, i, axisY - 5, i, axisY + 5);
                    if (i > axisX)
                    {
                        g.DrawString(day.ToString(), font, Brushes.Black, i - 5, axisY + 10);
                        day++;
                    }
                }  
            }
            //
            //Рисуем сам график 
            //
            Pen dashPen = new Pen(Color.Black, 5);
            dashPen.DashStyle = DashStyle.Dash;

            Pen compPen = new Pen(Color.Black, 3);
            compPen.CompoundArray = new float[] { 0.0f, 0.2f, 0.3f, 0.6f };

            pen.SetLineCap(LineCap.Round, LineCap.Round, DashCap.Triangle);
            compPen.SetLineCap(LineCap.Round, LineCap.Round, DashCap.Triangle);
            dashPen.SetLineCap(LineCap.Round, LineCap.Round, DashCap.Triangle);

            float stepX = axisX,
                  stepY = axisY;
            PointF[] point = new PointF[2];
            for (int i = 0; i < temp.Length; i++)
            {
                point[0].X = stepX;
                point[0].Y = stepY;
                point[1].X = stepX + offsetX;
                point[1].Y = correctPointY(offsetY, temp[i]);

                stepY = point[1].Y;
                stepX += offsetX;
                if (temp[i] < 15 && temp[i] > 0)
                {
                    g.DrawLines(compPen, point);
                }
                if (temp[i] < 0)
                {
                    pen.Width = 1;
                    g.DrawLines(pen, point);
                }
                if (temp[i] > 15)
                {
                    pen.Width = 5;
                    g.DrawLines(pen, point);
                }
                if (temp[i] < -5)
                {
                    g.DrawLines(dashPen, point);
                }
                g.DrawLines(pen, point);
            }           
            //
            g.Dispose();
        }
        public static float correctPointY(float offset,int num)
        {
            int rem = 0,//остаток от деления 
                steps = 8;//число делений до 0 на графике
            if (num == 0)
            {
                return offset * steps;
            }
            if (num > 0)
            {
                for (int i = 0; i < 35; i += 5)
                {
                    if (num == i)
                    {
                        return offset * steps;
                    }
                    if (num > 0 && num < 5)
                    {
                        rem = num;
                        return (offset * steps) - ((int)(offset / 5) * rem);
                    }
                    if (num > i && num < (i + 5))
                    {
                        rem = remain(num, i);
                        return (offset * steps) - ((int)(offset / 5) * rem);
                    }
                    else
                    {
                        steps--;
                    }
                }
            }
            else
            {
                for (int i = 0; i >= -15; i -= 5)
                {
                    if (num == i)
                    {
                        return offset * steps;
                    }
                    if (num > -5 && num < 0)
                    {
                        rem = -num;
                        return (offset * steps) + ((int)(offset / 5) * rem);
                    }
                    if (num < i && num > i - 5)
                    {
                        rem = remain(Math.Abs(num), Math.Abs(i));
                        return (offset * steps) + ((int)(offset / 5) * rem);
                    }
                    else
                    {
                        steps++;
                    }
                }
            }           
            return 0;
        }
        public static int remain(int number, int num)
        {
            return number % num;
        }
    }
}
