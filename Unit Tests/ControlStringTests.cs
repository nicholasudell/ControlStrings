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
    public class ControlStringTests
    {
        [Test, AutoData]
        public void Constructor_ShouldThrowExceptionsForMissingValueQueue(int index, int length) => 
            Should.Throw<ArgumentNullException>(() => new ControlString(index, length, null));

        [Test, AutoData]
        public void NextControlString_IncreasesIndexByCurrentValueLength([Frozen] Queue<string> values, ControlString unit, string firstString, string secondString)
        {
            values.Enqueue(firstString);
            values.Enqueue(secondString);

            unit.NextControlString.Index.ShouldBe(unit.Index + unit.Values.Peek().Length);
        }

        [Test, AutoData]
        public void NextControlString_DecreasesLengthByCurrentValueLength([Frozen] Queue<string> values, ControlString unit, string firstString, string secondString)
        {
            values.Enqueue(firstString);
            values.Enqueue(secondString);

            unit.NextControlString.Length.ShouldBe(unit.Length - unit.Values.Peek().Length);
        }

        [Test, AutoData]
        public void NextControlString_DequeuesFirstItemFromValueQueue([Frozen] Queue<string> values, ControlString unit, string firstString, string secondString)
        {
            values.Enqueue(firstString);
            values.Enqueue(secondString);

            unit.NextControlString.Values.Peek().ShouldBe(unit.Values.Skip(1).First());
        }
    }
}
