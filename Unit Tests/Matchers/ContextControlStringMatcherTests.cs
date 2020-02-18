using AutoFixture.NUnit3;
using NUnit.Framework;
using Shouldly;
using System;
using Moq;

namespace ControlStrings.UnitTests.Matchers
{
    [TestFixture]
    public class ContextControlStringMatcherTests
    {
        [Test, AutoMoqData]
        public void Match_WhenFirstValueIsContext_ExecutesSecondControlStringMatcher([Frozen]IControlStringMatcher submatcher, ContextControlStringMatcher unit)
        {
            Mock.Get(submatcher).Setup(x => x.Matches(It.IsAny<ControlString>())).Returns(true);

            unit.Context = "foo";

            var expected = "bar";

            var controlString = new ControlString(0, 3, new System.Collections.Generic.Queue<string>(new[] { unit.Context, expected }));

            unit.Match(controlString);

            Mock.Get(unit.Matcher).Verify(x => x.Match(It.Is<ControlString>(y => y.Values.Peek() == expected)));
        }

        [Test, AutoMoqData]
        public void Matches_WhenFirstValueIsContextAndSubMatcherMatches_ReturnsTrue([Frozen]IControlStringMatcher submatcher, ContextControlStringMatcher unit)
        {
            Mock.Get(submatcher).Setup(x => x.Matches(It.IsAny<ControlString>())).Returns(true);

            unit.Context = "foo";

            var controlString = new ControlString(0,3,new System.Collections.Generic.Queue<string>(new[] { unit.Context }));

            unit.Matches(controlString).ShouldBeTrue();
        }

        [Test, AutoMoqData]
        public void Matches_WhenFirstValueIsContextAndSubMatcherDoesNotMatch_ReturnsFalse([Frozen]IControlStringMatcher submatcher, ContextControlStringMatcher unit)
        {
            Mock.Get(submatcher).Setup(x => x.Matches(It.IsAny<ControlString>())).Returns(false);

            unit.Context = "foo";

            var controlString = new ControlString(0, 3, new System.Collections.Generic.Queue<string>(new[] { unit.Context }));

            unit.Matches(controlString).ShouldBeFalse();
        }

        [Test, AutoMoqData]
        public void Matches_WhenControlStringIsNull_ThrowsArgumentNullException(ContextControlStringMatcher unit)
        {
            Should.Throw<ArgumentNullException>(() => unit.Matches(null));
        }

        [Test, AutoMoqData]
        public void Match_WhenControlStringIsNull_ThrowsArgumentNullException(ContextControlStringMatcher unit)
        {
            Should.Throw<ArgumentNullException>(() => unit.Match(null));
        }

        [Test, AutoMoqData]
        public void Matches_WhenFirstValueIsNotContext_ReturnsFalse(ContextControlStringMatcher unit)
        {
            unit.Context = "foo";
            var controlString = new ControlString(0, 3, new System.Collections.Generic.Queue<string>(new[] { "bar" }));

            unit.Matches(controlString).ShouldBeFalse();
        }

        [Test, AutoMoqData]
        public void Match_WhenFirstValueIsNotContext_ThrowsInvalidOperationException(ContextControlStringMatcher unit)
        {
            unit.Context = "foo";
            var controlString = new ControlString(0, 3, new System.Collections.Generic.Queue<string>(new[] { "bar" }));

            Should.Throw<ArgumentException>(()=>unit.Match(controlString));
        }
    }
}
