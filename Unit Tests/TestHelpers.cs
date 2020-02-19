using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Kernel;
using AutoFixture.NUnit3;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ControlStrings.UnitTests
{
    public class CustomConstructorNamedParameterSpecimenBuilder<TClass, TParam> : ISpecimenBuilder
    {
        private readonly TParam value;
        private readonly string parameterName;

        public CustomConstructorNamedParameterSpecimenBuilder(TParam value, string parameterName)
        {
            this.value = value;
            this.parameterName = parameterName;
        }

        public object Create(object request, ISpecimenContext context)
        {
            if (!(request is ParameterInfo parameterInfo) ||
                parameterInfo.Member.DeclaringType != typeof(TClass) ||
                parameterInfo.ParameterType != typeof(TParam) ||
                !parameterInfo.Name.Equals(parameterName, StringComparison.Ordinal))
            {
                return new NoSpecimen();
            }

            return value;
        }
    }

    public class InlineConstructorParams<TClass> : ISpecimenBuilder
    {
        private readonly object[] parameters;

        public InlineConstructorParams(params object[] parameters)
        {
            this.parameters = parameters;
        }

        public object Create(object request, ISpecimenContext context)
        {
            var parameterInfo = request as ParameterInfo;

            if (parameterInfo is null ||
                parameterInfo.Member.DeclaringType != typeof(TClass) ||
                parameterInfo.Position >= parameters.Length ||
                !parameterInfo.ParameterType.IsAssignableFrom(parameters[parameterInfo.Position].GetType()))
            {
                return new NoSpecimen();
            }

            return parameters[parameterInfo.Position];
        }
    }

    internal class AutoMoqDataAttribute : AutoDataAttribute
    {
        internal AutoMoqDataAttribute()
            : base(()=> new Fixture().Customize(new AutoMoqCustomization()))
        {
        }
    }
}
