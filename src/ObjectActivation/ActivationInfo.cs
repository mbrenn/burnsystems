using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace BurnSystems.ObjectActivation
{
    /// <summary>
    /// This class contains the information how the object shall be
    /// created, when the object shall be created and which additional actions shall
    /// be performed.
    /// </summary>
    internal class ActivationInfo
    {
        private CriteriaCatalogue criteriaCatalogue;

        /// <summary>
        /// Gets the criteriacatalogue
        /// </summary>
        public CriteriaCatalogue CriteriaCatalogue
        {
            get { return criteriaCatalogue; }
        }

        /// <summary>
        /// Stores the factory method for the object within an activationblock
        /// </summary>
        public Func<ActivationContainer, IEnumerable<IEnabler>, object> FactoryActivationContainer
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the expression for activation. 
        /// This is just used to ease debugging
        /// </summary>
        public Expression ExpressionActivationContainer
        {
            get;
            set;
        }

        /// <summary>
        /// Stores the factory method for the object within an activationblock
        /// </summary>
        public Func<ActivationBlock, IEnumerable<IEnabler>, object> FactoryActivationBlock
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the expression for activation. 
        /// This is just used to ease debugging
        /// </summary>
        public Expression ExpressionActivationBlock
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the ActivationInfo class.
        /// </summary>
        /// <param name="criteriaCatalogue">CriteriaCatalogue to be used</param>
        public ActivationInfo(CriteriaCatalogue criteriaCatalogue)
        {
            if (criteriaCatalogue == null)
                throw new ArgumentNullException("criteriaCatalogue");

            this.criteriaCatalogue = criteriaCatalogue;
        }

        public override string ToString()
        {
            return this.criteriaCatalogue.ToString();
        }
    }
}
