using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cnc;
using System.IO;
using ConsoleCNC.Properties;
using System.IO.Ports;

namespace ConsoleCNC
{
    class Program
    {
        Main fresadora;
        Axis x, y, z;
        AxisXY axisXY;
        AxisZ axisZ;
        Taladro taladro;
        SerialPort port;
        string[] archivo;
        int index;

        public Program()
        {
            x = new Axis(Settings.Default.axisXStepsPercm,
                new byte[]{
                    Settings.Default.axisXStep1,
                    Settings.Default.axisXStep2,
                    Settings.Default.axisXStep3,
                    Settings.Default.axisXStep4
                },
                Settings.Default.axisXMask,
                "x");

            y = new Axis(Settings.Default.axisYStepsPercm,
                new byte[]{
                    Settings.Default.axisYStep1,
                    Settings.Default.axisYStep2,
                    Settings.Default.axisYStep3,
                    Settings.Default.axisYStep4
                },
                Settings.Default.axisYMask,
                "y");

            z = new Axis(Settings.Default.axisZStepsPercm,
                new byte[]{
                    Settings.Default.axisZStep1,
                    Settings.Default.axisZStep2,
                    Settings.Default.axisZStep3,
                    Settings.Default.axisZStep4
                },
                Settings.Default.axisZMask,
                "z");
            port = new SerialPort(
                Settings.Default.portName,
                Settings.Default.baudRate);
            axisXY = new AxisXY(x, y, Settings.Default.xyMasFeedRate);
            axisZ = new AxisZ(z, Settings.Default.zMasFeedRate);
            taladro = new Taladro();

            fresadora = new Main(port,axisXY,axisZ,taladro);
            index = 0;
        }

        void HandlemakeStep (object sender, CNCEventArgs e)
        {
			Console.WriteLine("Se hizo un paso");
        }

        static void Main(string[] args)
        {
            Program programa = new Program();
            Console.WriteLine("Ingresar ruta del archivo");
            programa.AbrirArchivo(Console.ReadLine());
            Console.WriteLine("Ejecutar comando a comando el gcode, apreta cualquier tecla apra avanzar");
            while (!programa.EOF())
            {
                Console.ReadKey();
                programa.HandleNextLine();
            }
        }

        public bool EOF()
        {
            if (index < archivo.Length)
                return false;
            else
                return true;
        }

        public void AbrirArchivo(string rutaDelArchivo)
        {
            archivo = File.ReadAllLines(rutaDelArchivo);
        }

        public void HandleNextLine()
        {
            fresadora.Handle(archivo[index]);
            Console.WriteLine(archivo[index++]);
        }

        public void HandleAllLines()
        {
            for (; index < archivo.Length; index++)
            {
                fresadora.Handle(archivo[index]);
            }
        }
    }
}
