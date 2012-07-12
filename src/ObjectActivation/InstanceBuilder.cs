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

                foreach (var property in typeof(T).GetProperties(BindingFlags.SetField | BindingFlags.Instance | BindingFlags.Public))
                {
                    var byName = property.GetCustomAttributes(typeof(ByNameAttribute), false);
                    NewExpression enablerCreation;

                    if (byName != null && byName.Length != 0)
                    {
                        enablerCreation =
                            Expression.New(
                                typeof(Enabler.ByNameEnabler).GetConstructor(new[] { typeof(string) }),
                                Expression.Constant((byName[0] as ByNameAttribute).Name));
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

                    // result.{property} = {this.container}.Get({parameters});
                    expressions.Add(
                        Expression.Assign(
                            Expression.MakeMemberAccess(
                                result,
                                property),
                            Expression.Convert(
                                Expression.Call(
                                    containerExpression,
                                    getMethod,
                                    parameters),
                                property.PropertyType)));
                }

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
    }
}
