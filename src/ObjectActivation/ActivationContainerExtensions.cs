using System;
using System.Collections.Generic;
using BurnSystems.ObjectActivation.Criteria;
using BurnSystems.ObjectActivation.Enabler;

namespace BurnSystems.ObjectActivation
{
    /// <summary>
    /// Helper class containing several extension methods
    /// for the activation container.
    /// </summary>
    public static class ActivationContainerExtensions
    {
        /// <summary>
        /// Gets the object by enablers
        /// </summary>
        /// <param name="activates">The container being able to activate
        /// or reuse objects</param>
        /// <param name="enablers">List of enablers</param>
        /// <returns>The activated or reused object</returns>
        public static T Get<T>(this IActivates activates, IEnumerable<IEnabler> enablers)
        {
            var result = activates.Get(enablers);
            if (result == null)
            {
                return default(T);
            }

            return (T)result;
        }

        /// <summary>
        /// Gets the object by type
        /// </summary>
        /// <param name="activates">Activation container of the object</param>
        /// <returns>Found type of null</returns>
        public static T Get<T>(this IActivates activates)
        {
            return activates.Get<T>(
                new IEnabler[] { new ByTypeEnabler(typeof(T)) });
        }

        /// <summary>
        /// Creates a binding for a certain type.
        /// The specific implementation of object retrieval has to be set 
        /// with an additional call.
        /// </summary>
        /// <param name="container">Container, where binding occurs</param>
        /// <returns>Helper used to add behaviour</returns>
        public static BindingHelper Bind<T>(this ActivationContainer container)
        {
            var result = container.Add(
                new CriteriaCatalogue(new ByTypeCriteria(typeof(T))));

            var helper = new BindingHelper();
            helper.ActivationInfo = result;
            return helper;
        }
    }
}
