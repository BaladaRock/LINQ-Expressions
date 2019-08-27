using ExtensionMethods;
using System;
using Xunit;

namespace ExtensionMethods_Facts
{
    public class ExtensionFacts
    {
        [Fact]
        public void Test_All_ExtMethod_Should_Return_False_When_Condition_is_Not_Satisfied()
        {
            //Given, When
            var array = new int[3] { 1, 2, 3 };
            //Then
            Assert.False(array.All(n => n < 0));
        }

        [Fact]
        public void Test_All_ExtMethod_Should_Return_False_When_Condition_is_Partially_Satisfied()
        {
            //Given, When
            var array = new int[3] { -1, -2, 3 };
            //Then
            Assert.False(array.All(n => n < 0));
        }

        [Fact]
        public void Test_All_ExtMethod_Should_Return_True_When_Condition_is_Satisfied()
        {
            //Given, When
            var array = new int[3] { 0, 1, 2 };
            //Then
            Assert.True(array.All(n => n >= 0));
        }

        [Fact]
        public void Test_All_ExtMethod_Should_Throw_Exception_When_Object_is_NULL()
        {
            //Given
            var array = new int[4] { 1, 2, 3, 4 };
            array = null;
            //When
            var exception = Assert.Throws<ArgumentNullException>(() => array.All(n => n < 0));
            //Then
            Assert.Equal("source", exception.ParamName);
        }

        [Fact]
        public void Test_Any_ExtMethod_Should_Return_False_When_NO_Element_Satisfies_Condition()
        {
            //Given, When
            var array = new int[3] { 1, 2, 3 };
            //Then
            Assert.False(array.All(n => n < 0));
        }

        [Fact]
        public void Test_Any_ExtMethod_Should_Return_True_When_All_Elements_Satisfy_Condition()
        {
            //Given, When
            var array = new int[3] { -1, -2, -3 };
            //Then
            Assert.True(array.Any(n => n < 0));
        }

        [Fact]
        public void Test_Any_ExtMethod_Should_Return_True_When_One_Element_Satisfies_Condition()
        {
            //Given, When
            var array = new int[3] { 1, -2, 3 };
            //Then
            Assert.True(array.Any(n => n < 0));
        }

        [Fact]
        public void Test_Any_ExtMethod_Should_Throw_Exception_When_Object_is_NULL()
        {
            //Given
            var array = new int[4] { 1, 2, 3, 4 };
            array = null;
            //When
            var exception = Assert.Throws<ArgumentNullException>(() => array.Any(n => n < 0));
            //Then
            Assert.Equal("source", exception.ParamName);
        }

        [Fact]
        public void Test_First_ExtMethod_Should_return_Correct_element()
        {
            //Given, When
            var array = new int[4] { 1, 2, -3, 4 };
            //Then
            bool LessThenZero(int n)
            {
                return n < 0;
            }
            Assert.Equal(-3, array.First(LessThenZero));
        }

        [Fact]
        public void Test_First_ExtMethod_Should_return_First_element()
        {
            //Given, When
            var array = new int[3] { -1, 2, 3 };
            //Then
            Assert.Equal(-1, array.First(n => n < 0));
        }

        [Fact]
        public void Test_First_ExtMethod_Should_Throw_Exception_When_NO_Element_Was_Found()
        {
            //Given
            var array = new int[4] { 1, 2, 3, 4 };
            //When
            var exception = Assert.Throws<InvalidOperationException>(() => array.First(n => n < 0));
            //Then
            Assert.Equal("No IEnumerable<TSource> element satisfies delegate condition!/t", exception.Message);
        }

        [Fact]
        public void Test_First_ExtMethod_Should_Throw_Exception_When_Object_is_NULL()
        {
            //Given
            var array = new int[4] { 1, 2, 3, 4 };
            array = null;
            //When
            var exception = Assert.Throws<ArgumentNullException>(() => array.First(n => n < 0));
            //Then
            Assert.Equal("source", exception.ParamName);
        }

        [Fact]
        public void Test_Select_ExtMethod_Should_return_ZeroValues_from_IntArray()
        {
            //Given
            var array = new int[3] { -1, 2, 3 };
            //When
            var enumerable = array.Select(_ => 0);
            //Then
            Assert.Equal(new[] { 0, 0, 0 }, enumerable);
        }

        [Fact]
        public void Test_Select_ExtMethod_Should_Throw_Exception_When_Object_is_NULL()
        {
            //Given
            int[] array = null;
            //When
            var enumerator = array.Select(n => n * 2).GetEnumerator();
            var exception = Assert.Throws<ArgumentNullException>(() => enumerator.MoveNext());
            //Then
            Assert.Equal("source", exception.ParamName);
        }

        [Fact]
        public void Test_Select_ExtMethod_Should_Throw_Exception_When_Selector_is_NULL()
        {
            //Given
            var array = new int[4] { 1, 2, 3, 4 };
            //When
            var enumerator = array.Select<int, int>(null).GetEnumerator();
            var exception = Assert.Throws<ArgumentNullException>(() => enumerator.MoveNext());
            //Then
            Assert.Equal("selector", exception.ParamName);
        }

        [Fact]
        public void Test_SelectMany_ExtMethod_Should_return_Single_IEnumerable_from_Array()
        {
            //Given
            string[] names = new string[3] { "Andrei", "Ion", "Vasile" };
            //When
            var enumerator = names.SelectMany(x => new[] { x.ToLower() });
            //Then
            Assert.Equal(new[] {"andrei", "ion", "vasile"}, enumerator);
        }

        [Fact]
        public void Test_SelectMany_ExtMethod_Should_Throw_Exception_When_Object_is_NULL()
        {
            //Given
            string[] names = new string[3] { "Andrei", "Ion", "Vasile" };
            names = null;
            //When
            var enumerator = names.SelectMany(x => new[] {x+"1"}).GetEnumerator();
            var exception = Assert.Throws<ArgumentNullException>(() => enumerator.MoveNext());
            //Then
            Assert.Equal("source", exception.ParamName);
        }

        [Fact]
        public void Test_SelectMany_ExtMethod_Should_Throw_Exception_When_Selector_is_NULL()
        {
            //Given
            string[] names = new string[3] { "Andrei", "Ion", "Vasile" };
            //When
            var enumerator = names.SelectMany<string, string>(null).GetEnumerator();
            var exception = Assert.Throws<ArgumentNullException>(() => enumerator.MoveNext());
            //Then
            Assert.Equal("selector", exception.ParamName);
        }
    }
}