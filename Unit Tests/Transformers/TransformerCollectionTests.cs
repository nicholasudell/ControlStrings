using AutoFixture;
using AutoFixture.NUnit3;
using ControlStrings;
using Moq;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;

namespace ControlStrings.UnitTests.Transformers
{
    [TestFixture]
    public class TransformerCollectionTests
    {
        [Test, AutoMoqData]
        public void Matches_IfAnyTransformerMatches_ReturnsTrue(ITransformer transformer1, ITransformer transformer2, string transformCode)
        {
            Mock.Get(transformer1).Setup(x => x.Matches(transformCode)).Returns(true);
            Mock.Get(transformer2).Setup(x => x.Matches(transformCode)).Returns(false);

            var fixture = new Fixture();

            fixture.Customizations.Add(new InlineConstructorParams<TransformerCollection>((object)new[] { transformer1, transformer2 }));

            var unit = fixture.Create<TransformerCollection>();

            unit.Matches(transformCode).ShouldBeTrue();
        }

        [Test, AutoMoqData]
        public void Matches_IfTransformCodeIsNull_ThrowsArgumentNullException(TransformerCollection unit)
        {
            Should.Throw<ArgumentNullException>(() => unit.Matches(null));
        }

        [Test, AutoMoqData]
        public void Transform_IfTransformCodeIsNull_ThrowsArgumentNullException(TransformerCollection unit, string input)
        {
            Should.Throw<ArgumentNullException>(() => unit.Transform(null, input));
        }

        [Test, AutoMoqData]
        public void Transform_IfInputIsNull_ThrowsArgumentNullException(TransformerCollection unit, string transformCode)
        {
            Should.Throw<ArgumentNullException>(() => unit.Transform(transformCode, null));
        }

        [Test, AutoMoqData]
        public void Matches_IfNoTransformerMatches_ReturnsFalse(ITransformer transformer1, ITransformer transformer2, string transformCode)
        {
            Mock.Get(transformer1).Setup(x => x.Matches(transformCode)).Returns(false);
            Mock.Get(transformer2).Setup(x => x.Matches(transformCode)).Returns(false);

            var fixture = new Fixture();

            fixture.Customizations.Add(new InlineConstructorParams<TransformerCollection>((object)new[] { transformer1, transformer2 }));

            var unit = fixture.Create<TransformerCollection>();

            unit.Matches(transformCode).ShouldBeFalse();
        }

        [Test, AutoMoqData]
        public void Transform_IfNoTransformerMatches_ThrowsMissingTransformerException(ITransformer transformer1, ITransformer transformer2, string transformCode, string input)
        {
            Mock.Get(transformer1).Setup(x => x.Matches(transformCode)).Returns(false);
            Mock.Get(transformer2).Setup(x => x.Matches(transformCode)).Returns(false);

            var fixture = new Fixture();

            fixture.Customizations.Add(new InlineConstructorParams<TransformerCollection>((object)new[] { transformer1, transformer2 }));

            var unit = fixture.Create<TransformerCollection>();

            Should.Throw<MissingTransformerException>(()=>unit.Transform(transformCode, input));
        }

        [Test, AutoMoqData]
        public void Transform_IfMultipleTransformersMatch_TransformsWithTheFirstTransformer(ITransformer transformer1, ITransformer transformer2, string transformCode, string input)
        {
            Mock.Get(transformer1).Setup(x => x.Matches(transformCode)).Returns(true);
            Mock.Get(transformer1).Setup(x => x.Transform(transformCode, input)).Returns("foo");
            Mock.Get(transformer2).Setup(x => x.Matches(transformCode)).Returns(true);
            Mock.Get(transformer2).Setup(x => x.Transform(transformCode, input)).Returns("bar");

            var fixture = new Fixture();

            fixture.Customizations.Add(new InlineConstructorParams<TransformerCollection>((object)new[] { transformer1, transformer2 }));

            var unit = fixture.Create<TransformerCollection>();

            unit.Transform(transformCode, input).ShouldBe("foo");
        }

    }
}
