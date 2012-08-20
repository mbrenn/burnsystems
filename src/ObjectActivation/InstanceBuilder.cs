using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;
using BurnSystems.ObjectActivation.Enabler;
using BurnSystems.Test;

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
        /// Stores the Method Info for System.Enumerable.FirstOrDefault(IEnumerable source)
        /// </summary>
        private static MethodInfo firstOrDefaultMethod;

        /// <summary>
        /// Gets the Method Info for System.Enumerable.FirstOrDefault(IEnumerable source)
        /// </summary>
        public static MethodInfo FirstOrDefaultMethod
        {
            get { return firstOrDefaultMethod; }
        }

        /// <summary>
        /// Static Constructor
        /// </summary>
        static InstanceBuilder()
        {
            var type = typeof(System.Linq.Enumerable);

            // Try to find the method FirstOrDefault<TSource>(IEnumerable<TSource> source)
            var methodInfos = type.GetMethods().Where(x => x.Name == "FirstOrDefault" && x.GetParameters().Length == 1).ToList();
            Ensure.That(methodInfos.Count == 1);
            var methodInfo = methodInfos[0];
            firstOrDefaultMethod = methodInfo.MakeGenericMethod(new[] { typeof(object) });
        }

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
            return (T) this.Create(typeof(T));
        }

        /// <summary>
        /// Creates an instance of a specific type
        /// </summary>
        /// <param name="type">Type being created</param>
        /// <returns>Object being created</returns>
        internal object Create(Type type)
        {
            InstantiationCacheEntry entry;
            if (cache.TryGetValue(type, out entry))
            {
                return entry.FactoryMethod(this.container);
            }
            else
            {
                // var result;
                var result = Expression.Parameter(type, "result");
                var containerExpression = Expression.Parameter(typeof(IActivates), "container");
                var expressions = new List<Expression>();

                // Check, if we have an object with inject attribute
                var injectAttributeConstructor = type
                    .GetConstructors(BindingFlags.Instance | BindingFlags.Public)
                    .Where(x => x.GetCustomAttributes(typeof(InjectAttribute), false).Count() > 0)
                    .SingleOrDefault();

                if (injectAttributeConstructor == null)
                {
                    // var result;
                    // result = new {typeof(T)}();
                    expressions.Add(Expression.Assign(result, Expression.New(type)));
                }
                else
                {
                    // Go through all parameter and create necessary information
                    var constructorParameters = new List<Expression>();
                    foreach (var parameter in injectAttributeConstructor.GetParameters())
                    {
                        constructorParameters.Add(this.QueryContainerMethod(
                            containerExpression,
                            parameter.ParameterType,
                            parameter.GetCustomAttributes(typeof(InjectAttribute), false).Cast<InjectAttribute>().FirstOrDefault()));
                    }

                    expressions.Add(Expression.Assign(result, Expression.New(injectAttributeConstructor, constructorParameters.ToArray())));
                }

                //
                // Assigns the properties
                //
                var variables = this.AddPropertyAssignments(type, result, containerExpression, expressions);

                // return result;
                expressions.Add(result);
                variables.Add(result);

                // Creates block with variables and the expression tree
                var expression = Expression.Block(
                    variables,
                    expressions);                

                // Creates cache entry containing the compiled method
                entry = new InstantiationCacheEntry();
                entry.FactoryMethod = Expression.Lambda<Func<IActivates, object>>(expression, containerExpression).Compile();
                entry.Expression = expression;

                // Store in dictionary
                cache[type] = entry;

                // Call!
                return entry.FactoryMethod(this.container);
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

                // Check, if assignment by Name shall be executed, otherwise by type
                var inject = property.GetCustomAttributes(typeof(InjectAttribute), false);                
                var enablers = new List<IEnabler>();

                foreach (var injectAttribute in inject.Cast<InjectAttribute>())
                {
                    var containerQuery = this.QueryContainerMethod(containerExpression, property.PropertyType, injectAttribute);

                    // {tempVariable} = {ContainerQuery}
                    expressions.Add(
                        Expression.Assign(
                            tempVariable,
                            containerQuery));


                    // Performs the following action, if tempVariable is not null
                    var memberAccess = Expression.MakeMemberAccess(target, property);
                    var conditionalStatements = new List<Expression>();
                    // {result}.{property} = {tempVariable}
                    conditionalStatements.Add(
                        Expression.Assign(
                        memberAccess,
                        tempVariable));

                    // if({tempVariable} != null) { /* New Block with {result}.{property}. */; {result}.{property} = {tempVariable} };
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
            }

            return result;
        }

        /// <summary>
        /// Creates an expression for a certain object by using the injectAttribute
        /// </summary>
        /// <param name="containerExpression">Container storing the objects</param>
        /// <param name="type">Type of the object</param>
        /// <param name="injectAttribute">Inject Attribute defining more information</param>
        /// <returns>Expression storing the retrieved object</returns>
        private UnaryExpression QueryContainerMethod(ParameterExpression containerExpression, Type type, InjectAttribute injectAttribute)
        {
            NewExpression enablerCreation;

            if (injectAttribute == null || string.IsNullOrEmpty(injectAttribute.ByName))
            {
                enablerCreation =
                    Expression.New(
                        typeof(Enabler.ByTypeEnabler).GetConstructor(new[] { typeof(Type) }),
                        Expression.Constant(type));
            }
            else
            {
                enablerCreation =
                    Expression.New(
                        typeof(Enabler.ByNameEnabler).GetConstructor(new[] { typeof(string) }),
                        Expression.Constant(injectAttribute.ByName));
            }

            // OK, we found it, add expression
            // var {parameters} = new Enabler.ByTypeEnabler [] { new ByTypeEnabler({typeof(property)}); }
            var parameters = Expression.NewArrayInit(
                typeof(IEnabler),
                enablerCreation);

            // {tempVariable} = Cast<{PropertyType}>({this.container}.Get({parameters}).FirstOrDefault());
            var getMethod = typeof(IActivates).GetMethod("GetAll");

            // {ContainerQuery} = Cast<{PropertyType}>({this.container}.Get({parameters}).FirstOrDefault());
            var containerQuery = Expression.Convert(
                    Expression.Call(
                        firstOrDefaultMethod,
                        Expression.Call(
                            containerExpression,
                            getMethod,
                            parameters)),
                    type);

            return containerQuery;
        }
    }
}
