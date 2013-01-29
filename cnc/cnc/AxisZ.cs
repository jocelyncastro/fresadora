using System;

namespace cnc
{
    public class AxisZ
    {
        float feedRate;
        string distanceMode;
        public Axis axisZ;
		public event CNCEventHandler makeStep;
        DateTime time;
        public AxisZ(string distanceMode, string unit,int axisZStepsPercm)
        {
            this.distanceMode = distanceMode;
            axisZ = new Axis(axisZStepsPercm, new byte[] { 0, 16, 32, 48 }, 207, unit,"z");
        	axisZ.makeStep += HandlemakeStep;
		}

        void HandlemakeStep (object sender, CNCEventArgs e)
        {
			Console.WriteLine("z noto step");
			makeStep(sender,e);
        }

        public void Move(float z)
        {
            Move(z, feedRate);
        }

        public void Move(float z, float f)
        {
            float timeToDoThis = 0;
            feedRate = f;
            if (distanceMode == "absolute")
            {
                axisZ.setStepsToDo(axisZ.actualPosition - z);
                timeToDoThis = (axisZ.actualPosition - z) * 60000 / feedRate;
                if (timeToDoThis < 0)
                    timeToDoThis *= -1;
            }
            else if (distanceMode == "incremental")
            {
                axisZ.setStepsToDo(z);
                timeToDoThis = z * 60000 / feedRate;
                if (timeToDoThis < 0)
                    timeToDoThis *= -1;
            }

            time = DateTime.Now;

            if (axisZ.stepsToDo != 0)
            {
                axisZ.setTimePerStep(timeToDoThis);
                axisZ.setTimeNextStep(time);
            }

            while (axisZ.stepsToDo != 0)
            {
                if (time >= axisZ.timeNextStep)
                {
                    Main.data &= axisZ.mask;
                    Main.data |= axisZ.getNextStep();
                    axisZ.setTimeNextStep(axisZ.timeNextStep);
                    Main.Refresh();
                }

                time = DateTime.Now;

            }

        }
    }
}

