using AutoFixture;
using AutoFixture.NUnit3;
using ControlStrings;
using Moq;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;

namespace ControlStrings.UnitTests
{
    [TestFixture]
    public class ParserTests
    {
        [Test, AutoMoqData]
        public void Parse_WhenInputIsNull_ThrowsArgumentNullException(Parser unit)
        {
            Should.Throw<ArgumentNullException>(() => unit.Parse(null));
        }

        [Test, AutoMoqData]
        public void Parse_ReplacesControlStringsInInputWithMatcherOutput([Frozen] IControlStringFinder finder, [Frozen] IControlStringMatcher matcher, Parser unit)
        {
            var fixture = new Fixture();

            fixture.Customizations.Add(new InlineConstructorParams<ControlString>(0, 5, new Queue<string>(new[] { "foo" })));

            var controlString = fixture.Create<ControlString>();

            Mock.Get(finder).Setup(x => x.FindAllControlStrings(It.IsAny<string>())).Returns(new[] { controlString });

            Mock.Get(matcher).Setup(x => x.Matches(It.IsAny<ControlString>())).Returns(true);
            Mock.Get(matcher).Setup(x => x.Match(It.IsAny<ControlString>())).Returns("bar");

            var actual = unit.Parse("{foo}");

            actual.ShouldBe("bar");
        }

        [Test, AutoMoqData]
        public void Parse_WhenTransformerSyntaxIsUsed_TransformsMatchedControlStrings([Frozen] IControlStringFinder finder, [Frozen] IControlStringMatcher matcher, [Frozen] ITransformer transformer, Parser unit)
        {
            var fixture = new Fixture();

            fixture.Customizations.Add(new InlineConstructorParams<ControlString>(0, 9, new Queue<string>(new[] { "foo" }), new Queue<string>(new[]{ "baz" })));

            var controlString = fixture.Create<ControlString>();

            Mock.Get(finder).Setup(x => x.FindAllControlStrings(It.IsAny<string>())).Returns(new[] { controlString });

            Mock.Get(matcher).Setup(x => x.Matches(It.IsAny<ControlString>())).Returns(true);
            Mock.Get(matcher).Setup(x => x.Match(It.IsAny<ControlString>())).Returns("bar");

            Mock.Get(transformer).Setup(x => x.Transform(It.IsAny<string>(), It.IsAny<string>())).Returns("bash");
            Mock.Get(transformer).Setup(x => x.Matches(It.IsAny<string>())).Returns(true);

            var actual = unit.Parse("{foo|baz}");

            actual.ShouldBe("bash");
        }

        [Test, AutoMoqData]
        public void Parse_WhenMatchingTransformerCannotBeFound_ThrowsMissingTransformerException([Frozen] IControlStringFinder finder, [Frozen] IControlStringMatcher matcher, [Frozen] ITransformer transformer, Parser unit)
        {
            var fixture = new Fixture();

            fixture.Customizations.Add(new InlineConstructorParams<ControlString>(0, 9, new Queue<string>(new[] { "foo" }), new Queue<string>(new[] { "baz" })));

            var controlString = fixture.Create<ControlString>();

            Mock.Get(finder).Setup(x => x.FindAllControlStrings(It.IsAny<string>())).Returns(new[] { controlString });

            Mock.Get(matcher).Setup(x => x.Matches(It.IsAny<ControlString>())).Returns(true);
            Mock.Get(matcher).Setup(x => x.Match(It.IsAny<ControlString>())).Returns("bar");

            Mock.Get(transformer).Setup(x => x.Matches(It.IsAny<string>())).Returns(false);

            Should.Throw<MissingTransformerException>(()=> unit.Parse("{foo|baz}"));
        }
    }
}
