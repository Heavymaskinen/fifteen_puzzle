using System;
using System.Collections.Generic;
using System.Drawing;
using NUnit.Framework;
using TilePuzzle;

namespace TilePuzzleTest
{
    public class PuzzleTest
    {
        private const string INITIAL_MAP = "1 2 3 4\n" +
                                    "5 6 7 8\n" +
                                    "9 10 11 12\n" +
                                    "13 14 15 0";

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void InitWithSize()
        {
            var puzzle = new Puzzle(4, 5);
            Assert.AreEqual(4, puzzle.Width);
            Assert.AreEqual(5, puzzle.Height);
        }

        [Test]
        public void UnshuffledHasAscendingNumbersAndEmpty()
        {
            var puzzle = new Puzzle(4, 4);
            string map = puzzle.ToMap();
            var expected = INITIAL_MAP;
            Assert.AreEqual(expected, map);
        }

        [Test]
        public void StartLowerRightCorner()
        {
            var puzzle = new Puzzle(4, 4);
            Assert.AreEqual(new Point(4, 4), puzzle.Position);
        }

        [Test]
        public void MoveUp_PositionChanged()
        {
            var puzzle = new Puzzle(4, 4);
            puzzle.MoveUp();
            Assert.AreEqual(new Point(4, 3), puzzle.Position);
        }

        [Test]
        public void MoveUp_PlacesSwapped()
        {
            var puzzle = new Puzzle(4, 4);
            var expected = "1 2 3 4\n" +
                            "5 6 7 8\n" +
                            "9 10 11 0\n" +
                            "13 14 15 12";

            puzzle.MoveUp();
            Assert.AreEqual(expected, puzzle.ToMap());
        }

        [Test]
        public void MoveWithPoint()
        {
            var puzzle = new Puzzle(4, 4);
            var expected = "1 2 3 4\n" +
                            "5 6 7 8\n" +
                            "9 10 11 0\n" +
                            "13 14 15 12";
            puzzle.Move(new Point(4, 3));
            Assert.AreEqual(expected, puzzle.ToMap());

        }

        [Test]
        public void MoveLeft_PlacesSwapped()
        {
            var puzzle = new Puzzle(4, 4);
            var expected = "1 2 3 4\n" +
                            "5 6 7 8\n" +
                            "9 10 11 12\n" +
                            "13 14 0 15";

            puzzle.MoveLeft();
            Assert.AreEqual(expected, puzzle.ToMap());
        }

        [Test]
        public void MoveRight_PlacesSwapped()
        {
            var puzzle = new Puzzle(4, 4);
            puzzle.MoveLeft();
            puzzle.MoveRight();
            Assert.AreEqual(INITIAL_MAP, puzzle.ToMap());
        }

        [Test]
        public void MoveDown_PlacesSwapped()
        {
            var puzzle = new Puzzle(4, 4);
            var expected = "1 2 3 4\n" +
                            "5 6 7 8\n" +
                            "9 10 11 0\n" +
                            "13 14 15 12";

            puzzle.MoveUp();
            puzzle.MoveUp();
            puzzle.MoveDown();
            Assert.AreEqual(expected, puzzle.ToMap());
        }

        [Test]
        public void EnforceUpperVertBounds()
        {
            var puzzle = new Puzzle(4, 4);
            puzzle.MoveDown();
            string map = puzzle.ToMap();

            Assert.AreEqual(INITIAL_MAP, map);
        }

        [Test]
        public void EnforceLowerVertBounds()
        {
            var puzzle = new Puzzle(4, 4);
            puzzle.MoveUp();
            puzzle.MoveUp();
            puzzle.MoveUp();
            puzzle.MoveUp();

            string map = puzzle.ToMap();

            var expected = "1 2 3 0\n" +
                            "5 6 7 4\n" +
                            "9 10 11 8\n" +
                            "13 14 15 12";

            Assert.AreEqual(expected, map);
        }

        [Test]
        public void EnforceUpperHorzBounds()
        {
            var puzzle = new Puzzle(4, 4);
            puzzle.MoveRight();
            string map = puzzle.ToMap();
            Assert.AreEqual(INITIAL_MAP, map);
        }

        [Test]
        public void EnforceLowerHorzBounds()
        {
            var puzzle = new Puzzle(4, 4);
            var map = "1 2 3 4\n" +
                      "5 6 7 8\n" +
                      "9 10 11 12\n" +
                      "0 13 14 15";
            puzzle.MoveLeft();
            puzzle.MoveLeft();
            puzzle.MoveLeft();
            puzzle.MoveLeft();
            Assert.AreEqual(map, puzzle.ToMap());
        }

        [Test]
        public void BlockLongMoves()
        {
            var puzzle = new Puzzle(4, 4);
            puzzle.Move(new Point(1, 1));
            Assert.AreEqual(new Point(4, 4), puzzle.Position);
        }

        [Test]
        public void BlockDiagonalMoves()
        {
            var puzzle = new Puzzle(4, 4);
            var result = puzzle.Move(new Point(3, 3));
            Assert.IsFalse(result);
        }

        [Test]
        public void InvalidMovesReturnFalse_ValidReturnTrue()
        {
            var puzzle = new Puzzle(4, 4);
            var result = puzzle.Move(new Point(1, 1));
            Assert.IsFalse(result);
        }

        [Test]
        public void CompleteSequence_PuzzleCompleted()
        {
            var puzzle = new Puzzle(4, 4);
            Assert.IsTrue(puzzle.IsComplete());
        }

        [Test]
        public void IncompleteSequence_PuzzleNotCompleted()
        {
            var puzzle = new Puzzle(4, 4);
            puzzle.MoveUp();
            puzzle.MoveLeft();
            puzzle.MoveDown();
            puzzle.MoveRight();
            Assert.IsFalse(puzzle.IsComplete());
        }

        [Test]
        public void Shuffle_MakePuzzleIncomplete()
        {
            var puzzle = new Puzzle(4, 4);
            puzzle.Shuffle();
            Assert.IsFalse(puzzle.IsComplete());
        }

        [Test]
        public void Shuffle_MakeRandom()
        {
            var puzzle = new Puzzle(4, 4);
            puzzle.Shuffle();
            var map = puzzle.ToMap();
            var fails = 0;
            for (var i = 0; i < 100; i++)
            {
                puzzle = new Puzzle(4, 4);
                puzzle.Shuffle();
                if (map.Equals(puzzle.ToMap()))
                {
                    fails++;
                }
            }

            Assert.Less(fails, 5, "More than 4 repititions in 100");
        }
    }
}