using System;
using NUnit.Framework;

namespace Kontur.Courses.Testing.Tdd
{
    class Frame
    {
        private int _firstRole;
        private int _secondRole;

        public int FirstRoll
        {
            get { return _firstRole; }
            set
            {
                if (value > 10)
                    throw new Exception();
                _firstRole = value;
            }
        }

        public int SecondRoll
        {
            get { return _secondRole; }
            set
            {
                if (value + _firstRole > 10)
                    throw new Exception();
                _secondRole = value;
            }
        }

        public int Sum
        {
            get { return FirstRoll + SecondRoll; }
        }
        public bool IsSpare
        {
            get { return FirstRoll != 10 && FirstRoll + SecondRoll == 10; }
        }

        public bool IsStrike
        {
            get { return FirstRoll == 10; }
        }
    }

    [TestFixture]
    class FrameClass_should
    {

        [TestCase(10, 0, Result = true)]
        [TestCase(7, 3, Result = false)]
        public bool determineStike(int firstRoll, int secondRoll)
        {
            var frame = new Frame
            {
                FirstRoll = firstRoll,
                SecondRoll = secondRoll
            };
            return frame.IsStrike;
        }

        [TestCase(1, 5, Result = false)]
        [TestCase(7, 3, Result = true)]
        public bool determineSpare(int firstRoll, int secondRoll)
        {
            var frame = new Frame
            {
                FirstRoll = firstRoll,
                SecondRoll = secondRoll
            };
            return frame.IsSpare;
        }
    }

    public class Game
    {
        private readonly Frame[] _frames;
        private int _currentFrame;
        private bool _isSecondRollOfFrame;

        public Game()
        {
            _frames = new Frame[10];
            for (var i = 0; i < 10; i++)
                _frames[i] = new Frame();
        }

        public void Roll(int pins)
        {
            if (_frames[_currentFrame].IsStrike)
            {
                _currentFrame++;
                _isSecondRollOfFrame = false;
            }
            if (_isSecondRollOfFrame)
            {
                _frames[_currentFrame].SecondRoll = pins;
                _isSecondRollOfFrame = false;
                _currentFrame++;
            }
            else
            {
                _frames[_currentFrame].FirstRoll = pins;
                _isSecondRollOfFrame = true;
            }
        }


        public int GetScore()
        {
            var score = _frames[0].Sum;
            for (int i = 1; i < 10; i++)
            {
                if (_frames[i - 1].IsSpare)
                    score += _frames[i].FirstRoll;
                if (_frames[i - 1].IsStrike)
                    score += _frames[i].Sum;
                score += _frames[i].Sum;
            }
            return score;
        }
    }

    [TestFixture]
    public class BowlingGame_GetScore_should
    {
        [Test]
        public void returnZero_beforeAnyRolls()
        {
            var game = new Game();
            Assert.AreEqual(0, game.GetScore());
        }


        [TestCase(1, 2, 3, 4, 5, 6, 7, 8, 9, 8, ExpectedException = typeof(Exception))]
        [TestCase(1, 2, 3, 4, 5, 4, 7, 1, 7, 2, Result = 36)]
        public int returnSumScoresByAllRolls(params int[] pinsForFrames)
        {
            var game = new Game();

            foreach (var pins in pinsForFrames)
            {
                game.Roll(pins);
            }

            return game.GetScore();
        }

        [Test]
        public void returnSum_byOneFrame()
        {
            var game = new Game();

            game.Roll(5);
            game.Roll(4);

            Assert.AreEqual(9, game.GetScore());
        }

        [Test]
        public void returnIncreaseSum_afterSpare()
        {
            var game = new Game();

            game.Roll(5);
            game.Roll(5);
            game.Roll(7);

            Assert.AreEqual(24, game.GetScore());
        }

        [Test]
        public void returnIncreaseSum_afterStrike()
        {
            var game = new Game();
            game.Roll(10);
            game.Roll(2);
            game.Roll(7);

            Assert.AreEqual(game.GetScore(), 28);
        }
    }
}
