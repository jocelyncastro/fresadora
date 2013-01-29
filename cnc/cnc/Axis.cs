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
        float paso;
        string name;

		public event CNCEventHandler makeStep;

		public Axis(int stepsBycm, byte[] steps, byte mask, string unit,string name)
		{
			this.steps = steps;
            this.mask = mask;
			actualPosition = 0;
			currentStep = 0;
            paso = (float) 10.0 / stepsBycm;
            this.name = name;

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
            Console.WriteLine("Steps to do:" + stepsToDo);
		}

		public void setTimePerStep(float time)
		{
			timePerStep = time / stepsToDo;
            Console.WriteLine("Time per step: "+timePerStep);
		}

		public void setTimeNextStep(DateTime time)
		{
			timeNextStep = time.AddMilliseconds(timePerStep);
		}

        public byte getNextStep()
        {
            if (stepsToDo > 0)
            {
                stepsToDo--;

                actualPosition += paso;
                
				if (currentStep == 4)
                    currentStep = 0;

				Console.WriteLine("ActualPosition: " + actualPosition.ToString());
                Console.WriteLine("CurrentStep: " + currentStep.ToString());
                Console.WriteLine("StepsToDo: " + stepsToDo.ToString());

				makeStep(this, new CNCEventArgs(true,name));

                return steps[currentStep++];
            }
            else if (stepsToDo < 0)
            {
                stepsToDo++;

				actualPosition-= paso;
                
				if (currentStep < 0)
                    currentStep = 3;

				Console.WriteLine("ActualPosition: "+actualPosition);
                Console.WriteLine("CurrentStep: " + currentStep.ToString());
                Console.WriteLine("StepsToDo: " + stepsToDo.ToString());

				makeStep(this, new CNCEventArgs(false,name));

                return steps[currentStep--];
            }
            else
                return 0;
        }

	}
}

