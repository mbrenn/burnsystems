using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BurnSystems.ObjectActivation;

namespace BurnSystems.UnitTests.ObjectActivation.Objects
{
    public class CalculationContainer
    {
        [Inject]
        public ICalculator Calculator
        {
            get;
            set;
        }
    }

    public class CalculationContainerGetter
    {
        [Inject]
        public ICalculator Calculator
        {
            get;
            private set;
        }
    }

    public class CalculationContainerByName
    {
        [Inject]
        public ICalculator CalculatorByType
        {
            get;
            set;
        }

        [Inject("Add")]
        public ICalculator Calculator
        {
            get;
            set;
        }

        [Inject("AddByTwo")]
        public ICalculator CalculatorByTwo
        {
            get;
            set;
        }
    }

    public class CalculationContainerContainer
    {
        [Inject]
        public CalculationContainer Container
        {
            get;
            set;
        }
    }
}
