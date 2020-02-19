using AutoFixture;
using AutoFixture.NUnit3;
using ControlStrings;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ControlStrings.UnitTests
{
    [TestFixture]
    public class ControlStringFinderTests
    {
        [Test, AutoData]
        public void FindAllControlStrings_WhenMessageIsNull_ThrowsArgumentNullException(ControlStringFinder unit)
        {
            Should.Throw<ArgumentNullException>(() => unit.FindAllControlStrings(null));
        }

        [Test]
        public void FindAllControlStrings_SplitsControlStringsByControlStringStarterAndTerminator()
        {
            const char ControlStringStarter = '{';
            const char ControlStringTerminator = '}';

            var input = $"{ControlStringStarter}foo{ControlStringTerminator}";

            var unit = new ControlStringFinder(ControlStringStarter, 'a', ControlStringTerminator, 'b', 'c', 'd');

            var valueQueue = new Queue<string>();

            valueQueue.Enqueue("foo");

            var expected = new ControlString(0,5,valueQueue, new Queue<string>());

            // Act
            var result = unit.FindAllControlStrings(input).Single();

            result.Index.ShouldBe(expected.Index);
            result.Length.ShouldBe(expected.Length);
            result.Values.Peek().ShouldBe(expected.Values.Peek());
        }

        [Test]
        public void FindAllControlStrings_SplitsFoundControlStringByControlStringSeparator()
        {
            const char ControlStringStarter = '{';
            const char ControlStringTerminator = '}';
            const char ControlStringSeparator = ':';

            var input = $"{ControlStringStarter}foo{ControlStringSeparator}bar{ControlStringTerminator}";

            var unit = new ControlStringFinder(ControlStringStarter, ControlStringSeparator, ControlStringTerminator, 'b', 'c', 'd');

            var valueQueue = new Queue<string>();

            valueQueue.Enqueue("foo");
            valueQueue.Enqueue("bar");

            var expected = new ControlString(0, 9, valueQueue, new Queue<string>());

            // Act
            var result = unit.FindAllControlStrings(input).Single();

            result.Length.ShouldBe(expected.Length);

            result.Values.Dequeue();
            expected.Values.Dequeue();

            result.Values.Peek().ShouldBe(expected.Values.Peek());
        }

        [Test, AutoData]
        public void FindPostpendingSpecial_WhenInputIsNull_ThrowsArgumentNullException(ControlStringFinder unit)
        {
            Should.Throw<ArgumentNullException>(() => unit.FindPostpendingSpecial(null));
        }

        [Test]
        public void FindPostpendingSpecial_FindsInternalString()
        {
            const char SpecialStringStarter = '[';
            const char SpecialStringTerminator = ']';

            var input = $"{SpecialStringStarter}bar{SpecialStringTerminator}";

            var unit = new ControlStringFinder('{', ':', '}', SpecialStringStarter, SpecialStringTerminator, 'd');

            var expected = "bar";

            // Act
            var result = unit.FindPostpendingSpecial(input);

            result.ShouldBe(expected);
        }

        [Test]
        public void FindPostpendingSpecial_IgnoresLeadingCharactersOutsideSpecialStringStarter()
        {
            const char SpecialStringStarter = '[';
            const char SpecialStringTerminator = ']';

            var input = $"foo{SpecialStringStarter}bar{SpecialStringTerminator}";

            var unit = new ControlStringFinder('{', ':', '}', SpecialStringStarter, SpecialStringTerminator, 'd');

            var expected = "bar";

            // Act
            var result = unit.FindPostpendingSpecial(input);

            result.ShouldBe(expected);
        }

        [Test]
        public void FindPostpendingSpecial_WhenInputDoesNotEndWithSpecialStringTerminator_ReturnsEmptyString()
        {
            const char SpecialStringStarter = '[';
            const char SpecialStringTerminator = ']';

            var input = $"{SpecialStringStarter}bar{SpecialStringTerminator}foo";

            var unit = new ControlStringFinder('{', ':', '}', SpecialStringStarter, SpecialStringTerminator, 'd');

            var expected = string.Empty;

            // Act
            var result = unit.FindPostpendingSpecial(input);

            result.ShouldBe(expected);
        }

        [Test, AutoData]
        public void FindPrependingSpecial_WhenInputIsNull_ThrowsArgumentNullException(ControlStringFinder unit)
        {
            Should.Throw<ArgumentNullException>(() => unit.FindPrependingSpecial(null));
        }

        [Test, AutoData]
        public void FindPrependingSpecial_WhenInputIsEmptyString_ReturnsEmptyString(ControlStringFinder unit)
        {
            var actual = unit.FindPrependingSpecial(string.Empty);

            actual.ShouldBe(string.Empty);
        }

        [Test, AutoData]
        public void FindPostpendingSpecial_WhenInputIsEmptyString_ReturnsEmptyString(ControlStringFinder unit)
        {
            var actual = unit.FindPostpendingSpecial(string.Empty);

            actual.ShouldBe(string.Empty);
        }

        [Test]
        public void FindPrependingSpecial_FindsInternalString()
        {
            const char SpecialStringStarter = '[';
            const char SpecialStringTerminator = ']';

            var input = $"{SpecialStringStarter}bar{SpecialStringTerminator}";

            var unit = new ControlStringFinder('{', ':', '}', SpecialStringStarter, SpecialStringTerminator, 'd');

            var expected = "bar";

            // Act
            var result = unit.FindPrependingSpecial(input);

            result.ShouldBe(expected);
        }

        [Test]
        public void FindPrependingSpecial_IgnoresTrailingCharactersOutsideSpecialStringTerminator()
        {
            const char SpecialStringStarter = '[';
            const char SpecialStringTerminator = ']';

            var input = $"{SpecialStringStarter}bar{SpecialStringTerminator}foo";

            var unit = new ControlStringFinder('{', ':', '}', SpecialStringStarter, SpecialStringTerminator, 'd');

            var expected = "bar";

            // Act
            var result = unit.FindPrependingSpecial(input);

            result.ShouldBe(expected);
        }

        [Test]
        public void FindPrependingSpecial_WhenInputDoesNotStartWithSpecialStringStarter_ReturnsEmptyString()
        {
            const char SpecialStringStarter = '[';
            const char SpecialStringTerminator = ']';

            var input = $"foo{SpecialStringStarter}bar{SpecialStringTerminator}";

            var unit = new ControlStringFinder('{', ':', '}', SpecialStringStarter, SpecialStringTerminator, 'd');
            var expected = string.Empty;

            // Act
            var result = unit.FindPrependingSpecial(input);

            result.ShouldBe(expected);
        }

    }
}
