using MotorEmpireAutohaus.Tools.Utility.Messages;

namespace Tools.Utility.VehiclePostValidator
{
    public static class VehiclePostValidator
    {
        public static bool ProductionYearIsValid(int providedYear, string providedGeneration=null)
        {
            if(providedGeneration is null)
            {
                return CheckForValidYear(providedYear);
            }

            Tuple<int,int> productionYears= ObtainYearsFromGeneration(providedGeneration);
            if(providedYear>=productionYears.Item1 && providedYear <= productionYears.Item2)
            {
                return true;
            }

            return false;
        }

        private static bool CheckForValidYear(int year)
        {
            if (year >= 1912 && year <= 2023) 
            { 
                return true;
            }

            return false; 
        }

        private static Tuple<int, int> ObtainYearsFromGeneration(string generation)
        {
            try {
                int productionYearStart = int.Parse(generation.Substring(generation.IndexOf('[') + 1, 4));
                int productionYearEnd = int.Parse(generation.Substring(generation.IndexOf('-') + 1, 4));
                return new Tuple<int,int>(productionYearStart, productionYearEnd);
            }
            catch(ArgumentException)
            {
                CrossPlatformMessageRenderer.RenderMessages("The year you have provided is not valid because it doesn't match the generation year bounds!", "Retry", 6);
                return null;
            }
        }
    }
}
