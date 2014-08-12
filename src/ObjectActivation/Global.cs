using System;

namespace BurnSystems.ObjectActivation
{
    /// <summary>
    /// Stores intances of activation container, which may be used in global scope and 
    /// can be accessed via global variables.
    /// </summary>
    public class Global
    {
        /// <summary>
        /// Stores the activation container on application scope
        /// </summary>
        private static ActivationContainer application;

        /// <summary>
        /// Returns the activation container on application scope. 
        /// The variable has to be initialized via InitForApplication()
        /// </summary>
        public static ActivationContainer Application
        {
            get
            {
                if (application == null)
                {
                    InitForApplication();
                }

                return application;
            }
        }

        /// <summary>
        /// Initializes the activation container on application scope
        /// </summary>
        private static void InitForApplication()
        {
            if (application == null)
            {
                application = new ActivationContainer("Global.Application");
            }
        }

        /// <summary>
        /// Deletes the activattion container and creates a complete new instance. 
        /// This is necessary for unit testing for example
        /// </summary>
        public static void Reset()
        {
            application = null;            
        }
    }
}
