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

        [Test, AutoData]
        public void FindAllControlStrings_SplitsControlStringsByControlStringStarterAndTerminator()
        {
            const char ControlStringStarter = '{';
            const char ControlStringTerminator = '}';

            var input = $"{ControlStringStarter}foo{ControlStringTerminator}";

            var unit = new ControlStringFinder(ControlStringStarter, 'a', ControlStringTerminator, 'b', 'c');

            var valueQueue = new Queue<string>();

            valueQueue.Enqueue("foo");

            var expected = new ControlString(0,5,valueQueue);

            // Act
            var result = unit.FindAllControlStrings(input).Single();

            result.Index.ShouldBe(expected.Index);
            result.Length.ShouldBe(expected.Length);
            result.Values.Peek().ShouldBe(expected.Values.Peek());
        }

        [Test, AutoData]
        public void FindAllControlStrings_SplitsFoundControlStringByControlStringSeparator()
        {
            const char ControlStringStarter = '{';
            const char ControlStringTerminator = '}';
            const char ControlStringSeparator = ':';

            var input = $"{ControlStringStarter}foo{ControlStringSeparator}bar{ControlStringTerminator}";

            var unit = new ControlStringFinder(ControlStringStarter, ControlStringSeparator, ControlStringTerminator, 'b', 'c');

            var valueQueue = new Queue<string>();

            valueQueue.Enqueue("foo");
            valueQueue.Enqueue("bar");

            var expected = new ControlString(0, 9, valueQueue);

            // Act
            var result = unit.FindAllControlStrings(input).Single();

            result.Length.ShouldBe(expected.Length);

            result.Values.Dequeue();
            expected.Values.Dequeue();

            result.Values.Peek().ShouldBe(expected.Values.Peek());
        }

        //[Test]
        //public void FindPostpendingSpecial_StateUnderTest_ExpectedBehavior()
        //{
        //    // Arrange
        //    var controlStringFinder = new ControlStringFinder(TODO, TODO, TODO, TODO, TODO);
        //    string input = null;

        //    // Act
        //    var result = controlStringFinder.FindPostpendingSpecial(
        //        input);

        //    // Assert
        //    Assert.Fail();
        //}

        //[Test]
        //public void FindPrependingSpecial_StateUnderTest_ExpectedBehavior()
        //{
        //    // Arrange
        //    var controlStringFinder = new ControlStringFinder(TODO, TODO, TODO, TODO, TODO);
        //    string input = null;

        //    // Act
        //    var result = controlStringFinder.FindPrependingSpecial(
        //        input);

        //    // Assert
        //    Assert.Fail();
        //}
    }
}
