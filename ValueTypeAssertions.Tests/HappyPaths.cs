using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ValueTypeAssertions.Tests
{
    [TestClass]
    public class HappyPaths
    {
        [TestMethod]
        public void Int()
        {
            ((Action) (() => ValueTypeAssertions.HasValueEquality(1, 1))).ShouldNotThrow();
            ((Action) (() => ValueTypeAssertions.HasValueEquality(1, 2))).ShouldThrow<AssertFailedException>();
            ((Action) (() => ValueTypeAssertions.HasValueInequality(2, 1))).ShouldNotThrow();
            ((Action) (() => ValueTypeAssertions.HasValueInequality(2, 2))).ShouldThrow<AssertFailedException>();
        }

        [TestMethod]
        public void String()
        {
            ((Action) (() => ValueTypeAssertions.HasValueEquality("", ""))).ShouldNotThrow();
            ((Action) (() => ValueTypeAssertions.HasValueEquality("foo", "foo"))).ShouldNotThrow();
            ((Action) (() => ValueTypeAssertions.HasValueEquality("foo", "bar"))).ShouldThrow<AssertFailedException>();
            ((Action) (() => ValueTypeAssertions.HasValueInequality("foo", "foo"))).ShouldThrow<AssertFailedException>();
        }

        [TestMethod]
        public void CustomReferenceTypeTest()
        {
            ((Action) (() => ValueTypeAssertions.HasValueEquality(new AClass(1), new AClass(1)))).ShouldNotThrow();
            ((Action) (() => ValueTypeAssertions.HasValueEquality(new AClass(1), new AClass(2))))
                .ShouldThrow<AssertFailedException>();
            ((Action) (() => ValueTypeAssertions.HasValueInequality(new AClass(1), new AClass(2)))).ShouldNotThrow();
            ((Action) (() => ValueTypeAssertions.HasValueInequality(new AClass(1), new AClass(1))))
                .ShouldThrow<AssertFailedException>();
        }

        [TestMethod]
        public void CustomValueTypeTest()
        {
            ((Action) (() => ValueTypeAssertions.HasValueEquality(new AStruct(1), new AStruct(1)))).ShouldNotThrow();
            ((Action) (() => ValueTypeAssertions.HasValueEquality(new AStruct(1), new AStruct(2))))
                .ShouldThrow<AssertFailedException>();
            ((Action) (() => ValueTypeAssertions.HasValueInequality(new AStruct(1), new AStruct(2)))).ShouldNotThrow();
            ((Action) (() => ValueTypeAssertions.HasValueInequality(new AStruct(1), new AStruct(1))))
                .ShouldThrow<AssertFailedException>();
        }

        private struct AStruct : IEquatable<AStruct>
        {
            public readonly int X;

            public AStruct(int x)
            {
                X = x;
            }

            bool IEquatable<AStruct>.Equals(AStruct other) => Equals(other);

            public static bool operator ==(AStruct left, AStruct right)
            {
                return left.Equals(right);
            }

            public static bool operator !=(AStruct left, AStruct right)
            {
                return !left.Equals(right);
            }
        }

        private class AClass : IEquatable<AClass>
        {
            public readonly int X;

            public AClass(int x)
            {
                X = x;
            }

            bool IEquatable<AClass>.Equals(AClass other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                return X == other.X;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != GetType()) return false;
                return ((IEquatable<AClass>) this).Equals((AClass) obj);
            }

            public override int GetHashCode()
            {
                return X.GetHashCode();
            }

            public static bool operator ==(AClass left, AClass right)
            {
                return Equals(left, right);
            }

            public static bool operator !=(AClass left, AClass right)
            {
                return !Equals(left, right);
            }
        }
    }
}