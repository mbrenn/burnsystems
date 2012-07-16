using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BurnSystems.ObjectActivation;

namespace BurnSystems.UnitTests.ObjectActivation
{
    public class CalculationContainer
    {
        public ICalculator Calculator
        {
            get;
            set;
        }
    }

    public class CalculationContainerGetter
    {
        public ICalculator Calculator
        {
            get;
            private set;
        }
    }

    public class CalculationContainerByName
    {
        public ICalculator CalculatorByType
        {
            get;
            set;
        }

        [ByName("Add")]
        public ICalculator Calculator
        {
            get;
            set;
        }

        [ByName("AddByTwo")]
        public ICalculator CalculatorByTwo
        {
            get;
            set;
        }
    }

    public class CalculationContainerContainer
    {
        public CalculationContainer Container
        {
            get;
            set;
        }
    }
}
