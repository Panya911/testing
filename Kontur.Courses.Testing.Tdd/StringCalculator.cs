using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Kontur.Courses.Testing.Tdd
{
    public class StringCalculator
    {
        public int Add(string expr)
        {
            var delimiters = ParseDelimiters(ref expr);
            var parsedNumbers = ParseNumbers(expr, delimiters);
            FailOnNegatives(parsedNumbers);
            return parsedNumbers.Any()
                ? parsedNumbers.Sum() : 0;
        }

        public static List<int> ParseNumbers(string expr, char[] delimiters)
        {
            return expr.Split(delimiters)
                .Where(ch => ch != "")
                .Select(int.Parse)
                .ToList();
        }

        public static void FailOnNegatives(List<int> numbers)
        {
            var negatives = numbers.Where(n => n < 0).ToList();
            if (negatives.Any())
                throw new ArgumentException(
                    "negatives not allowed: " + string.Join(", ", negatives));
        }

        public static char[] ParseDelimiters(ref string text)
        {
            if (!text.StartsWith("//") || text.Length <= 2)
                return new[] { ',', '\n' };
            var delimiter = text[2];
            text = text.Split('\n')[1];
            return new[] { delimiter };
        }
    }

    [TestFixture]
    public class StringCalculator_should
    {
        [Test]
        public void returnZero_onEmptyInput()
        {
            var result = new StringCalculator().Add("");
            Assert.AreEqual(0, result);
        }

        [TestCase("", Result = 0)]
        [TestCase("42", Result = 42)]
        [TestCase("42,13", Result = 55)]
        [TestCase("1,2,3,4,5", Result = 15)]
        public int add_ComaSeparatedNumbers(string numbers)
        {
            return new StringCalculator().Add(numbers);
        }

        [TestCase("1\n2\n3", Result = 6)]
        public int add_NewlineSeparatedNumbers(string numbers)
        {
            return new StringCalculator().Add(numbers);
        }

        [TestCase("1\n2,3", Result = 6)]
        public int add_NumbersSeparatedByBothCommaAndNewline(string numbers)
        {
            return new StringCalculator().Add(numbers);
        }

        [TestCase("//;\n1;2;3", Result = 6)]
        [TestCase("//|\n4|5|61", Result = 70)]
        public int useDelimiterFromFirstSpecialLine(string numbers)
        {
            return new StringCalculator().Add(numbers);
        }

        [Test]
        public void throwException_onNegativeNumbers()
        {
            var calc = new StringCalculator();
            Assert.Throws<ArgumentException>(() => calc.Add("-1"));
        }

        [Test]
        public void listAllNegatives_inExceptionMessage()
        {
            var calc = new StringCalculator();
            var ex = Assert.Throws<ArgumentException>(
                () => calc.Add("-1,2,-5,-100,8"));
            StringAssert.Contains("-1, -5, -100", ex.Message);
        }
    }
}
