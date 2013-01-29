using System;

namespace cnc
{
	public class AxisXY
	{
		float feedRate;
		Axis axisX, axisY;
		string distanceMode;

		public AxisXY (string distanceMode, string unit, int axisXStepsPercm, int axisYStepsPercm)
		{
            this.distanceMode = distanceMode;
			axisX = new Axis(axisXStepsPercm, new byte[]{0,1,2,3},252, unit);
			axisY = new Axis(axisYStepsPercm, new byte[]{0,4,8,12},243, unit);

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

			float timeToDoThis = diagonal * 60 / feedRate;

			axisX.setTimePerStep(timeToDoThis);
			axisY.setTimePerStep(timeToDoThis);

			axisX.setTimeNextStep();
			axisY.setTimeNextStep();

			while (axisY.stepsToDo != 0 || axisX.stepsToDo != 0) 
			{
				if(DateTime.Now >= axisX.timeNextStep)
				{
                    Console.WriteLine("Ejex");
                    Main.data &= axisX.mask;
                    Main.data |= axisX.getNextStep();
                    axisX.setTimeNextStep();
                    Main.Refresh();
				}

                if (DateTime.Now >= axisY.timeNextStep)
				{
                    Console.WriteLine("Ejey");
                    Main.data &= axisY.mask;
                    Main.data |= axisY.getNextStep();
                    axisY.setTimeNextStep();
                    Main.Refresh();
				}
			}
		}

		public void Move(float x, float y)
		{
			Move (x,y,feedRate);
		}

	}
}

