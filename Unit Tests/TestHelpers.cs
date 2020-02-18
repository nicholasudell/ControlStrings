using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.NUnit3;
using System;
using System.Collections.Generic;
using System.Text;

namespace ControlStrings.UnitTests
{
    internal class AutoMoqDataAttribute : AutoDataAttribute
    {
        internal AutoMoqDataAttribute()
            : base(()=> new Fixture().Customize(new AutoMoqCustomization()))
        {
        }
    }
}
