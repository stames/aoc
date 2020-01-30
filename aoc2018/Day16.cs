using System;
using System.Linq;
using System.Collections.Generic;
using AdventOfCode;
using System.IO;
using System.Text;

namespace aoc2018
{
    public class Day16
    {
        class Operation
        {
            public Operation()
            {
                BeforeRegisters = new int[4];
                InputInstructions = new int[4];
                AfterRegisters = new int[4];
            }

            public int[] BeforeRegisters { get; set; }
            public int[] InputInstructions { get; set; }
            public int[] AfterRegisters { get; set; }
        }

        int[] registers;
		List<Action<int, int, int>> funcs = new List<Action<int, int, int>>();
		Dictionary<int, string> finalMap = new Dictionary<int, string>();
		int behavesLikeThreeOrMore = 0;

		public Day16()
        {
			registers = new int[4];

			// read input
			var lines = InputUtils.GetDayInputLines(2018, 16);

			List<Operation> operations = new List<Operation>();

			for (int i = 0; i < lines.Count; i += 4)
			{
				var set = lines.Skip(i).Take(4).ToList();

				if (!set[0].StartsWith("Before"))
				{
					break;
				}

				Operation o = new Operation();

				int[] beforeRegs = new int[4];
				int[] inputInstructions = new int[4];
				int[] afterRegs = new int[4];

				o.BeforeRegisters = set[0].Substring(9, 10).Split(',').Select(int.Parse).ToArray();
				o.InputInstructions = set[1].Split(' ').Select(int.Parse).ToArray();
				o.AfterRegisters = set[2].Substring(9, 10).Split(',').Select(int.Parse).ToArray();

				operations.Add(o);
			}

			funcs.Add(addr);
			funcs.Add(addi);
			funcs.Add(mulr);
			funcs.Add(muli);
			funcs.Add(banr);
			funcs.Add(bani);
			funcs.Add(borr);
			funcs.Add(bori);
			funcs.Add(setr);
			funcs.Add(seti);
			funcs.Add(gtir);
			funcs.Add(gtri);
			funcs.Add(gtrr);
			funcs.Add(eqir);
			funcs.Add(eqri);
			funcs.Add(eqrr);

			
			Dictionary<int, HashSet<string>> possibleMap = new Dictionary<int, HashSet<string>>();
			for (int i = 0; i < 16; i++)
			{
				possibleMap.Add(i, new HashSet<string>());
			}

			foreach (var op in operations)
			{
				int count = 0;
				foreach (var f in funcs)
				{
					for (int i = 0; i < 4; i++)
					{
						registers[i] = op.BeforeRegisters[i];
					}

					f(op.InputInstructions[1], op.InputInstructions[2], op.InputInstructions[3]);

					if (registers.SequenceEqual(op.AfterRegisters))
					{
						count++;

						// possible match

						possibleMap[op.InputInstructions[0]].Add(f.Method.Name);
					}
				}
				if (count >= 3)
				{
					behavesLikeThreeOrMore++;
				}
			}

			while (finalMap.Count < 16)
			{
				foreach (var kvp in possibleMap)
				{
					if (!finalMap.ContainsKey(kvp.Key))
					{
						if (kvp.Value.Count == 1)
						{
							// add to final
							string str = kvp.Value.First();
							finalMap.Add(kvp.Key, str);

							foreach (var k2 in possibleMap)
							{
								k2.Value.Remove(str);
							}
						}
					}
				}
			}
		}

        public int Part1()
		{ 
			return behavesLikeThreeOrMore;
        }

        public int Part2()
        {
			var part2 = InputUtils.GetDayInputLines(2018, 16).Skip(3097);

			registers = new[] { 0, 0, 0, 0 };
			foreach (var line in part2)
			{
				if (String.IsNullOrEmpty(line)) continue;

				// get function
   				int[] inputValues = new int[4];
				inputValues = line.Split(' ').Select(int.Parse).ToArray();

				string funcName = finalMap[inputValues[0]];
				Action<int, int, int> action = funcs.First(p => p.Method.Name == funcName);

				action(inputValues[1], inputValues[2], inputValues[3]);
			}

			for (int i = 0; i < 4; i++)
			{
				Console.Write(registers[i] + " ");
			}

			return registers[0];
        }

		void addr(int a, int b, int c)
		{
			registers[c] = registers[a] + registers[b];
		}

		void addi(int a, int b, int c)
		{
			registers[c] = registers[a] + b;
		}

		void mulr(int a, int b, int c)
		{
			registers[c] = registers[a] * registers[b];
		}

		void muli(int a, int b, int c)
		{
			registers[c] = registers[a] * b;
		}

		void banr(int a, int b, int c)
		{
			registers[c] = registers[a] & registers[b];
		}

		void bani(int a, int b, int c)
		{
			registers[c] = registers[a] & b;
		}

		void borr(int a, int b, int c)
		{
			registers[c] = registers[a] | registers[b];
		}

		void bori(int a, int b, int c)
		{
			registers[c] = registers[a] | b;
		}

		void setr(int a, int b, int c)
		{
			registers[c] = registers[a];
		}

		void seti(int a, int b, int c)
		{
			registers[c] = a;
		}

		void gtir(int a, int b, int c)
		{
			registers[c] = (a > registers[b] ? 1 : 0);
		}

		void gtri(int a, int b, int c)
		{
			registers[c] = (registers[a] > b ? 1 : 0);
		}

		void gtrr(int a, int b, int c)
		{
			registers[c] = (registers[a] > registers[b] ? 1 : 0);
		}

		void eqir(int a, int b, int c)
		{
			registers[c] = (a == registers[b] ? 1 : 0);
		}

		void eqri(int a, int b, int c)
		{
			registers[c] = (registers[a] == b ? 1 : 0);
		}

		void eqrr(int a, int b, int c)
		{
			registers[c] = (registers[a] == registers[b] ? 1 : 0);
		}
	}
}
