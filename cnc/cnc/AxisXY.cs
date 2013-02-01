using System;

namespace cnc
{
	public class AxisXY
	{
		float currentFeedRate;
        float maxFeedRate;
		public Axis axisX, axisY;
		string distanceMode;
        DateTime time;
        float diagonal;
        float msecondsToDoThis;
        string unit;

		public AxisXY (Axis axisX, Axis axisY, float maxFeedRate)
		{
            this.axisX = axisX;
            this.axisY = axisY;
            this.maxFeedRate = maxFeedRate;
		}


        public void setDistanceMode(string distanceMode)
        {
            this.distanceMode = distanceMode;
        }

        public void setUnit(string unit)
        {
            this.unit = unit;
        }

        void stepsToDo(Axis axis, float position)
        {
            if (position > axis.actualPosition && position < 0)
                axis.setStepsToDo((axis.actualPosition - position) * -1);
            else
                axis.setStepsToDo(position - axis.actualPosition);
        }

        void setTime(Axis axis)
        {
            if (axis.stepsToDo != 0)
            {
                axis.setTimePerStep(msecondsToDoThis);
                axis.setTimeNextStep(time);
            }
        }

        void doStep(Axis axis)
        {
            Main.data &= axis.mask;
            Main.data |= axis.getNextStep();
            axis.setTimeNextStep(axis.timeNextStep);
            Main.Refresh();
        }

        public void Move(float x, float y, float f)
        {
            currentFeedRate = f;
            if (distanceMode == "absolute")
            {
                diagonal = (float)Math.Sqrt(Math.Pow(axisX.actualPosition - x, 2) + Math.Pow(axisY.actualPosition - y, 2));

                stepsToDo(axisX, x);
                stepsToDo(axisY, y);

            }
            else if (distanceMode == "incremental")
            {
                diagonal = (float)Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
                axisX.setStepsToDo(x);
                axisY.setStepsToDo(y);
            }

            msecondsToDoThis = diagonal * 60000 / currentFeedRate;

            time = DateTime.Now;


            setTime(axisX);
            setTime(axisY);

            while (axisY.stepsToDo != 0 || axisX.stepsToDo != 0)
            {
                if (time >= axisX.timeNextStep && axisX.stepsToDo != 0)
                    doStep(axisX);

                if (time >= axisY.timeNextStep && axisY.stepsToDo != 0)
                    doStep(axisY);

                time = DateTime.Now;

            }
        }

        public void FastMove(float x, float y)
        {
            Move(x,y,maxFeedRate);
        }

		public void Move(float x, float y)
		{
			Move (x,y,currentFeedRate);
		}

	}
}

