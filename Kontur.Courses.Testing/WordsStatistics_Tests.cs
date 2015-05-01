using System;
using System.Collections.Generic;
using System.Linq;
using Kontur.Courses.Testing.Implementations;
using NUnit.Framework;

namespace Kontur.Courses.Testing
{
    public class WordsStatistics_Tests
    {
        public Func<IWordsStatistics> createStat = () => new WordsStatistics_CorrectImplementation();
        // меняется на разные реализации при запуске exe

        public IWordsStatistics stat;

        [SetUp]
        public void SetUp()
        {
            stat = createStat();
        }

        [Test]
        public void no_stats_if_no_words()
        {
            CollectionAssert.IsEmpty(stat.GetStatistics());
        }

        [Test]
        public void same_word_twice()
        {
            stat.AddWord("xxx");
            stat.AddWord("xxx");
            CollectionAssert.AreEqual(new[] { Tuple.Create(2, "xxx") }, stat.GetStatistics());
        }

        [Test]
        public void single_word()
        {
            stat.AddWord("hello");
            CollectionAssert.AreEqual(new[] { Tuple.Create(1, "hello") }, stat.GetStatistics());
        }

        [Test]
        public void two_same_words_one_other()
        {
            stat.AddWord("hello");
            stat.AddWord("world");
            stat.AddWord("world");
            CollectionAssert.AreEqual(new[] { Tuple.Create(2, "world"), Tuple.Create(1, "hello") }, stat.GetStatistics());
        }

        [Test]

        public void words_same_count()
        {
            stat.AddWord("Hello");
            stat.AddWord("hello");
            stat.AddWord("world");
            stat.AddWord("world");
            CollectionAssert.AreEqual(new[] { Tuple.Create(2, "hello"), Tuple.Create(2, "world") }, stat.GetStatistics());
        }

        [Test]
        public void three_words()
        {
            stat.AddWord("hello");
            stat.AddWord("worlds");
            stat.AddWord("world");
            CollectionAssert.AreEqual(
                new[] { Tuple.Create(1, "hello"), Tuple.Create(1, "world"), Tuple.Create(1, "worlds") },
                stat.GetStatistics());
        }

        [Test]
        public void recreate_stat()
        {
            stat.AddWord("hello");
            var stat2 = createStat();
            stat2.AddWord("world");
            CollectionAssert.AreEqual(new[] { Tuple.Create(1, "hello") }, stat.GetStatistics());
        }

        [Test]
        public void putNullOrEmpty()
        {
            stat.AddWord("");
            stat.AddWord(null);
            Assert.IsEmpty(stat.GetStatistics());
        }

        [Test]
        public void test_elevens_distinct()
        {
            stat.AddWord("abcdefghioZ");
            stat.AddWord("abcdefghioK");
            CollectionAssert.AreEqual(new[] { Tuple.Create(2, "abcdefghio") }, stat.GetStatistics());
        }

        [Test, Timeout(1000)]
        public void test_many_different_strings()
        {
            var rnd = new Random();
            var words = new Dictionary<string, int>();
            for (var i = 0; i < 10000; i++)
            {
                var word = new char[10];
                for (var ch = 0; ch < 10; ch++)
                    word[ch] = (char)rnd.Next(50, 140);
                var str = new string(word).ToLower();
                int count;
                words[str] = words.TryGetValue(str, out count) ? count + 1 : 1;
            }


            foreach (var word in words)
            {
                stat.AddWord(word.Key);
            }
            var expect = words
                            .OrderByDescending(w => w.Value)
                            .ThenBy(w => w.Key)
                            .Select(pair => Tuple.Create(pair.Value, pair.Key));

            CollectionAssert.AreEqual(expect, stat.GetStatistics());
        }
    }
}