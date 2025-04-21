using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Autopark.Common.Helpers;

public static class ReflectionHelper
{
    /// <summary>
    ///     Общие флаги поиска
    /// </summary>
    public const BindingFlags CommonFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

    public static readonly JsonSerializerSettings ClonningSettings =
        new JsonSerializerSettings
        {
            DefaultValueHandling = DefaultValueHandling.Ignore,
            NullValueHandling = NullValueHandling.Ignore,
            Formatting = Formatting.None,
            Culture = CultureInfo.InvariantCulture,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple,
            TypeNameHandling = TypeNameHandling.Auto,
            ContractResolver = new DefaultContractResolver { IgnoreSerializableAttribute = true }
        };
    
    public static bool HasCustomAttribute<T>(this MemberInfo member)
        where T : Attribute
    {
        return member.GetCustomAttribute<T>() != null;
    }
    
    public static TObject Clone<TObject>(TObject obj)
    {
        return Clone(obj, null);
    }

    public static TObject Clone<TObject>(TObject obj, Type objType)
    {
        var ser = JsonConvert.SerializeObject(obj, ClonningSettings);
        if (objType != null)
        {
            return (TObject)JsonConvert.DeserializeObject(ser, objType, ClonningSettings);
        }

        return JsonConvert.DeserializeObject<TObject>(ser, ClonningSettings);
    }

    /// <summary>
    ///     Получает информацию о члене класса по выражению
    /// </summary>
    /// <param name="target">
    ///     The target.
    /// </param>
    /// <param name="func">
    ///     The func.
    /// </param>
    /// <typeparam name="TObject">
    ///     Тип объекта
    /// </typeparam>
    /// <typeparam name="TMember">
    ///     Тип свойства/поля и т.п.
    /// </typeparam>
    /// <returns>
    ///     информация о члене класса
    /// </returns>
    public static MemberInfo GetMemberInfo<TObject, TMember>(
        this TObject target,
        Expression<Func<TObject, TMember>> func)
    {
        var res = func.Body is UnaryExpression unary ? unary.Operand : func.Body;

        if (res is MemberExpression me)
        {
            return me.Member;
        }

        if (res is MethodCallExpression mc)
        {
            return mc.Method;
        }

        throw new ArgumentException(
            "Невалидное выражение. Поддерживается только ображение к свойству и вызов метода.",
            nameof(func));
    }
    
    public static PropertyInfo GetPropertyInfo<TObject, TProperty>(Expression<Func<TObject, TProperty>> func)
    {
        var obj = default(TObject);
        var prop = GetMemberInfo(obj, func) as PropertyInfo;
        if (prop == null)
        {
            throw new InvalidCastException($"Expression '{func}' is not a property.");
        }

        return prop;
    }

    /// <summary>
    ///     Проверка, может ли тип быть  null
    /// </summary>
    /// <param name="type">Тип, который проверяем</param>
    /// <returns>Результат проверки</returns>
    public static bool IsNullable(this Type type)
    {
        return !type.IsValueType || Nullable.GetUnderlyingType(type) != null;
    }
    
    public static object CallGenericMethod(
        object instance,
        string methodName,
        Type genericMethodArgs,
        params object[] args)
    {
        var mi = GetMethodInfo(instance.NotNull(nameof(instance)).GetType(),
            methodName,
            CommonFlags | BindingFlags.Static,
            true,
            args?.Length ?? -1);
        mi = mi.MakeGenericMethod(genericMethodArgs);
        return mi.Invoke(instance, args);
    }

