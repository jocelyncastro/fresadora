using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using cnc;
using GraphicCNC.Properties;

namespace GraphicCNC
{
    public partial class Form1 : Form
    {
        string[] archivo;
        int index;
        Main fresadora;
        Graphics g;
        Pen p;
        int z;
        float x, y;
        public Form1()
        {
            InitializeComponent();
            index = 0;
            fresadora = new Main(
                Settings.Default.portName,
                Settings.Default.baudRate,
                Settings.Default.axisXStepsPercm,
                Settings.Default.axisYStepsPercm,
                Settings.Default.axisZStepsPercm,
                Settings.Default.xyMaxFeedRate,
                Settings.Default.zMaxFeedRate
                );
            fresadora.makeStep += new CNCEventHandler(HandlemakeStep);
            g = cboxFresado.CreateGraphics();
            z = 200;
            x = y = 0;
        }

        void HandlemakeStep(object sender, CNCEventArgs e)
        { 
            /*switch(e.axis)
            {
                case "x":
                    if (e.avanzar)
                        Draw(z, x+1, y);
                    else
                        Draw(z, x-1, y);
                    break;
                case "y":
                    if (e.avanzar)
                        Draw(z, x, y+1);
                    else
                        Draw(z, x, y-1);
                    break;
                case "z":
                    if (e.avanzar)
                        Draw(z + 1, x, y);
                    else
                        Draw(z - 1, x, y);
                    break;
            }*/
        }

        void Draw(int axisZ, float axisX, float axisY)
        {
            p = new Pen(Color.FromArgb(axisZ, Color.Black));
            g.DrawLine(p, x, y, axisX, axisY);
            x = axisX;
            y = axisY;
            z = axisZ;
        }

        private void btnFile_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                archivo = File.ReadAllLines(openFileDialog.FileName);
            }
        }

        void DrawLine(string xy)
        {
            string[] comandos = xy.Split(' ');
            float axisX = 0;
            float axisY = 0;
            string temp;
            try
            {
                foreach (string comando in comandos)
                {
                    temp = comando.Trim();
                    temp = temp.Replace('.', ',');
                    if (temp.Length > 1)
                    {
                        if (temp[0] == 'X')
                            axisX = float.Parse(temp.Remove(0, 1));
                        if (temp[0] == 'Y')
                            axisY = float.Parse(temp.Remove(0, 1));
                    }
                }

                if (axisX != 0 || axisY != 0)
                    Draw(z, axisX*10, axisY*10);
            }
            catch { }
        }

        private void btnStep_Click(object sender, EventArgs e)
        {
            lblComando.Text = archivo[index];
            DrawLine(archivo[index]);
            fresadora.Handle(archivo[index++]);
            //lblComando.Text = "Termino de procesar";
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            while(index < archivo.Length)
            {
                btnStep_Click(this, null);
            }
        }
    }
}
