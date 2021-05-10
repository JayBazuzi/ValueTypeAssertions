[![Build status](https://ci.appveyor.com/api/projects/status/drlkrd4ftaou35j1/branch/master?svg=true)](https://ci.appveyor.com/project/JayBazuzi/valuetypeassertions/branch/master)

[Available on NuGet](https://www.nuget.org/packages/Bazuzi.ValueTypeAssertions/)

# ValueTypeAssertions

By "value type", I mean "a type that represents a value in some domain." Two of these objects are equal if they have the same value.

To correctly implement this in .Net is tricky. There's `object.Equals()` and `operator ==` and `GetHashCode()` and more. These are assertions that ensure that you have checked all the checkboxes in implementing equality. 

You can use it like this:

```
class NtfsPath
{
  // implementation here
}

[Test]
public void NtfsPathHasValueEquality()
{
    new NtfsPath("foo.txt")
      .ShouldBeEquivalentTo(new NtfsPath("foo.txt"))
      .And(new NtfsPath("FOO.TXT"))
      .ButDifferentFrom(new NtfsPath("bar.txt"))
}
```

# Acknowledgements

99% of the ideas in this project came from other people. A big chunk came from [Brian Geihsler](https://gist.github.com/bgeihsgt).
