using ExtensionMethods;
using System;
using System.Collections.Generic;
using Xunit;

namespace ExtensionMethods_Facts
{
    public class ExtensionFacts
    {
        [Fact]
        public void Test_Aggregate_ExtMethod_Should_Return_Max_Number()
        {
            //Given
            var array = new int[3] { 1, 2, 3 };
            int seed = array[0];
            //When
            var maximum = array.Aggregate(seed, (maxValue, current) =>
                                          current > maxValue ? current : maxValue);
            //Then
            Assert.Equal(3, maximum);
        }

        [Fact]
        public void Test_Aggregate_ExtMethod_Should_Return_Product_Of_Integers()
        {
            //Given
            var array = new int[3] { 1, 2, 3 };
            const int seed = 1;
            //When
            var product = array.Aggregate(seed, (total, current) => total * current);
            //Then
            Assert.Equal(6, product);
        }

        [Fact]
        public void Test_Aggregate_ExtMethod_Should_Throw_Exception_When_Func_is_NULL()
        {
            //Given
            const string array = "Andrei";
            //When
            string seed = string.Empty;
            var exception = Assert.Throws<ArgumentNullException>(() => array.Aggregate(seed, null));
            //Then
            Assert.Equal("func", exception.ParamName);
        }

        [Fact]
        public void Test_Aggregate_ExtMethod_Should_Throw_Exception_When_Source_is_NULL()
        {
            //Given
            int[] array = null;
            //When
            const int seed = 0;
            Action sumCalculus = () => array.Aggregate(
                                         seed,
                                         (now, after) => now + after);
            //Then
            Assert.Throws<ArgumentNullException>(sumCalculus);
        }

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
            Assert.False(array.Any(n => n < 0));
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
        public void Test_Distinct_ExtMethod_Should_Return_Distinct_Items_From_IntArray()
        {
            //Given
            int[] array = { 1, 2, 2, 3, 3, 4, 5 };
            //When
            var newArray = array.Distinct(new DistinctComparison());
            //Then
            Assert.Equal(new[] { 1, 2, 3, 4, 5 }, newArray);
        }

        [Fact]
        public void Test_Distinct_ExtMethod_Should_Throw_Exception_When_Object_is_NULL()
        {
            //Given
            int[] array = null;
            //When
            var enumerator = array.Distinct(new DistinctComparison()).GetEnumerator();
            var exception = Assert.Throws<ArgumentNullException>(() => enumerator.MoveNext());
            //Then
            Assert.Equal("source", exception.ParamName);
        }

        [Fact]
        public void Test_Distinct_ExtMethod_Should_Work_Correctly_for_Simple_Array()
        {
            //Given
            int[] array = { 1, 2, 2 };
            //When
            var newArray = array.Distinct(new DistinctComparison());
            //Then
            Assert.Equal(new[] { 1, 2 }, newArray);
        }

        [Fact]
        public void Test_Distinct_ExtMethod_Test_For_Strings()
        {
            //Given
            string[] names = { "Abc", "Bca", "abc", "bca" };
            //When
            var newArray = names.Distinct(new StringsComparison());
            //Then
            Assert.Equal(new[] { "Abc", "Bca" }, newArray);
        }

        [Fact]
        public void Test_Except_ExtMethod_for_simple_IntArrays()
        {
            //Given
            int[] firstArray = { 1, 2, 3 };
            int[] secondArray = { 3, 4, 5 };
            //When
            var newArray = firstArray.Except(secondArray, new DistinctComparison());
            //Then
            Assert.Equal(new[] { 1, 2 }, newArray);
        }

        [Fact]
        public void Test_Except_ExtMethod_More_Complex_Case()
        {
            //Given
            int[] firstArray = { 1, 2, 2, 6, 3, 5, 5, 4, 6 };
            int[] secondArray = { 1, 3, 3, 4, 4, 7 };
            //When
            var newArray = firstArray.Except(secondArray, new DistinctComparison());
            //Then
            Assert.Equal(new[] { 2, 6, 5 }, newArray);
        }

