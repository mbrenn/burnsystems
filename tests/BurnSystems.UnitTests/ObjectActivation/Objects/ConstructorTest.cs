using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BurnSystems.ObjectActivation;

namespace BurnSystems.UnitTests.ObjectActivation.Objects
{
    public class ConstructorTest
    {
        public bool IsConstructed
        {
            get;
            set;
        }

        public ICalculator Calculator
        {
            get;
            set;
        }

        public ConstructorTest()
        {
        }

        [Inject]
        public ConstructorTest(ICalculator calculator)
        {
            this.IsConstructed = true;
            this.Calculator = calculator;
        }
    }

    public class ConstructorTestContainer
    {
        [Inject]
        public ConstructorTest Test
        {
            get;
            set;
        }
    }
}
