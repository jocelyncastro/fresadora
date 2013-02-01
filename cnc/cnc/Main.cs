using System;
using System.Reflection;
using System.IO.Ports;

namespace cnc
{

	public delegate void CNCEventHandler(object sender, CNCEventArgs e);

	public class CNCEventArgs : EventArgs
	{
		public bool avanzar;
        public string axis;
		public CNCEventArgs (bool avanzar, string axis)
		{
			this.avanzar = avanzar;
            this.axis = axis;
		}

	}

	public class Main
	{
        AxisXY axisXY;
        AxisZ axisZ;
        Taladro taladro;
        MethodInfo[] metodos;

        static SerialPort port;
        public static byte data;

        public Main (SerialPort port, AxisXY axisXY, AxisZ axisZ, Taladro taladro)
		{
            Type miTipo = typeof(Main);
            Main.port = port;
            metodos = miTipo.GetMethods();
            data = 0;
            taladro = new Taladro();
            this.axisXY = axisXY;
            this.axisZ = axisZ;

		}

        static public void Refresh()
        {
            port.Write(new byte[]{data},0,1);      
        }

        public void Handle(string comando)
        {
            string[] parametros;
            string[] comandos;
            try
            {
                comando = comando.Trim();
                comandos = comando.Split(' ');
                Console.WriteLine("Cantidad de comandos: "+comandos.Length);

                if (comandos.Length > 1)
                {
                    parametros = new string[comandos.Length - 1];
                    for (int i = 0; i < comandos.Length-1; i++)
                    {
                        parametros[i] = comandos[i + 1].Trim().Replace('.',',');
                        Console.WriteLine("Asignando "+ parametros[i] +" a parametro: "+i);
                    }
                }
                else
                    parametros = null;

                foreach (MethodInfo metodo in metodos)
                    if (metodo.Name == comandos[0])
                    {
                        try
                        {
                            if (metodo.GetParameters().Length == parametros.Length)
                                metodo.Invoke(this, parametros);
                        }
                        catch
                        {
                            metodo.Invoke(this, parametros);
                        }
                    }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }

        }

        public void setUnit(string unit)
        {
            axisXY.setUnit(unit);
            axisZ.setUnit(unit);
        }

        public void setDistanceMode(string distanceMode)
        {
            axisXY.setDistanceMode(distanceMode);
            axisZ.setDistanceMode(distanceMode);
        }

		public void G21()
		{
           setUnit("millimeters");
		}
        public void G20()
		{
            setUnit("inches");
		}
        public void G90()
		{
            setDistanceMode("absolute");
		}
        public void G91()
		{
            setDistanceMode("incremental");
		}

        //funca
        public void G00(string xString, string yString)
		{            
            float x = float.Parse(xString.Remove(0, 1));
            float y = float.Parse(yString.Remove(0, 1));

			axisXY.FastMove(x,y);
		}
        //no funca
        public void G00(string zString)
		{
            float z = float.Parse(zString.Remove(0, 1));

            axisZ.FastMove(z);
		}
        public void M03()
		{
            taladro.StartClockWise();      
		}
        public void M04()
		{
            taladro.StartCounterClockWise();
        }
        public void M05()
		{
            taladro.Stop();
        }
        public void G04(string seconds)
		{
            seconds = seconds.Remove(0, 1);
            double segundos = double.Parse(seconds);
            Console.WriteLine("Segundos:"+segundos);
            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(segundos));
        }

        public void G01(string xz, string yf)
        {
            if (xz[0] == 'X')
            {
                Console.WriteLine("Float x:" + xz);
                Console.WriteLine("Float y:" + yf);
                axisXY.Move(float.Parse(xz.Remove(0, 1)), float.Parse(yf.Remove(0, 1)));
            }
            else
            {
                Console.WriteLine("Float z:" + xz);
                Console.WriteLine("Float f:" + yf);
                axisZ.Move(float.Parse(xz.Remove(0, 1)), float.Parse(yf.Remove(0, 1)));
            }

        }

        //funca
        public void G01(string xString, string yString, string fString)
		{
            float x = float.Parse(xString.Remove(0, 1));
            float y = float.Parse(yString.Remove(0, 1));
            float f = float.Parse(fString.Remove(0, 1));

            Console.WriteLine("Float x: " + x);

            axisXY.Move(x, y, f);
        }

        public void G82(string x, string y, string z, string f, string r, string p)
        { 
            G01(z,f);
            G01(x,y);
        }

        public void G82(string x, string y)
        {
            G01(x,y);
        }

        public void M02()
		{
            
        }
	}
}