    /// <summary>
    ///     Получает инфу о методе
    /// </summary>
    /// <param name="type">The type.</param>
    /// <param name="methodName">The method name.</param>
    /// <param name="flags">The flags.</param>
    /// <param name="generic">The generic.</param>
    /// <param name="parametersCount">The parametersCount.</param>
    /// <returns>MethodInfo</returns>
    private static MethodInfo GetMethodInfo(
        Type type,
        string methodName,
        BindingFlags flags,
        bool generic = false,
        int parametersCount = -1)
    {
        var candidates = type.GetMethods(flags).Where(
                a => a.Name == methodName && a.IsGenericMethodDefinition == generic
                                          && (parametersCount == -1 || a.GetParameters().Length ==
                                              parametersCount))
            .ToArray();
        if (candidates.Length == 0)
        {
            throw new MissingMethodException(type.FullName, methodName);
        }

        if (candidates.Length > 1)
        {
            throw new AmbiguousMatchException($"Ambigous methods {methodName} found in type {type.FullName}");
        }

        var pi = candidates.Single();

        return pi;
    }
    
    public static Type GetGenericElementType(this Type type)
    {
        // Short-circuit for Array types
        if (typeof(Array).IsAssignableFrom(type))
        {
            return type.GetElementType();
        }

        while (true)
        {
            // Type is IEnumerable<T>
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                return type.GetGenericArguments().First();
            }

            // Type implements/extends IEnumerable<T>
            var elementType = (from subType in type.GetInterfaces()
                let retType = subType.GetGenericElementType()
                where retType != subType
                select retType).FirstOrDefault();

            if (elementType != null)
            {
                return elementType;
            }

            if (type.BaseType == null)
            {
                return type;
            }

            type = type.BaseType;
        }
    }

    public static bool IsSubclassOfOpenGeneric(this Type toCheck, Type generic)
    {
        return GetClosedOfOpenGeneric(toCheck, generic) != null;
    }
    
    public static bool IsSubclassOfOpenGenericInterface(this Type checkType, Type baseType)
    {
        return checkType.GetSubclassOfOpenGenericInterface(baseType) != null;
    }

    public static Type GetClosedOfOpenGeneric(this Type toCheck, Type generic)
    {
        while (toCheck != null && toCheck != typeof(object))
        {
            var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
            if (generic == cur)
            {
                return toCheck;
            }

            toCheck = toCheck.BaseType;
        }

        return null;
    }

    public static Type GetSubclassOfOpenGenericInterface(this Type checkType, Type baseType)
    {
        var checkTypes = checkType.GetInterfaces();

        foreach (var type in checkTypes)
        {
            var cur = type.IsGenericType ? type.GetGenericTypeDefinition() : type;
            if (baseType == cur)
            {
                return type;
            }

            var sub = GetSubclassOfOpenGenericInterface(type, baseType);

            if (sub != null)
            {
                return sub;
            }
        }

        return null;
    }

    public static IEnumerable<Type> GetImmediateInterfaces(this Type type)
    {
        var interfaces = type.GetInterfaces();
        var result = new HashSet<Type>(interfaces);
        foreach (var i in interfaces)
        {
            result.ExceptWith(i.GetInterfaces());
        }

        return result;
    }

    public static void LoadAssembliesFromApplicationDirectory(string pattern)
    {
        var directoryUri = new Uri(typeof(ReflectionHelper).Assembly.CodeBase);
        var directoryPath = Path.GetDirectoryName(Uri.UnescapeDataString(directoryUri.AbsolutePath));

        LoadAssembliesFromDirectory(directoryPath, pattern);
    }
    
    public static void LoadAssembliesFromDirectory(string directoryPath, string pattern)
    {
        if (!Directory.Exists(directoryPath))
        {
            return;
        }

        foreach (var path in Directory.GetFiles(directoryPath, pattern))
        {
            var asmName = AssemblyName.GetAssemblyName(path);
            if (!AppDomain.CurrentDomain.GetAssemblies().Any(a => a.GetName() == asmName))
            {
                try
                {
                    AppDomain.CurrentDomain.Load(asmName);
                }
                catch (FileNotFoundException e)
                {
                    Console.WriteLine(e.Message);
                    // do nothing
                }
            }
        }
    }
}
