using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cnc;
using System.IO;
using ConsoleCNC.Properties;

namespace ConsoleCNC
{
    class Program
    {
        Main fresadora;
        string[] archivo;
        int index;

        public Program()
        {
            fresadora = new Main(
                Settings.Default.portName,
                Settings.Default.baudRate,
                Settings.Default.axisXStepsPercm,
                Settings.Default.axisYStepsPercm,
                Settings.Default.axisZStepsPercm,
                Settings.Default.xyMasFeedRate,
                Settings.Default.zMasFeedRate
                );
            index = 0;
			fresadora.makeStep += HandlemakeStep;
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
