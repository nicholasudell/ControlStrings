using ControlStrings;
using NUnit.Framework;
using Shouldly;
using System;

namespace ControlStrings.UnitTests.Transformers
{
    [TestFixture]
    public class NullTransformerTests
    {
        [Test, AutoMoqData]
        public void Matches_ReturnsTrue(NullTransformer unit, string transformCode) => 
            unit.Matches(transformCode).ShouldBeTrue();


        [Test, AutoMoqData]
        public void Transform_ReturnsInput(NullTransformer unit, string transformCode,string input) => 
            unit.Transform(transformCode, input).ShouldBe(input);
    }
}
