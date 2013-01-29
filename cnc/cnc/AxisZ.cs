using System;

namespace cnc
{
    public class AxisZ
    {
        float feedRate;
        string distanceMode;
        Axis axisZ;
        
        public AxisZ(string distanceMode, string unit,int axisZStepsPercm)
        {
            this.distanceMode = distanceMode;
            axisZ = new Axis(axisZStepsPercm, new byte[] { 0, 16, 32, 48 }, 207, unit);
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
                timeToDoThis = (axisZ.actualPosition - z) * 60 / feedRate;
                if (timeToDoThis < 0)
                    timeToDoThis *= -1;
            }
            else if (distanceMode == "incremental")
            {
                axisZ.setStepsToDo(z);
                timeToDoThis = z * 60 / feedRate;
                if (timeToDoThis < 0)
                    timeToDoThis *= -1;
            }
            axisZ.setTimePerStep(timeToDoThis);
            axisZ.setTimeNextStep();

            while (axisZ.stepsToDo != 0)
            {
                if (DateTime.Now >= axisZ.timeNextStep)
                {
                    Main.data &= axisZ.mask;
                    Main.data |= axisZ.getNextStep();
                    axisZ.setTimeNextStep();
                    Main.Refresh();
                }
            }

        }
    }
}

