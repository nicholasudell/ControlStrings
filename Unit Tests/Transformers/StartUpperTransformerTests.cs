using AutoFixture;
using ControlStrings;
using NUnit.Framework;
using Shouldly;
using System;

namespace ControlStrings.UnitTests.Transformers
{
    [TestFixture]
    public class StartUpperTransformerTests
    {
        [Test, AutoMoqData]
        public void Matches_WhentransformCodeIsStartUpper_ReturnsTrue(StartUpperTransformer unit)
        {
            unit.Matches("StartUpper").ShouldBeTrue();
        }

        [Test, AutoMoqData]
        public void Matches_WhentransformCodeIsNull_ThrowsArgumentNullException(StartUpperTransformer unit)
        {
            Should.Throw<ArgumentNullException>(()=>unit.Matches(null));
        }

        [Test, AutoMoqData]
        public void Matches_WhentransformCodeIsNotStartUpper_ReturnsFalse(StartUpperTransformer unit, string transformCode)
        {
            unit.Matches(transformCode).ShouldBeFalse();
        }

        [Test, AutoMoqData]
        public void Transform_ConvertsFirstCharacterToUpperCase(StartUpperTransformer unit, string transformCode)
        {
            unit.Transform(transformCode, "foo").ShouldBe("Foo");
        }

        [Test, AutoMoqData]
        public void Transform_WhenInputIsNull_ThrowsArgumentNullException(StartUpperTransformer unit, string transformCode)
        {
            Should.Throw<ArgumentNullException>(() => unit.Transform(transformCode, null));
        }

        [Test, AutoMoqData]
        public void Transform_WhenInputIsEmptyString_ReturnsEmptyString(StartUpperTransformer unit, string transformCode)
        {
            unit.Transform(transformCode, string.Empty).ShouldBe(string.Empty);
        }

        [Test, AutoMoqData]
        public void Transform_WhenInputStartsWithNonAlphabetCharacter_ReturnsInput(StartUpperTransformer unit, string transformCode)
        {
            unit.Transform(transformCode, "1foo").ShouldBe("1foo");
        }

        [Test, AutoMoqData]
        public void Transform_WhenInputIsSingleCharacter_CapitalisesIt(StartUpperTransformer unit, string transformCode)
        {
            unit.Transform(transformCode, "a").ShouldBe("A");
        }
    }
}
