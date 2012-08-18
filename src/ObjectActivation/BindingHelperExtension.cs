using System;
using System.Linq;
using BurnSystems.ObjectActivation.Enabler;
using System.Linq.Expressions;
using System.Collections.Generic;

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
            // Checks, if we have an injection constructor
            var constructors = typeof(T).GetConstructors(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public)
                .Where (x=>x.GetCustomAttributes (typeof (InjectAttribute), false).Length > 0)
                .ToList();

            if (constructors.Count > 1)
            {
                throw new InvalidOperationException("More than one constructor with InjectAttribute in class: " + typeof(T).FullName);
            }

            if (constructors.Count == 0)
            {
                // No constructor with InjectAttribute
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
            }
            else
            {
                // Constructor Count = 1, create
                var constructor = constructors[0];

                // Find Get-Method of container and block
                var containerExpression = Expression.Parameter(typeof(ActivationContainer), "activationContainer");
                var blockExpression = Expression.Parameter(typeof(ActivationBlock), "activationBlock");

                var getContainerMethod = typeof(ActivationContainer).GetMethod("GetAll");
                var getBlockMethod = typeof(ActivationBlock).GetMethod("GetAll");

                // Create parameter list
                var parameterContainerList = new List<Expression>();
                var parameterBlockList = new List<Expression>();
                foreach (var parameter in constructor.GetParameters())
                {
                    // new [] { ByTypeEnabler(typeof({ParameterType})) }
                    var byTypeEnabler =
                        Expression.NewArrayInit(
                            typeof(IEnabler),
                            Expression.New(
                                typeof(Enabler.ByTypeEnabler).GetConstructor(new[] { typeof(Type) }),
                                Expression.Constant(parameter.ParameterType)));

                    // (parameterType) container.Get({ByTypeEnabler}).FirstOrDefault()
                    var parameterContainer = Expression.Convert(
                                Expression.Call(
                                    InstanceBuilder.FirstOrDefaultMethod,
                                    Expression.Call(
                                        containerExpression,
                                        getContainerMethod,
                                        byTypeEnabler)),
                                parameter.ParameterType);

                    // (parameterType) block.Get({ByTypeEnabler}).FirstOrDefault()
                    var parameterBlock = Expression.Convert(
                                Expression.Call(
                                    InstanceBuilder.FirstOrDefaultMethod,
                                    Expression.Call(
                                        blockExpression,
                                        getBlockMethod,
                                        byTypeEnabler)),
                                parameter.ParameterType);

                    parameterContainerList.Add(parameterContainer);
                    parameterBlockList.Add(parameterBlock);
                }

                // new {Constructor}({Parameterlist});
                var resultContainer =
                    Expression.New(
                        constructor,
                        parameterContainerList);

                // new {Constructor}({Parameterlist});
                var resultBlock =
                    Expression.New(
                        constructor,
                        parameterBlockList);

                // Input
                var enumerableExpression1 = Expression.Parameter(typeof(IEnumerable<IEnabler>), "Enablers");
                var enumerableExpression2 = Expression.Parameter(typeof(IEnumerable<IEnabler>), "Enablers");

                var instanceContainer = Expression.Lambda<Func<ActivationContainer, IEnumerable<IEnabler>, object>>(resultContainer, new [] { containerExpression, enumerableExpression1 }).Compile();
                var instanceBlock = Expression.Lambda<Func<ActivationBlock, IEnumerable<IEnabler>, object>>(resultBlock, new [] { blockExpression, enumerableExpression2 }).Compile();

                helper.ActivationInfo.FactoryActivationContainer = instanceContainer;
                helper.ActivationInfo.ExpressionActivationContainer = resultContainer;
                helper.ActivationInfo.FactoryActivationBlock = instanceBlock;
                helper.ActivationInfo.ExpressionActivationBlock = resultBlock;
            }

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
        /// Binds the object to a specific factory method
        /// </summary>
        /// <param name="helper">Helper being executed</param>
        /// <param name="factory">Factory being executed</param>
        /// <returns></returns>
        public static BindingHelper To(this BindingHelper helper, Func<IActivates, object> factory)
        {
            helper.ActivationInfo.FactoryActivationContainer =
                (x, y) => factory(x);

            helper.ActivationInfo.FactoryActivationBlock =
                (x, y) =>
                    factory(x);

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
                        x.Container, 
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
            helper.ActivationInfo.FactoryActivationContainer =
                (x, y) =>
                {
                    throw new InvalidOperationException("AsScoped cannot be created in ActivationContainer");
                };

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