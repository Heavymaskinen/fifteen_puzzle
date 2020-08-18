using System;
using System.Drawing;

namespace TilePuzzle
{
    public class Puzzle
    {
        private int[,] tiles;
        private Point position;

        public int Width { get; }
        public int Height { get; }
        public Point Position => position;

        public Puzzle(int width, int height)
        {
            Width = width;
            Height = height;
            BuildMap();
        }

        private void BuildMap()
        {
            tiles = new int[Width, Height];

            var count = 1;
            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    tiles[x, y] = count;
                    count++;

                    if (count == Height * Width)
                    {
                        tiles[Width - 1, Height - 1] = 0;
                        position = new Point(Width, Height);
                        break;
                    }
                }
            }
        }

        public string ToMap()
        {
            var map = "";
            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    map += tiles[x, y] + " ";
                }

                map = map.TrimEnd() + "\n";
            }

            return map.TrimEnd();
        }

        public void MoveUp()
        {
            Move(new Point(position.X, position.Y - 1));
        }

        public void MoveLeft()
        {
            Move(new Point(position.X - 1, position.Y));
        }

        public void MoveDown()
        {
            Move(new Point(position.X, position.Y + 1));
        }

        public void MoveRight()
        {
            Move(new Point(position.X + 1, position.Y));
        }

        public bool IsComplete()
        {
            if (Position.X != Width || Position.Y != Height)
            {
                return false;
            }

            var prev = -1;

            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    var i = tiles[x, y];

                    if (prev == -1 || i == 0)
                    {
                        prev = i;
                        continue;
                    }

                    if (i < prev)
                    {
                        return false;
                    }

                    prev = i;
                }
            }

            return true;
        }

        public void Shuffle()
        {

            var rand = new Random();
            var rand2 = new Random();

            for (var i = 0; i < 80; i++)
            {
                var shuffX = rand.Next(-1, 2);
                var shuffY = rand2.Next(-1, 2);
                var shuffPos = new Point(position.X + shuffX, position.Y + shuffY);
                var result = Move(shuffPos);
                if (!result)
                {
                   // i--;
                }
            }
        }

        public bool Move(Point newPosition)
        {
            if (IsOutOfBounds(newPosition))
            {
                return false;
            }

            if (IsInvalid(newPosition))
            {
                return false;
            }

            if (IsDiagonal(newPosition))
            {
                return false;
            }

            if (newPosition.Equals(position))
            {
                return false;
            }

            var val = tiles[position.X - 1, position.Y - 1];
            tiles[position.X - 1, position.Y - 1] = tiles[newPosition.X - 1, newPosition.Y - 1];
            tiles[newPosition.X - 1, newPosition.Y - 1] = val;
            position = newPosition;

            return true;
        }

        private bool IsDiagonal(Point newPosition)
        {
            return Math.Abs(position.X - newPosition.X) == 1 && Math.Abs(position.Y - newPosition.Y) == 1;
        }

        private bool IsInvalid(Point newPosition)
        {
            return Math.Abs(newPosition.X - position.X) > 1 ||
                            Math.Abs(newPosition.Y - position.Y) > 1;
        }

        private bool IsOutOfBounds(Point newPosition)
        {
            return newPosition.Y - 1 >= Height || newPosition.Y - 1 < 0
                            || newPosition.X - 1 >= Width || newPosition.X - 1 < 0;
        }
    }
}
