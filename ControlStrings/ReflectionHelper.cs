using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace ControlStrings
{
    public static class ReflectionHelper
    {
        public static IEnumerable<IControlStringMatcher> GenerateValueMatchersFor<T>(T obj)
        {
            var typeInfo = typeof(T)
                .GetTypeInfo();

            return typeInfo
                .DeclaredProperties
                .Where(x => x.CanRead && x.GetMethod.IsPublic && x.PropertyType.Equals(typeof(string)))
                .Select(x => x.Name)
                .Concat(typeInfo
                        .DeclaredFields
                        .Where(x => x.IsPublic && x.FieldType.Equals(typeof(string)))
                        .Select(x => x.Name))
                .Select(x => new ValueControlStringMatcher(x, ConvertToGetterLambda<T>(x)(obj)));
        }

        private static Func<T, Func<string>> ConvertToGetterLambda<T>(string propertyOrFieldName)
        {
            var parameter = Expression.Parameter(typeof(T));
            var propExp = Expression.PropertyOrField(parameter, propertyOrFieldName);
            var conversion = Expression.Convert(propExp, typeof(string));
            var innerLambda = Expression.Lambda<Func<string>>(conversion);
            var lambda = Expression.Lambda<Func<T, Func<string>>>(innerLambda, parameter);

            return lambda.Compile();
        }
    }
}
