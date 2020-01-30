using System;
using System.Linq;
using System.Collections.Generic;
using AdventOfCode;
using System.IO;
using System.Text;

namespace aoc2018
{
    public class Day15
    {
		enum UnitType
		{
			Elf,
			Goblin
		}

		class Unit
		{
            public static int ElfPower { get; set; }
			public Unit()
			{
				HitPoints = 200;
			}

			public UnitType UnitType { get; set; }
			public int AttackPower
			{
				get
				{
					return UnitType == UnitType.Goblin ? 3 : ElfPower;
				}
			}

			public int HitPoints { get; set; }
			public Point Location { get; set; }
		}

		const int gridSize = 32;

		public Day15()
        {
        }

        public int Part1()
        {
			Unit.ElfPower = 3;
			return Simulate(true);
        }

		public int Part2()
		{
			Unit.ElfPower = 12;
			return Simulate(false);
		}

		public int Simulate(bool allowElfDeaths = true)
        {
			int roundsCompleted = 1;

			// false == wall
			// true == available
			bool[,] board = new bool[gridSize, gridSize];
			List<Unit> units = new List<Unit>();

			string[] lines = InputUtils.GetDayInputLines(2018, 15).ToArray();
			//string[] lines = File.ReadAllLines("/Users/jjacoby/testing/advent2018/day15test.txt");

			int rowIndex = 0;
			int columnIndex = 0;
			foreach (var line in lines)
			{
				columnIndex = 0;
				foreach (var c in line)
				{
					if (c == '#')
					{
						board[columnIndex, rowIndex] = false;
					}
					else if (c == '.')
					{
						board[columnIndex, rowIndex] = true;
					}
					else
					{
						board[columnIndex, rowIndex] = true;
						Unit u = new Unit();
						u.UnitType = (c == 'G' ? UnitType.Goblin : UnitType.Elf);
						u.Location = new Point(columnIndex, rowIndex);
						units.Add(u);
					}

					columnIndex++;
				}
				rowIndex++;
			}

			//Console.WriteLine(DrawBoard(board, units));

			while (true)
			{
				// units take turns in reading order
				var orderedUnits = units.OrderBy(p => p.Location.Y).ThenBy(p => p.Location.X).ToList();
				
				foreach (var unit in orderedUnits)
				{
					if (!allowElfDeaths)
					{
						if (unit.HitPoints <= 0 && unit.UnitType == UnitType.Elf)
						{
							Console.WriteLine("part 2 failed - elf died");
							return -1;
						}
					}

					if (unit.HitPoints <= 0)
					{
						continue;
					}

					if (units.Count(p => p.UnitType == UnitType.Elf && p.HitPoints > 0) == 0 ||
						units.Count(p => p.UnitType == UnitType.Goblin && p.HitPoints > 0) == 0)
					{
						//Console.WriteLine("End:");
						//Console.WriteLine(DrawBoard(board, units));

						//Console.WriteLine("Round {0}: no targets remaining", roundsCompleted);
						int totalHitPoints = units.Where(p => p.HitPoints > 0).Sum(p => p.HitPoints);
						//Console.WriteLine("Total hit points = {0}", totalHitPoints);
						//Console.WriteLine("Round * points = {0}", roundsCompleted * totalHitPoints);
						//Console.WriteLine("Round - 1 * points = {0}", (roundsCompleted - 1) * totalHitPoints);

    				    return (roundsCompleted - 1) * totalHitPoints;
					}


					bool didAttack = false;

					// first - can i attack?
					// find targets in range
					var targets = units.Where(p => p.UnitType != unit.UnitType && p.HitPoints > 0 &&
												   ((p.Location.X == unit.Location.X && Math.Abs(p.Location.Y - unit.Location.Y) == 1) ||
													(p.Location.Y == unit.Location.Y && Math.Abs(p.Location.X - unit.Location.X) == 1)))
													.OrderBy(p => p.HitPoints)
													.ThenBy(p => p.Location.Y)
													.ThenBy(p => p.Location.X);

					if (targets.Any())
					{
						var target = targets.First();
						target.HitPoints -= unit.AttackPower;

						didAttack = true;
					}

					if (!didAttack)
					{
						// otherwise, move
						// find possible locations to move
						// open spots next to enemies
						List<Point> openSpots = new List<Point>();
						for (int y = 0; y < gridSize; y++)
						{
							for (int x = 0; x < gridSize; x++)
							{
								if (board[x, y])
								{
									// not a wall
									// is it next to an enemy?
									if (units.Any(p => p.HitPoints > 0 && p.UnitType != unit.UnitType &&
												 ((p.Location.X == x && Math.Abs(p.Location.Y - y) == 1) ||
														 (p.Location.Y == y && Math.Abs(p.Location.X - x) == 1))))
									{
										// is there already a unit there?
										if (!units.Any(p => p.HitPoints > 0 && p.Location.X == x && p.Location.Y == y))
										{
											openSpots.Add(new Point(x, y));
										}
									}

								}
							}
						}

						// find open spots that have a clear path
						Dictionary<Point, int> spotDistances = new Dictionary<Point, int>();
						foreach (var spot in openSpots)
						{
							// is there a path from unit.Location to spot?  
							var distance = GetDistance(board, units, unit.Location, spot);
							if (distance >= 0)
							{
								spotDistances.Add(spot, distance);
							}
						}

						// closest spot
						if (!spotDistances.Any())
						{
							// can't move
							continue;
						}

						var chosenSpot = spotDistances.OrderBy(p => p.Value)
								.ThenBy(p => p.Key.Y)
								.ThenBy(p => p.Key.X)
								.First().Key;

						int nextX = unit.Location.X;
						int nextY = unit.Location.Y;
						// see which adjacent spot has the shortest distance

						int minDistance = int.MaxValue;
						// x,y-1
						if (board[unit.Location.X, unit.Location.Y - 1] &&
								!units.Any(p => p.HitPoints > 0
											 && p.Location.X == unit.Location.X
											 && p.Location.Y == unit.Location.Y - 1))
						{
							int distanceFrom = GetDistance(board, units, new Point(unit.Location.X, unit.Location.Y - 1), chosenSpot);
							if (distanceFrom >= 0 && (distanceFrom < minDistance))
							{
								minDistance = distanceFrom;
								nextX = unit.Location.X;
								nextY = unit.Location.Y - 1;
							}
						}
						// x-1,y
						if (board[unit.Location.X - 1, unit.Location.Y] &&
								!units.Any(p => p.HitPoints > 0
											 && p.Location.X == unit.Location.X - 1
											 && p.Location.Y == unit.Location.Y))
						{
							int distanceFrom = GetDistance(board, units, new Point(unit.Location.X - 1, unit.Location.Y), chosenSpot);
							if (distanceFrom >= 0 && (distanceFrom < minDistance))
							{
								minDistance = distanceFrom;
								nextX = unit.Location.X - 1;
								nextY = unit.Location.Y;
							}
						}
						// x+1,y
						if (board[unit.Location.X + 1, unit.Location.Y] &&
								!units.Any(p => p.HitPoints > 0
											 && p.Location.X == unit.Location.X + 1
											 && p.Location.Y == unit.Location.Y))
						{
							int distanceFrom = GetDistance(board, units, new Point(unit.Location.X + 1, unit.Location.Y), chosenSpot);
							if (distanceFrom >= 0 && (distanceFrom < minDistance))
							{
								minDistance = distanceFrom;
								nextX = unit.Location.X + 1;
								nextY = unit.Location.Y;
							}
						}
						// x,y+1
						if (board[unit.Location.X, unit.Location.Y + 1] &&
								!units.Any(p => p.HitPoints > 0
											 && p.Location.X == unit.Location.X
											 && p.Location.Y == unit.Location.Y + 1))
						{
							int distanceFrom = GetDistance(board, units, new Point(unit.Location.X, unit.Location.Y + 1), chosenSpot);
							if (distanceFrom >= 0 && (distanceFrom < minDistance))
							{
								minDistance = distanceFrom;
								nextX = unit.Location.X;
								nextY = unit.Location.Y + 1;
							}
						}


						unit.Location.X = nextX;
						unit.Location.Y = nextY;

						// now, can i attack again?
						var newTargets = units.Where(p => p.UnitType != unit.UnitType && p.HitPoints > 0 &&
													   ((p.Location.X == unit.Location.X && Math.Abs(p.Location.Y - unit.Location.Y) == 1) ||
														(p.Location.Y == unit.Location.Y && Math.Abs(p.Location.X - unit.Location.X) == 1)))
														.OrderBy(p => p.HitPoints)
														.ThenBy(p => p.Location.Y)
														.ThenBy(p => p.Location.X);

						if (newTargets.Any())
						{
							var target = newTargets.First();
							target.HitPoints -= unit.AttackPower;
						}
					}
				}

				//Console.WriteLine(roundsCompleted);
				//Console.WriteLine(DrawBoard(board, units));
				roundsCompleted++;
			}

			//return 0;
        }

        

