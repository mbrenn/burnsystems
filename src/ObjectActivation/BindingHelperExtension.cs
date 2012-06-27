using System;

namespace BurnSystems.ObjectActivation
{
    /// <summary>
    /// Description of BinderHelperExtension.
    /// </summary>
    public static class BindingHelperExtension
    {
        /// <summary>
        /// Binds the object to a specific class. 
        /// The class is created when necessary
        /// </summary>
        /// <param name="helper">Helper being used</param>
        /// <returns>The Binding Helper</returns>
        public static BindingHelper To<T>(this BindingHelper helper)
        {
            helper.ActivationInfo.FactoryActivationContainer =
                (x, y) =>
                {
                    return Activator.CreateInstance(typeof(T));
                };

            helper.ActivationInfo.FactoryActivationBlock =
                (x, y) =>
                {
                    return Activator.CreateInstance(typeof(T));
                };

            return helper;
        }

        /// <summary>
        /// Binds the object to a specific constant
        /// </summary>
        /// <param name="helper">Helper being used to store all intermediate data</param>
        /// <param name="value">Value to be executed</param>
        /// <returns>The Binding Helper</returns>
        public static BindingHelper ToConstant(this BindingHelper helper, object value)
        {
            helper.ActivationInfo.FactoryActivationContainer =
                (x, y) => value;

            helper.ActivationInfo.FactoryActivationBlock =
                (x, y) => value;

            return helper;
        }

        /// <summary>
        /// Binds the object to a specific factory method
        /// </summary>
        /// <param name="helper">Helper being executed</param>
        /// <param name="factory">Factory being executed</param>
        /// <returns></returns>
        public static BindingHelper To(this BindingHelper helper, Func<object> factory)
        {
            helper.ActivationInfo.FactoryActivationContainer =
                (x, y) => factory();

            helper.ActivationInfo.FactoryActivationBlock =
                (x, y) => 
                    factory();

            return helper;
        }

        /// <summary>
        /// Marks the current binding as transient, which means 
        /// that the factory function will be executed during each
        /// resolution.
        /// </summary>
        /// <param name="helper">Binding helper to be used</param>
        /// <returns>The binding helper</returns>
        public static BindingHelper AsTransient(this BindingHelper helper)
        {
            // No change for activation container. 
            // Only within activation blocks, the disposal has to be organized. 
            var oldContainerFactory = helper.ActivationInfo.FactoryActivationBlock;
            helper.ActivationInfo.FactoryActivationBlock =
                (x, y) =>
                {
                    var found = oldContainerFactory(x, y);

                    x.Add(
                        new ActiveInstance()
                        {
                            Criterias = helper.ActivationInfo.CriteriaCatalogue,
                            Value = found
                        });
                    return found;
                };
            return helper;
        }

        /// <summary>
        /// Marks the current binding as singleton, which means
        /// that the same instance will be returned every time, when one
        /// instance has been created
        /// </summary>
        /// <param name="helper">Binding helper to be used</param>
        /// <returns>The binding helper</returns>
        public static BindingHelper AsSingleton(this BindingHelper helper)
        {
            var oldContainerFactory = helper.ActivationInfo.FactoryActivationContainer;

            helper.ActivationInfo.FactoryActivationContainer =
                (x, y) =>
                {
                    var foundInstance =
                        x.ActiveInstances.Find(helper.ActivationInfo);

                    if (foundInstance == null)
                    {
                        // Singleton has not been created yet. Create new object
                        foundInstance = oldContainerFactory(x, y);
                        x.ActiveInstances.Add(
                            new ActiveInstance()
                            {
                                Value = foundInstance,
                                Criterias = helper.ActivationInfo.CriteriaCatalogue
                            });
                    }

                    // Return instance
                    return foundInstance;
                };

            helper.ActivationInfo.FactoryActivationBlock =
                (x, y) =>
                {
                    return helper.ActivationInfo.FactoryActivationContainer(
                        x.InnerContainer, 
                        y);
                };

            return helper;
        }

        /// <summary>
        /// Marks the current binding as scoped, which means that 
        /// the same instance shall be returned, when it has been created 
        /// within the same scope. 
        /// Scopes are defined by Activation Block.
        /// When the object is used within activation block, 
        /// the instance will be created during each resolution request.
        /// </summary>
        /// <param name="helper">Binding helper to be used</param>
        /// <returns>The binding helper</returns>
        public static BindingHelper AsScoped(this BindingHelper helper)
        {
            // No change for activation container. 
            // Only within activation blocks, the disposal has to be organized. 
            var oldContainerFactory = helper.ActivationInfo.FactoryActivationBlock;
            helper.ActivationInfo.FactoryActivationBlock =
                (x, y) =>
                {

                    var found =
                        x.ActiveInstances.Find(helper.ActivationInfo);

                    if (found == null)
                    {
                        found = oldContainerFactory(x, y);

                        x.Add(
                            new ActiveInstance()
                            {
                                Criterias = helper.ActivationInfo.CriteriaCatalogue,
                                Value = found
                            });
                    }

                    return found;
                };

            return helper;
        }
    }
}