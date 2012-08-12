using System;
using System.Collections.Generic;
using System.Linq;
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
            var result = activates.Get(enablers).FirstOrDefault();
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
        /// Gets all object being bound to the specific type
        /// </summary>
        /// <param name="activates">Container to be queried</param>
        /// <returns>Enumeration of objects</returns>
        public static IEnumerable<T> GetAll<T>(this IActivates activates)
        {
            return activates.Get(
                new IEnabler[] { new ByTypeEnabler(typeof(T)) })
                .Cast<T>();
        }

        /// <summary>
        /// Gets the object by type
        /// </summary>
        /// <param name="activates">Activation container of the object</param>
        /// <param name="name">Name of the bound object. Name is set via 'BindToName'</param>
        /// <returns>Found type of null</returns>
        public static T GetByName<T>(this IActivates activates, string name)
        {
            return activates.Get<T>(
                new IEnabler[] { new ByNameEnabler(name) });
        }

        /// <summary>
        /// Gets all objects having the name
        /// </summary>
        /// <typeparam name="T">Expected type</typeparam>
        /// <param name="activates">Activation Container or block containing the information</param>
        /// <param name="name">Name being requested</param>
        /// <returns>Enumeration of all found objcts</returns>
        public static IEnumerable<T> GetAllByName<T>(this IActivates activates, string name)
        {
            return activates.Get(
                new IEnabler[] { new ByNameEnabler(name) })
                .Cast<T>();
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
            var criteria = container.Add(
                new CriteriaCatalogue(new ByTypeCriteria(typeof(T))));

            var helper = new BindingHelper();
            helper.ActivationInfo = criteria;
            return helper;
        }

        /// <summary>
        /// Creates a binding for a certain name
        /// </summary>
        /// <param name="container">Container to be bound</param>
        /// <param name="name">Name of the binding</param>
        /// <returns>Heper used to add behaviour</returns>
        public static BindingHelper BindToName(this ActivationContainer container, string name)
        {
            var criteria = container.Add(
                new CriteriaCatalogue(new ByNameCriteria(name)));

            var helper = new BindingHelper();
            helper.ActivationInfo = criteria;
            return helper;
        }

        /// <summary>
        /// Creates a specific object and all properties are injected
        /// </summary>
        /// <typeparam name="T">Object to be created</typeparam>
        /// <param name="activates">Activationcontext</param>
        /// <returns>Created type</returns>
        public static T Create<T>(this IActivates activates)
        {
            var instanceBuilder = new InstanceBuilder(activates);
            return instanceBuilder.Create<T>();
        }

        /// <summary>
        /// Creates a specific object and all properties are injected
        /// </summary>
        /// <param name="activates">Activationcontext</param>
        /// <param name="type">Type of the instance to be created</param>
        /// <returns>Created type</returns>
        public static object Create(this IActivates activates, Type type)
        {
            var instanceBuilder = new InstanceBuilder(activates);
            return instanceBuilder.Create(type);
        }
    }
}
