using AutoFixture;
using AutoFixture.NUnit3;
using ControlStrings;
using Moq;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;

namespace ControlStrings.UnitTests.Matchers
{
    [TestFixture]
    public class ControlStringMatcherCollectionTests
    {
        [Test, AutoMoqData]
        public void Match_WhenControlStringIsNull_ThrowsArgumentNullException(ControlStringMatcherCollection unit)
        {
            Should.Throw<ArgumentNullException>(() => unit.Match(null));
        }

        [Test, AutoMoqData]
        public void Matches_WhenControlStringIsNull_ThrowsArgumentNullException(ControlStringMatcherCollection unit)
        {
            Should.Throw<ArgumentNullException>(() => unit.Matches(null));
        }

        [Test, AutoMoqData]
        public void Match_WhenNoInternalMatchersMatch_ThrowsArgumentException([Frozen] IControlStringMatcher internalMatcher1, [Frozen] IControlStringMatcher internalMatcher2)
        {
            const string controlStringValue = "foo";

            Mock.Get(internalMatcher1).Setup(x => x.Matches(It.IsAny<ControlString>())).Returns(false);
            Mock.Get(internalMatcher2).Setup(x => x.Matches(It.IsAny<ControlString>())).Returns(false);

            var fixture = new Fixture();

            fixture.Customizations.Add(new InlineConstructorParams<ControlString>(0, 3, new Queue<string>(new[] { controlStringValue })));
            fixture.Customizations.Add(new InlineConstructorParams<ControlStringMatcherCollection>((object)new[] { internalMatcher1, internalMatcher2 }));

            var controlString = fixture.Create<ControlString>();

            var unit = fixture.Create<ControlStringMatcherCollection>();

            Should.Throw<ArgumentException>(()=>unit.Match(controlString));
        }

        [Test, AutoMoqData]
        public void Match_SkipsNonMatchingInternalMatchers([Frozen] IControlStringMatcher internalMatcher1, [Frozen] IControlStringMatcher internalMatcher2)
        {
            const string controlStringValue = "foo";

            Mock.Get(internalMatcher1).Setup(x => x.Matches(It.IsAny<ControlString>())).Returns(false);

            var internalMatcher2Mock = Mock.Get(internalMatcher2);
            internalMatcher2Mock.Setup(x => x.Matches(It.IsAny<ControlString>())).Returns(true);

            var fixture = new Fixture();

            fixture.Customizations.Add(new InlineConstructorParams<ControlString>(0, 3, new Queue<string>(new[] { controlStringValue })));
            fixture.Customizations.Add(new InlineConstructorParams<ControlStringMatcherCollection>((object)new[] { internalMatcher1, internalMatcher2 }));

            var controlString = fixture.Create<ControlString>();

            var unit = fixture.Create<ControlStringMatcherCollection>();

            unit.Match(controlString);

            internalMatcher2Mock.Verify(x => x.Match(It.IsAny<ControlString>()));
        }

        [Test, AutoMoqData]
        public void Match_PassesToFirstMatchingInternalMatcher([Frozen] IControlStringMatcher internalMatcher1, [Frozen] IControlStringMatcher internalMatcher2)
        {
            const string controlStringValue = "foo";

            var internalMatcher1Mock = Mock.Get(internalMatcher1);
            internalMatcher1Mock.Setup(x => x.Matches(It.IsAny<ControlString>())).Returns(false);
            Mock.Get(internalMatcher2).Setup(x => x.Matches(It.IsAny<ControlString>())).Returns(true);

            var fixture = new Fixture();

            fixture.Customizations.Add(new InlineConstructorParams<ControlString>(0, 3, new Queue<string>(new[] { controlStringValue })));
            fixture.Customizations.Add(new InlineConstructorParams<ControlStringMatcherCollection>((object)new[] { internalMatcher1, internalMatcher2 }));

            var controlString = fixture.Create<ControlString>();

            var unit = fixture.Create<ControlStringMatcherCollection>();

            unit.Match(controlString);

            internalMatcher1Mock.Verify(x => x.Match(It.IsAny<ControlString>()));
        }

        [Test, AutoMoqData]
        public void Matches_WhenAnyInternalMatchersMatch_ReturnsTrue([Frozen] IControlStringMatcher internalMatcher1, [Frozen] IControlStringMatcher internalMatcher2) 
        {
            const string controlStringValue = "foo";

            Mock.Get(internalMatcher1).Setup(x => x.Matches(It.IsAny<ControlString>())).Returns(false);
            Mock.Get(internalMatcher2).Setup(x => x.Matches(It.IsAny<ControlString>())).Returns(true);

            var fixture = new Fixture();

            fixture.Customizations.Add(new InlineConstructorParams<ControlString>(0, 3, new Queue<string>(new[] { controlStringValue })));
            fixture.Customizations.Add(new InlineConstructorParams<ControlStringMatcherCollection>((object)new[] { internalMatcher1, internalMatcher2 }));

            var controlString = fixture.Create<ControlString>();

            var unit = fixture.Create<ControlStringMatcherCollection>();

            unit.Matches(controlString).ShouldBeTrue();
        }

        [Test, AutoMoqData]
        public void Matches_WhenAllInternalMatcherDoesNotMatch_ReturnsFalse([Frozen] IControlStringMatcher internalMatcher1, [Frozen] IControlStringMatcher internalMatcher2)
        {
            const string controlStringValue = "foo";

            Mock.Get(internalMatcher1).Setup(x => x.Matches(It.IsAny<ControlString>())).Returns(false);
            Mock.Get(internalMatcher2).Setup(x => x.Matches(It.IsAny<ControlString>())).Returns(false);

            var fixture = new Fixture();

            fixture.Customizations.Add(new InlineConstructorParams<ControlString>(0, 3, new Queue<string>(new[] { controlStringValue })));
            fixture.Customizations.Add(new InlineConstructorParams<ControlStringMatcherCollection>((object)new[] { internalMatcher1, internalMatcher2 }));

            var controlString = fixture.Create<ControlString>();

            var unit = fixture.Create<ControlStringMatcherCollection>();

            unit.Matches(controlString).ShouldBeFalse();
        }
    }
}
