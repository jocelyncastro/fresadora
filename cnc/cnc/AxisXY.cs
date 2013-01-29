using System;

namespace cnc
{
	public class AxisXY
	{
		float feedRate;
		public Axis axisX, axisY;
		string distanceMode;
		public event CNCEventHandler makeStep;
        DateTime time;

		public AxisXY (string distanceMode, string unit, int axisXStepsPercm, int axisYStepsPercm)
		{
            this.distanceMode = distanceMode;
			axisX = new Axis(axisXStepsPercm, new byte[]{0,1,2,3},252, unit,"x");
			axisY = new Axis(axisYStepsPercm, new byte[]{0,4,8,12},243, unit,"y");
            Console.WriteLine("LLega hasta aca");
            axisX.makeStep += new CNCEventHandler(HandlemakeStep);
            axisY.makeStep += new CNCEventHandler(HandlemakeStep);
            Console.WriteLine("LLega hasta aca");
		}

		void HandlemakeStep (object sender, CNCEventArgs e)
		{
			Console.WriteLine("XY noto step");
			makeStep(sender,e);
		}

		public void Move (float x, float y, float f)
		{
			float diagonal = 0;
            feedRate = f;
			if (distanceMode == "absolute") 
			{
				diagonal = (float)Math.Sqrt (Math.Pow (axisX.actualPosition - x, 2) + Math.Pow (axisY.actualPosition - y, 2));

                if(x > axisX.actualPosition && x > 0)
                    axisX.setStepsToDo(x - axisX.actualPosition);
                else if (x < axisX.actualPosition && x > 0)
                    axisX.setStepsToDo(x - axisX.actualPosition);
                else if(x > axisX.actualPosition && x < 0)
                    axisX.setStepsToDo((axisX.actualPosition - x)*-1);
                else if(x < axisX.actualPosition && x < 0)
                    axisX.setStepsToDo(x - axisX.actualPosition);

                if (y > axisY.actualPosition && y > 0)
                    axisY.setStepsToDo(y - axisY.actualPosition);
                else if (y < axisY.actualPosition && y > 0)
                    axisY.setStepsToDo(y - axisY.actualPosition);
                else if (y > axisY.actualPosition && y < 0)
                    axisY.setStepsToDo((axisY.actualPosition - y) * -1);
                else if (y < axisY.actualPosition && y < 0)
                    axisY.setStepsToDo(y - axisY.actualPosition);

			} 
			else if (distanceMode == "incremental") 
			{
				diagonal = (float)Math.Sqrt (Math.Pow (x, 2) + Math.Pow (y, 2));
				axisX.setStepsToDo (x);
				axisY.setStepsToDo (y);
			}

			float timeToDoThis = diagonal * 60000 / feedRate;

            Console.WriteLine("Tiempo para hacerlo:"+timeToDoThis);

            time = DateTime.Now;

            if (axisX.stepsToDo != 0)
            {
                axisX.setTimePerStep(timeToDoThis);
                axisX.setTimeNextStep(time);
            }
            if (axisY.stepsToDo != 0)
            {
                axisY.setTimePerStep(timeToDoThis);
                axisY.setTimeNextStep(time);
            }
			
			while (axisY.stepsToDo != 0 || axisX.stepsToDo != 0) 
			{
				if(time >= axisX.timeNextStep && axisX.stepsToDo != 0)
				{
                    Console.WriteLine("Ejex");
                    Main.data &= axisX.mask;
                    Main.data |= axisX.getNextStep();
                    axisX.setTimeNextStep(axisX.timeNextStep);
                    Main.Refresh();
				}

                if (time >= axisY.timeNextStep && axisY.stepsToDo != 0)
				{
                    Console.WriteLine("Ejey");
                    Main.data &= axisY.mask;
                    Main.data |= axisY.getNextStep();
                    axisY.setTimeNextStep(axisY.timeNextStep);
                    Main.Refresh();
				}

                time = DateTime.Now;

			}
		}

		public void Move(float x, float y)
		{
			Move (x,y,feedRate);
		}

	}
}

