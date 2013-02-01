using System;

namespace cnc
{
    public class AxisZ
    {
        float currentFeedRate, maxFeedRate;
        string distanceMode;
        public Axis axisZ;
		public event CNCEventHandler makeStep;
        DateTime time;
        float msecondsToDoThis;
        string unit;

        public AxisZ(Axis axisZ, float maxFeedRate)
        {
            this.axisZ = axisZ;
            this.maxFeedRate = maxFeedRate;
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

        public void FastMove(float z)
        {
            Move(z, maxFeedRate);
        }

        public void Move(float z)
        {
            Move(z, currentFeedRate);
        }

        public void setDistanceMode(string distanceMode)
        {
            this.distanceMode = distanceMode;
        }

        public void setUnit(string unit)
        {
            this.unit = unit;
        }

        public void Move(float z, float f)
        {
            float timeToDoThis = 0;
            currentFeedRate = f;
            if (distanceMode == "absolute")
            {
                stepsToDo(axisZ, z);

                msecondsToDoThis = (z - axisZ.actualPosition) * 60000 / currentFeedRate;
            }
            else if (distanceMode == "incremental")
            {
                axisZ.setStepsToDo(z);
                msecondsToDoThis = z * 60000 / currentFeedRate;
            }

            if (timeToDoThis < 0)
                timeToDoThis *= -1;

            time = DateTime.Now;

            setTime(axisZ);

            while (axisZ.stepsToDo != 0)
            {
                if (time >= axisZ.timeNextStep)
                    doStep(axisZ);
                time = DateTime.Now;

            }

        }
    }
}

