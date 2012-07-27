using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;
using BurnSystems.ObjectActivation.Enabler;

namespace BurnSystems.ObjectActivation
{
    /// <summary>
    /// This class is capable to hold an IActivates interface and creates the necessary
    /// interfaces. 
    /// </summary>
    public class InstanceBuilder
    {
        /// <summary>
        /// Stores the container or block being used for instantiation
        /// </summary>
        private IActivates container;

        /// <summary>
        /// Stores the cache for instantiation
        /// </summary>
        private static Dictionary<Type, InstantiationCacheEntry> cache = new Dictionary<Type, InstantiationCacheEntry>();

        /// <summary>
        /// Initializes a new instance of the instance builder
        /// </summary>
        /// <param name="container"></param>
        public InstanceBuilder(IActivates container)
        {
            this.container = container;
        }

        /// <summary>
        /// Creates an instance of the object
        /// </summary>
        /// <typeparam name="T">Type of the object to be requested</typeparam>
        /// <returns>Created instance</returns>
        public T Create<T>()
        {
            InstantiationCacheEntry entry;
            if (cache.TryGetValue(typeof(T), out entry))
            {
                return (T)entry.FactoryMethod(this.container);
            }
            else
            {
                // var result;
                var result = Expression.Parameter(typeof(T), "result");
                var containerExpression = Expression.Parameter(typeof(IActivates), "container");
                var expressions = new List<Expression>();

                // var result;
                // result = new {typeof(T)}();
                expressions.Add(Expression.Assign(result, Expression.New(typeof(T))));
                var variables = this.AddPropertyAssignments(typeof(T), result, containerExpression, expressions);

                // return result;
                expressions.Add(result);
                variables.Add(result);

                var expression = Expression.Block(
                    // var result;
                    variables,
                    expressions);                

                // Creates cache entry containing the compiled method
                entry = new InstantiationCacheEntry();
                entry.FactoryMethod = Expression.Lambda<Func<IActivates, object>>(expression, containerExpression).Compile();
                entry.Expression = expression;

                // Store in dictionary
                cache[typeof(T)] = entry;

                // Call!
                return (T)entry.FactoryMethod(this.container);
            }
        }

        /// <summary>
        /// Just a temporary counter to get a better naming of variables
        /// </summary>
        private int tempCounter = 1;

        /// <summary>
        /// Adds property assignements to the list of expression. 
        /// The Assignments are found by recursive visiting of all properties of the type
        /// </summary>
        /// <param name="type">Type of the instance 'result', whose properties will be visited</param>
        /// <param name="target">Object, where properties shall be assigned to</param>
        /// <param name="containerExpression">Expression containing the container</param>
        /// <param name="expressions">Expressions, where assignments shall be added</param>
        private List<ParameterExpression> AddPropertyAssignments(Type type, Expression target, ParameterExpression containerExpression, List<Expression> expressions)
        {
            var result = new List<ParameterExpression>();

            foreach (var property in type.GetProperties(BindingFlags.SetField | BindingFlags.Instance | BindingFlags.Public))
            {
                // No action for primitive types
                if (property.PropertyType.IsEnum || property.PropertyType.IsPrimitive)
                {
                    continue;
                }

                // Create temporary variable, where Binding will be tested first                
                var tempVariable = Expression.Variable(property.PropertyType, "temp" + this.tempCounter);
                this.tempCounter++;
                var setMethod = property.GetSetMethod();
                if (setMethod == null || property.GetSetMethod().IsPrivate)
                {
                    // No private properties are set
                    continue;
                }

                var byName = property.GetCustomAttributes(typeof(ByNameAttribute), false);
                NewExpression enablerCreation;
                var enablers = new List<IEnabler>();

                if (byName != null && byName.Length != 0)
                {
                    var byNameAttribute = byName[0] as ByNameAttribute;
                    enablerCreation =
                        Expression.New(
                            typeof(Enabler.ByNameEnabler).GetConstructor(new[] { typeof(string) }),
                            Expression.Constant(byNameAttribute.Name));
                }
                else
                {
                    enablerCreation =
                        Expression.New(
                            typeof(Enabler.ByTypeEnabler).GetConstructor(new[] { typeof(Type) }),
                            Expression.Constant(property.PropertyType));
                }

                // OK, we found it, add expression
                var getMethod = typeof(IActivates).GetMethod("Get");
                // var {parameters} = new Enabler.ByTypeEnabler [] { new ByTypeEnabler({typeof(property)}); }
                var parameters = Expression.NewArrayInit(
                    typeof(IEnabler),
                    enablerCreation);

                // {result}.{property} = {this.container}.Get({parameters});
                var memberAccess = Expression.MakeMemberAccess(target, property);
                expressions.Add(
                    Expression.Assign(
                        tempVariable,
                        Expression.Convert(
                            Expression.Call(
                                containerExpression,
                                getMethod,
                                parameters),
                            property.PropertyType)));

                
                // Performs the following action, if tempVariable is not null
                var conditionalStatements = new List<Expression>();
                conditionalStatements.Add(
                    Expression.Assign(
                    memberAccess,
                    tempVariable));

                var variables = this.AddPropertyAssignments(property.PropertyType, memberAccess, containerExpression, conditionalStatements);
                if (conditionalStatements.Count > 0)
                {                    
                    var innerBlock = Expression.Block(variables, conditionalStatements);
                    expressions.Add(
                        Expression.IfThen(
                            Expression.NotEqual(tempVariable, Expression.Constant(null)),
                            innerBlock));
                }

                result.Add(tempVariable);
            }

            return result;
        }
    }
}
