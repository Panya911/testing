using System;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace Kontur.Courses.Testing.Patterns.Specifications
{
    public class MarkdownProcessor
    {
        public string Render(string input)
        {
            var emReplacer = new Regex(@"([^\w\\]|^)_(.*?[^\\])_(\W|$)");
            var strongReplacer = new Regex(@"([^\w\\]|^)__(.*?[^\\])__(\W|$)");
            input = strongReplacer.Replace(input,
                    match => match.Groups[1].Value +
                            "<strong>" + match.Groups[2].Value + "</strong>" +
                            match.Groups[3].Value);
            input = emReplacer.Replace(input,
                    match => match.Groups[1].Value +
                            "<em>" + match.Groups[2].Value + "</em>" +
                            match.Groups[3].Value);
            input = input.Replace(@"\_", "_");
            return input;
        }
    }

    [TestFixture]
    public class MarkdownProcessor_should
    {
        private readonly MarkdownProcessor md = new MarkdownProcessor();

        [TestCase("it _is_ it!", Result = "it <em>is</em> it!")]
        [TestCase("it_is_word", Result = "it_is_word")]
        [TestCase(@"\_word\_", Result = @"_word_")]
        [TestCase("hello _world", Result = "hello _world")]
        public string surroundWithEm_textInsideUnderscores(string text)
        {
            return md.Render(text);
        }
        //TODO see Markdown.txt
        [TestCase("it __is__ it!", Result = "it <strong>is</strong> it!")]
        [TestCase("it__is__word", Result = "it__is__word")]
        [TestCase(@"\_word\_", Result = @"_word_")]
        [TestCase("hello __world", Result = "hello __world")]
        public string surroundWithStrong_textInsideDoubleUnderscores(string text)
        {
            return md.Render(text);
        }

        [Test]
        public void ignoredUnparedSymbols()
        {
            var result = md.Render("__непарные _символы");
            Assert.AreEqual("__непарные _символы", result);
        }

        [Test]
        public void handleNestedTags()
        {
            var result = md.Render("Внутри _выделения может быть __другое__ выделение_");
            Assert.AreEqual("Внутри <em>выделения может быть <strong>другое</strong> выделение</em>",
                result);           
        }
    }
}