        [Fact]
        public void Test_Except_ExtMethod_Should_Throw_Exception_When_One_Enumerable_is_NULL()
        {
            //Given
            int[] firstArray = new int[2] { 4, 5 };
            int[] secondArray = null;
            //When
            var enumerator = firstArray.Except(secondArray, new DistinctComparison()).GetEnumerator();
            var exception = Assert.Throws<ArgumentNullException>(() => enumerator.MoveNext());
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
        public void Test_Intersect_ExtMethod_for_more_complex_Case()
        {
            //Given
            int[] firstArray = { 1, 3, 3, 4, 2, 5, 5 };
            int[] secondArray = { 3, 4, 1, 5, 6 };
            //When
            var newArray = firstArray.Intersect(secondArray, new DistinctComparison());
            //Then
            Assert.Equal(new[] { 1, 3, 4, 5 }, newArray);
        }

        [Fact]
        public void Test_Intersect_ExtMethod_for_simple_IntArrays()
        {
            //Given
            int[] firstArray = { 1, 2, 3 };
            int[] secondArray = { 3, 4, 2 };
            //When
            var newArray = firstArray.Intersect(secondArray, new DistinctComparison());
            //Then
            Assert.Equal(new[] { 2, 3 }, newArray);
        }

        [Fact]
        public void Test_Intersect_ExtMethod_Should_Throw_Exception_When_One_Enumerable_is_NULL()
        {
            //Given
            int[] firstArray = new int[2] { 4, 5 };
            int[] secondArray = null;
            //When
            var enumerator = firstArray.Intersect(secondArray, new DistinctComparison()).GetEnumerator();
            var exception = Assert.Throws<ArgumentNullException>(() => enumerator.MoveNext());
            //Then
            Assert.Equal("source", exception.ParamName);
        }

        [Fact]
        public void Test_Join_ExtMethod_Should_Join_2_Lists_Of_Strings()
        {
            //Given
            List<string> firstList = new List<string>()
            {
                "Andrei",
                "Eusebiu"
            };

            List<string> secondList = new List<string>()
            {
                "Andrei",
                "AltAndrei"
            };
            //When
            var result = firstList.Join(secondList,
                                      firstNames => firstNames,
                                      secondNames => secondNames,
                                      (firstNames, _) => firstNames
                                      );
            //Then
            Assert.Equal(new List<string> { "Andrei" }, result);
        }

        [Fact]
        public void Test_Join_ExtMethod_Should_Join_2_Lists_Of_Strings_Second_Test()
        {
            //Given
            const string firstWord = "ABCD";
            const string secondWord = "EFCD";
            //When
            var result = firstWord.Join(secondWord,
                                      firstNames => firstNames,
                                      secondNames => secondNames,
                                      (firstNames, _) => firstNames
                                      );
            //Then
            Assert.Equal("CD", result);
        }

        [Fact]
        public void Test_Join_ExtMethod_Should_Throw_Exception_When_ResultSelector_is_NULL()
        {
            //Given
            const string firstWord = "ABCD";
            const string secondWord = "EFCD";
            //When
            Func<char, char, char> nullFunc = null;
            var result = firstWord.Join(secondWord,
                                      firstNames => firstNames,
                                      secondNames => secondNames,
                                      nullFunc
                                      ).GetEnumerator();
            var exception = Assert.Throws<ArgumentNullException>(() => result.MoveNext());
            //Then
            Assert.Equal("resultSelector", exception.ParamName);
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
            Assert.Equal(new[] { "andrei", "ion", "vasile" }, enumerator);
        }

        [Fact]
        public void Test_SelectMany_ExtMethod_Should_Throw_Exception_When_Object_is_NULL()
        {
            //Given
            string[] names = null;
            //When
            var enumerator = names.SelectMany(x => new[] { x + "1" }).GetEnumerator();
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

        [Fact]
        public void Test_ToDictionary_ExtMethod_Should_Correctly_Return_Dictionary_Of_Int_Pair()
        {
            //Given
            var array = new int[4] { 1, 2, 3, 4 };
            //When
            var dictionary = array.ToDictionary(n => n.GetHashCode(), m => m.GetHashCode());
            var enumerator = dictionary.GetEnumerator();
            enumerator.MoveNext();
            //Then
            Assert.Equal(new KeyValuePair<int, int>(1, 1), enumerator.Current);
        }

        [Fact]
        public void Test_ToDictionary_ExtMethod_Should_Throw_Exception_Key_produced_already_Exists()
        {
            //Given
            int[] array = new int[3] { 1, 1, 1 };
            //When
            Action action = () => array.ToDictionary(
                                         m => m.GetHashCode(),
                                         m => m.GetHashCode());
            //Then
            Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void Test_ToDictionary_ExtMethod_Should_Throw_Exception_When_ElementSelector_Is_NULL()
        {
            //Given
            int[] array = new int[3] { 1, 2, 3 };
            //When
            var exception = Assert.Throws<ArgumentNullException>(
                () => array.ToDictionary<int, int, string>(
                                         m => m.GetHashCode(),
                                         null));
            //Then
            Assert.Equal("selector", exception.ParamName);
        }

        [Fact]
        public void Test_ToDictionary_ExtMethod_Should_Throw_Exception_When_Key_Produced_Is_NULL()
        {
            //Given
            int[] array = new int[3] { 1, 2, 3 };
            //When
            string KeySelector()
            {
                return null;
            }

            var exception = Assert.Throws<ArgumentNullException>(
                () => array.ToDictionary(
                                         _ => KeySelector(),
                                         m => m.GetHashCode()));
            //Then
            Assert.Equal("key", exception.ParamName);
        }

        [Fact]
        public void Test_ToDictionary_ExtMethod_Should_Throw_Exception_When_KeySelector_is_NULL()
        {
            //Given
            int[] array = new int[3] { 1, 2, 3 };
            //When
            var exception = Assert.Throws<ArgumentNullException>(
                () => array.ToDictionary<int, string, int>(
                                         null,
                                         m => m.GetHashCode()));
            //Then
            Assert.Equal("selector", exception.ParamName);
        }

        [Fact]
        public void Test_ToDictionary_ExtMethod_Should_Throw_Exception_When_Object_Is_NULL()
        {
            //Given
            int[] array = null;
            //When
            var exception = Assert.Throws<ArgumentNullException>(
                () => array.ToDictionary(n => n.GetHashCode(),
                                         m => m.GetHashCode()));
            //Then
            Assert.Equal("source", exception.ParamName);
        }

        [Fact]
        public void Test_Union_ExtMethod_for_simple_IntArrays()
        {
            //Given
            int[] firstArray = { 1, 2, 2 };
            int[] secondArray = { 3, 4, 2 };
            //When
            var newArray = firstArray.Union(secondArray, new DistinctComparison());
            //Then
            Assert.Equal(new[] { 1, 2, 3, 4 }, newArray);
        }

        [Fact]
        public void Test_Union_ExtMethod_IntArrays_More_Complex_Case()
        {
            //Given
            int[] firstArray = { 1, 2, 2, 3, 4, 4, 4 };
            int[] secondArray = { 3, 4, 5, 5, 6, 4, 8, 7 };
            //When
            var newArray = firstArray.Union(secondArray, new DistinctComparison());
            //Then
            Assert.Equal(new[] { 1, 2, 3, 4, 5, 6, 8, 7 }, newArray);
        }

        [Fact]
        public void Test_Union_ExtMethod_Should_Throw_Exception_When_One_Enumerable_is_NULL()
        {
            //Given
            int[] firstArray = new int[2] { 4, 5 };
            int[] secondArray = null;
            //When
            var enumerator = firstArray.Union(secondArray, new DistinctComparison()).GetEnumerator();
            var exception = Assert.Throws<ArgumentNullException>(() => enumerator.MoveNext());
            //Then
            Assert.Equal("source", exception.ParamName);
        }

        [Fact]
        public void Test_Where_ExtMethod_Should_return_Positive_Numbers_from_Array()
        {
            //Given
            var array = new int[4] { -1, 2, 3, -4 };
            //When
            var enumerable = array.Where(n => n > 0);
            //Then
            Assert.Equal(new[] { 2, 3 }, enumerable);
        }

        [Fact]
        public void Test_Where_ExtMethod_Should_Throw_Exception_When_Object_is_NULL()
        {
            //Given
            int[] array = null;
            //When
            var enumerator = array.Where(x => x > 1).GetEnumerator();
            var exception = Assert.Throws<ArgumentNullException>(() => enumerator.MoveNext());
            //Then
            Assert.Equal("source", exception.ParamName);
        }

        [Fact]
        public void Test_Where_ExtMethod_Should_Throw_Exception_When_Predicate_is_NULL()
        {
            //Given
            int[] array = new int[3] { 1, 2, 3 };
            //When
            var enumerator = array.Where(null).GetEnumerator();
            var exception = Assert.Throws<ArgumentNullException>(() => enumerator.MoveNext());
            //Then
            Assert.Equal("selector", exception.ParamName);
        }

        [Fact]
        public void Test_Zip_ExtMethod_Should_Correctly_Merge_Arrays_First_Is_Longer()
        {
            //Given
            var firstArray = new int[3] { 1, 2, 3 };
            var secondArray = new int[2] { 4, 5 };
            //When
            var newArray = firstArray.Zip(secondArray, (m, n) => m + n);
            //Then
            Assert.Equal(new[] { 5, 7 }, newArray);
        }

        [Fact]
        public void Test_Zip_ExtMethod_Should_Merge_2_Arrays_Of_Different_Length()
        {
            //Given
            var firstArray = new int[2] { 1, 2 };
            var secondArray = new int[3] { 3, 4, 5 };
            //When
            var newArray = firstArray.Zip(secondArray, (m, n) => m + n);
            //Then
            Assert.Equal(new[] { 4, 6 }, newArray);
        }

        [Fact]
        public void Test_Zip_ExtMethod_Should_Merge_2_Arrays_Of_Same_Length()
        {
            //Given
            var firstArray = new int[2] { 1, 2 };
            var secondArray = new int[2] { 3, 4 };
            //When
            var newArray = firstArray.Zip(secondArray, (m, n) => m + n);
            //Then
            Assert.Equal(new[] { 4, 6 }, newArray);
        }

        [Fact]
        public void Test_Zip_ExtMethod_Should_Throw_Exception_When_First_is_NULL()
        {
            //Given
            int[] firstArray = null;
            int[] secondArray = new int[2] { 4, 5 };
            //When
            var enumerator = firstArray.Zip(secondArray, (m, n) => m + n).GetEnumerator();
            var exception = Assert.Throws<ArgumentNullException>(() => enumerator.MoveNext());
            //Then
            Assert.Equal("source", exception.ParamName);
        }

        [Fact]
        public void Test_Zip_ExtMethod_Should_Throw_Exception_When_Second_is_NULL()
        {
            //Given
            int[] firstArray = new int[2] { 4, 5 };
            int[] secondArray = null;
            //When
            var enumerator = firstArray.Zip(secondArray, (m, n) => m + n).GetEnumerator();
            var exception = Assert.Throws<ArgumentNullException>(() => enumerator.MoveNext());
            //Then
            Assert.Equal("source", exception.ParamName);
        }
    }

    internal class DistinctComparison : IEqualityComparer<int>
    {
        public bool Equals(int x, int y)
        {
            return x == y;
        }

        public int GetHashCode(int obj)
        {
            return obj.GetHashCode();
        }
    }

    internal class StringsComparison : IEqualityComparer<string>
    {
        public bool Equals(string x, string y)
        {
            return string.Equals(x, y, StringComparison.OrdinalIgnoreCase);
        }

        public int GetHashCode(string obj)
        {
            return obj.ToLower().GetHashCode();
        }
    }
}