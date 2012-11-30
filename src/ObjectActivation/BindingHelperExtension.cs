using System;
using System.Linq;
using BurnSystems.ObjectActivation.Enabler;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace BurnSystems.ObjectActivation
{
    /// <summary>
    /// Description of BinderHelperExtension.
    /// </summary>
    public static class BindingHelperExtension
    {
        /// <summary>
        /// Stores the memberinfo for
        /// public static void InstanceBuilder::AddPropertyAssignmentsByReflection(object target, IActivates container)
        /// </summary>
        private static MethodInfo instanceBuilderAddPropertyAssignmentsByReflection;

        static BindingHelperExtension()
        {
            var type = typeof(InstanceBuilder);
            instanceBuilderAddPropertyAssignmentsByReflection = type.GetMethod("AddPropertyAssignmentsByReflection");
        }

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
                // No constructor with InjectAttribute, default implementation
                helper.ActivationInfo.FactoryActivationContainer =
                    (x, y) =>
                    {
                        var result = Activator.CreateInstance(typeof(T));
                        InstanceBuilder.AddPropertyAssignmentsByReflection(result, x);

                        return result;
                    };

                helper.ActivationInfo.FactoryActivationBlock =
                    (x, y) =>
                    {
                        var result = Activator.CreateInstance(typeof(T));
                        InstanceBuilder.AddPropertyAssignmentsByReflection(result, x);

                        return result;
                    };
            }
            else
            {
                // Constructor Count with injection is more than, create
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
                var newContainer =
                    Expression.New(
                        constructor,
                        parameterContainerList);
                var resultContainer = Expression.Parameter(typeof(object), "result");
                var resultBlock = Expression.Parameter(typeof(object), "result");

                // new {Constructor}({Parameterlist});
                var newBlock =
                    Expression.New(
                        constructor,
                        parameterBlockList);

                var blockContainer = Expression.Block(
                    new ParameterExpression[] { resultContainer },
                    new Expression[]{ 
                        Expression.Assign ( resultContainer, newContainer ),
                        Expression.Call(
                            instanceBuilderAddPropertyAssignmentsByReflection,
                            resultContainer, 
                            containerExpression 
                            ),
                        resultContainer
                    }
                );

                var blockResult = Expression.Block(
                    new ParameterExpression[] { resultBlock},
                    new Expression[]{ 
                        Expression.Assign ( resultBlock, newBlock),
                        Expression.Call(
                            instanceBuilderAddPropertyAssignmentsByReflection,
                            resultBlock, 
                            blockExpression 
                            ),
                        resultBlock
                    }
                );

                // Input
                var enumerableExpression1 = Expression.Parameter(typeof(IEnumerable<IEnabler>), "Enablers");
                var enumerableExpression2 = Expression.Parameter(typeof(IEnumerable<IEnabler>), "Enablers");

                var instanceContainer = Expression.Lambda<Func<ActivationContainer, IEnumerable<IEnabler>, object>>(
                    blockContainer, 
                    new [] { containerExpression, enumerableExpression1 }).Compile();
                var instanceBlock = Expression.Lambda<Func<ActivationBlock, IEnumerable<IEnabler>, object>>(
                    blockResult, 
                    new [] { blockExpression, enumerableExpression2 }).Compile();

                helper.ActivationInfo.FactoryActivationContainer = instanceContainer;
                helper.ActivationInfo.ExpressionActivationContainer = blockContainer;
                helper.ActivationInfo.FactoryActivationBlock = instanceBlock;
                helper.ActivationInfo.ExpressionActivationBlock = blockResult;
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
                (container, enablers) => factory();

            helper.ActivationInfo.FactoryActivationBlock =
                (block, enablers) => 
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
                (container, enablers) => factory(container);

            helper.ActivationInfo.FactoryActivationBlock =
                (block, enablers) =>
                    factory(block);

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
                (block, enablers) =>
                {
                    var found = oldContainerFactory(block, enablers);

                    block.Add(
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
                (container, enablers) =>
                {
                    var foundInstance =
                        container.ActiveInstances.Find(helper.ActivationInfo);

                    if (foundInstance == null)
                    {
                        // Singleton has not been created yet. Create new object
                        foundInstance = oldContainerFactory(container, enablers);
                        container.ActiveInstances.Add(
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
                (block, enablers) =>
                {
                    var result = helper.ActivationInfo.FactoryActivationContainer(
                        block.Container, 
                        enablers);

                    if (result == null && block.OuterBlock != null)
                    {
                        return helper.ActivationInfo.FactoryActivationBlock(block.OuterBlock, enablers);
                    }

                    return result;
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
            return AsScopedIn(helper, x => true);
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
        /// <param name="where">Condition, whose scope shall be used for automatic disposing</param>
        /// <returns>The binding helper</returns>
        public static BindingHelper AsScopedIn(this BindingHelper helper, Func<ActivationBlock, bool> where)
        {
            // As Scoped is not allowed in ActivationContainer
            // Only within activation blocks, the disposal has to be organized. 
            var oldContainerFactory = helper.ActivationInfo.FactoryActivationBlock;
            helper.ActivationInfo.FactoryActivationContainer =
                (container, block) =>
                {
                    throw new InvalidOperationException("AsScoped cannot be created in ActivationContainer");
                };

            helper.ActivationInfo.FactoryActivationBlock =
                (block, container) =>
                {
                    var relevantActivationBlock = block.FindActivationBlockInChain(where);
                    if (relevantActivationBlock == null)
                    {
                        // No activation block matches
                        var enablerText = new StringBuilder();
                        foreach (var enabler in container)
                        {
                            enablerText.AppendLine(enablerText.ToString());
                        }

                        throw new InvalidOperationException(LocalizationBS.NoScopeFoundForListOfEnablers + enablerText.ToString());
                    }

                    var found =
                        relevantActivationBlock.ActiveInstances.Find(helper.ActivationInfo);

                    if (found == null)
                    {
                        found = oldContainerFactory(block, container);

                        // Ok, Add to first match

                        if (relevantActivationBlock != null)
                        {
                            relevantActivationBlock.Add(
                                new ActiveInstance()
                                {
                                    Criterias = helper.ActivationInfo.CriteriaCatalogue,
                                    Value = found
                                });
                        }
                    }

                    return found;
                };

            return helper;
        }

        /// <summary>
        /// Marks the current binding as scoped, which means that 
        /// the same instance shall be returned, when it has been created 
        /// within the same scope. 
        /// Scopes are defined by Activation Blocks.
        /// The name of the activation block defines, whose scope will be used for automatic disposing
        /// </summary>
        /// <param name="helper">Binding helper to be used</param>
        /// <param name="nameOfActivationBlock">Name of block, whose scope shall be used for automatic disposing</param>
        /// <returns>The binding helper</returns>
        public static BindingHelper AsScopedIn(this BindingHelper helper, string nameOfActivationBlock)
        {
            return AsScopedIn(helper, x => x.Name == nameOfActivationBlock);
        }
    }
}