		private int GetDistance(bool[,] board, List<Unit> units, Point source, Point dest)
		{
			bool[,] visited = new bool[gridSize, gridSize];

			for (int i = 0; i < gridSize; i++)
			{
				for (int j = 0; j < gridSize; j++)
				{
					if (!board[i, j] || units.Any(p => p.HitPoints > 0 && p.Location.X == i && p.Location.Y == j))
						visited[i, j] = true;
					else
						visited[i, j] = false;
				}
			}

			Queue<Point> queue = new Queue<Point>();
			Dictionary<Point, int> distances = new Dictionary<Point, int>();

			queue.Enqueue(source);
			visited[source.X, source.Y] = true;

			while (queue.Any())
			{
				Point p = queue.Dequeue();

				if (p.Equals(dest))
				{
					return distances.ContainsKey(p) ? distances[p] : 0;
				}

				if (p.Y - 1 >= 0 && !visited[p.X, p.Y - 1])
				{
					Point newPoint = new Point(p.X, p.Y - 1);
					queue.Enqueue(newPoint);

					int distance = distances.ContainsKey(p) ? distances[p] : 0;
					distances.Add(newPoint, distance + 1);

					visited[p.X, p.Y - 1] = true;
				}

				if (p.Y + 1 < gridSize && !visited[p.X, p.Y + 1])
				{
					Point newPoint = new Point(p.X, p.Y + 1);
					queue.Enqueue(newPoint);

					int distance = distances.ContainsKey(p) ? distances[p] : 0;
					distances.Add(newPoint, distance + 1);

					visited[p.X, p.Y + 1] = true;
				}

				if (p.X - 1 >= 0 && !visited[p.X - 1, p.Y])
				{
					Point newPoint = new Point(p.X - 1, p.Y);
					queue.Enqueue(newPoint);

					int distance = distances.ContainsKey(p) ? distances[p] : 0;
					distances.Add(newPoint, distance + 1);

					visited[p.X - 1, p.Y] = true;
				}

				if (p.X + 1 < gridSize && !visited[p.X + 1, p.Y])
				{
					Point newPoint = new Point(p.X + 1, p.Y);
					queue.Enqueue(newPoint);

					int distance = distances.ContainsKey(p) ? distances[p] : 0;
					distances.Add(newPoint, distance + 1);

					visited[p.X + 1, p.Y] = true;
				}
			}
			return -1;
		}

		private string DrawBoard(bool[,] board, List<Unit> units)
		{
			StringBuilder sb = new StringBuilder();
			for (int y = 0; y < gridSize; y++)
			{
				for (int x = 0; x < gridSize; x++)
				{
					if (!board[x, y])
					{
						sb.Append("#");
					}
					else
					{
						if (units.Any(p => p.HitPoints > 0 && p.Location.X == x && p.Location.Y == y))
						{
							if (units.First(p => p.HitPoints > 0 && p.Location.X == x && p.Location.Y == y).UnitType == UnitType.Goblin)
							{
								sb.Append("G");
							}
							else
							{
								sb.Append("E");
							}
						}
						else
						{
							sb.Append(".");
						}
					}
				}

				foreach (var u in units.Where(p => p.HitPoints > 0 && p.Location.Y == y).OrderBy(p => p.Location.X))
				{
					sb.AppendFormat(" {0}({1}),", u.UnitType, u.HitPoints);
				}
				sb.AppendLine();
			}
			return sb.ToString();
		}
	}
}
