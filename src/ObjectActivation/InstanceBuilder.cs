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
        private Dictionary<Type, InstantiationCacheEntry> cache = new Dictionary<Type, InstantiationCacheEntry>();

        /// <summary>
        /// Initializes a new instance of the instance builder
        /// </summary>
        /// <param name="container"></param>
        public InstanceBuilder(IActivates container)
        {
            this.container = container;

            this.container.BindingChanged += (x, y) => cache.Clear();
        }

        /// <summary>
        /// Creates an instance of the object
        /// </summary>
        /// <typeparam name="T">Type of the object to be requested</typeparam>
        /// <returns>Created instance</returns>
        public T Create<T>()
        {
            InstantiationCacheEntry entry;
            if (this.cache.TryGetValue(typeof(T), out entry))
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
                this.AddPropertyAssignments(typeof(T), result, containerExpression, expressions);

                // return result;
                expressions.Add(result);

                var expression = Expression.Block(
                    // var result;
                    new[] { result },
                    expressions);                

                // Creates cache entry containing the compiled method
                entry = new InstantiationCacheEntry();
                entry.FactoryMethod = Expression.Lambda<Func<IActivates, object>>(expression, containerExpression).Compile();

                // Store in dictionary
                this.cache[typeof(T)] = entry;

                // Call!
                return (T)entry.FactoryMethod(this.container);
            }
        }

        /// <summary>
        /// Adds property assignements to the list of expression. 
        /// The Assignments are found by recursive visiting of all properties of the type
        /// </summary>
        /// <param name="type">Type of the instance 'result', whose properties will be visited</param>
        /// <param name="target">Object, where properties shall be assigned to</param>
        /// <param name="containerExpression">Expression containing the container</param>
        /// <param name="expressions">Expressions, where assignments shall be added</param>
        private void AddPropertyAssignments(Type type, Expression target, ParameterExpression containerExpression, List<Expression> expressions)
        {
            foreach (var property in type.GetProperties(BindingFlags.SetField | BindingFlags.Instance | BindingFlags.Public))
            {
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
                    enablers.Add(new ByNameEnabler(byNameAttribute.Name));
                }
                else
                {
                    enablerCreation =
                        Expression.New(
                            typeof(Enabler.ByTypeEnabler).GetConstructor(new[] { typeof(Type) }),
                            Expression.Constant(property.PropertyType));
                    enablers.Add(new ByTypeEnabler(property.PropertyType));
                }

                // Check, if we have a binding
                if (!this.container.Has(enablers))
                {
                    // I do not know this type
                    continue;
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
                        memberAccess,
                        Expression.Convert(
                            Expression.Call(
                                containerExpression,
                                getMethod,
                                parameters),
                            property.PropertyType)));

                // Add properties of embedded object
                this.AddPropertyAssignments(property.PropertyType, memberAccess, containerExpression, expressions);
            }
        }
    }
}
