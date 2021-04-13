namespace Snake
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    struct Position
    {
        public int row;
        public int col;

        public Position(int row, int col)
        {
            this.row = row;
            this.col = col;
        }
    }

    public class Program
    {
        public static void Main()
        {
            int right = 0;
            int left = 1;
            int down = 2;
            int up = 3;

            int lastFoodTime = 0;
            int foodDissapearTime = 8000;

            double sleepTime = 50;
            int finalScore = 0;

            Position[] directions = new Position[]
            {
                new Position(0, 1), // right
                new Position(0, -1), // left
                new Position(1, 0), // down
                new Position(-1, 0), // up
            };

            var currDirection = right;
            //Console.BufferHeight = Console.WindowHeight;

            Random randomNumberGenerator = new Random();
            Position food = new Position(randomNumberGenerator.Next(0, Console.WindowHeight),
                                         randomNumberGenerator.Next(0, Console.WindowWidth));
            lastFoodTime = Environment.TickCount;
            Console.SetCursorPosition(food.col, food.row);
            Console.Write("@");

            Queue<Position> snakeElements = new Queue<Position>();
            for (int i = 0; i <= 5; i++)
            {
                snakeElements.Enqueue(new Position(0, i));
            }

            foreach (Position position in snakeElements)
            {
                Console.SetCursorPosition(position.col, position.row);
                Console.WriteLine("*");
            }

            while (true)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo userInput = Console.ReadKey();
                    if (userInput.Key == ConsoleKey.UpArrow)
                    {
                        if (currDirection != down)
                        {
                            currDirection = up;
                        }
                    }
                    else if (userInput.Key == ConsoleKey.DownArrow)
                    {
                        if (currDirection != up)
                        {
                            currDirection = down;
                        }
                    }
                    else if (userInput.Key == ConsoleKey.LeftArrow)
                    {
                        if (currDirection != right)
                        {
                            currDirection = left;
                        }
                    }
                    else if (userInput.Key == ConsoleKey.RightArrow)
                    {
                        if (currDirection != left)
                        {
                            currDirection = right;
                        }
                    }
                }

                Position head = snakeElements.Last();
                Position nextDirection = directions[currDirection];
                Position snakeNewHead = new Position
                    (head.row + nextDirection.row, head.col + nextDirection.col);

                //if (snakeNewHead.col < 0) snakeNewHead.col = Console.WindowWidth - 1;
                //if (snakeNewHead.row < 0) snakeNewHead.row = Console.WindowHeight - 1;
                //if (snakeNewHead.col >= Console.WindowWidth) snakeNewHead.col = 0;
                //if (snakeNewHead.row >= Console.WindowHeight) snakeNewHead.row = 0;

                if (snakeNewHead.row < 0 ||
                    snakeNewHead.col < 0 ||
                    snakeNewHead.row >= Console.WindowHeight ||
                    snakeNewHead.col >= Console.WindowWidth ||
                    snakeElements.Contains(snakeNewHead))
                {

                    Console.SetCursorPosition(0, 0);
                    Console.WriteLine("Game Over!");
                    if (finalScore < 0)
                    {
                        Console.WriteLine($"Score: 0");
                    }
                    else
                    {
                        Console.WriteLine($"Score: {finalScore}");
                    }
                    return;
                }
                snakeElements.Enqueue(snakeNewHead);
                Console.SetCursorPosition(snakeNewHead.col, snakeNewHead.row);
                Console.Write("*");

                if (snakeNewHead.row == food.row && snakeNewHead.col == food.col)
                {
                    do
                    {
                        // feeding the snake
                        food = new Position(
                            randomNumberGenerator.Next(0, Console.WindowHeight),
                            randomNumberGenerator.Next(0, Console.WindowWidth));
                        lastFoodTime = Environment.TickCount;
                    }
                    while (snakeElements.Contains(food));

                    finalScore += 100;
                    sleepTime -= 0.01;
                }
                else
                {
                    Position last = snakeElements.Dequeue();
                    Console.SetCursorPosition(last.col, last.row);
                    Console.Write(" ");
                }

                //foreach (Position position in snakeElements)
                //{
                //    Console.SetCursorPosition(position.col, position.row);
                //    Console.Write("*");
                //}

                //Console.SetCursorPosition(food.col, food.row);
                //Console.Write("@");

                if (Environment.TickCount - lastFoodTime >= foodDissapearTime)
                {
                    finalScore -= 50;
                    Console.SetCursorPosition(food.col, food.row);
                    Console.Write(" ");
                    do
                    {
                        // feeding the snake
                        food = new Position(
                            randomNumberGenerator.Next(0, Console.WindowHeight),
                            randomNumberGenerator.Next(0, Console.WindowWidth));
                    }
                    while (snakeElements.Contains(food));
                    lastFoodTime = Environment.TickCount;
                }

                Console.SetCursorPosition(food.col, food.row);
                Console.Write("@");

                Console.CursorVisible = false;
                sleepTime -= 0.01;
                Thread.Sleep((int)sleepTime);
            }
        }
    }
}
