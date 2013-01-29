using System;

namespace cnc
{
	public class Axis
	{

		int stepsByUnit;
		public float actualPosition;
		byte[] steps;
		public int stepsToDo;
		int currentStep;
		public float timePerStep;
		public DateTime timeNextStep;
        public byte mask;

		public Axis(int stepsBycm, byte[] steps, byte mask, string unit)
		{
			this.steps = steps;
            this.mask = mask;
			actualPosition = 0;
			currentStep = 0;

            switch (unit)
            { 
                case "millimeters":
                    stepsByUnit = stepsBycm / 10;
                    break;
                case "inches":
                    stepsByUnit = (int)(stepsBycm * 2.54);
                    break;
            }

		}

		public void setStepsToDo(float dato)
		{
			stepsToDo = (int)(dato * stepsByUnit);
		}

		public void setTimePerStep(float time)
		{
			timePerStep = time / stepsToDo;
		}

		public void setTimeNextStep()
		{
			timeNextStep = DateTime.Now.AddMilliseconds(timePerStep);
		}

        public byte getNextStep()
        {
            if (stepsToDo > 0)
            {
                stepsToDo--;

                if (currentStep == 4)
                    currentStep = 0;

                Console.WriteLine("CurrentStep: " + currentStep.ToString());
                Console.WriteLine("StepsToDo: " + stepsToDo.ToString());

                return steps[currentStep++];
            }
            else if (stepsToDo < 0)
            {
                stepsToDo++;
                if (currentStep < 0)
                    currentStep = 3;

                Console.WriteLine("CurrentStep: " + currentStep.ToString());
                Console.WriteLine("StepsToDo: " + stepsToDo.ToString());

                return steps[currentStep--];
            }
            else
                return 0;
        }

	}
}

