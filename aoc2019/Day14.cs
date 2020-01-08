using System;
using System.Linq;
using System.Collections.Generic;
using AdventOfCode;

namespace aoc2019
{
    public class Nanofactory
    {
        // map output (key = chemical name) to the reaction that makes it
        public Dictionary<string, Reaction> Reactions = new Dictionary<string, Reaction>();

        // track extra stuff that's unused by reactions
        public Dictionary<string, int> ExtraMaterials = new Dictionary<string, int>();
        public long OreCount;

        public Nanofactory(List<string> reactionList)
        {
            foreach (var l in reactionList)
            {
                List<InputOutputChemical> reactionInputs = new List<InputOutputChemical>();

                var equationSides = l.Split("=>");
                var outputChemicalStr = equationSides.Last().Trim().Split(' ').Last();
                int outputChemicalAmt = int.Parse(equationSides.Last().Trim().Split(' ').First());

                var outputProduction = new InputOutputChemical { Amount = outputChemicalAmt, Chemical = outputChemicalStr };

                var inputChemicals = equationSides.First().Split(',');
                foreach(var c in inputChemicals)
                {
                    var inputChemStr = c.Trim().Split(' ').Last();
                    int inputChemAmt = int.Parse(c.Trim().Split(' ').First());

                    reactionInputs.Add(new InputOutputChemical { Amount = inputChemAmt, Chemical = inputChemStr });
                }

                Reactions.Add(outputProduction.Chemical, new Reaction { Output = outputProduction, RequiredInputs = reactionInputs });
            }
        }

        public void RunFactory(InputOutputChemical request)
        {
            if (request.Chemical.Equals("ORE"))
            {
                OreCount += request.Amount;
                return;
            }

            // if we have extra
            if (ExtraMaterials.ContainsKey(request.Chemical))
            {
                if (ExtraMaterials[request.Chemical] < request.Amount)
                {
                    request.Amount -= ExtraMaterials[request.Chemical];
                    ExtraMaterials[request.Chemical] = 0;
                }
                else
                {
                    ExtraMaterials[request.Chemical] -= request.Amount;
                    return;
                }
            }

            var reaction = Reactions[request.Chemical];

            // how many times we need to run the reaction
            var reactionCount = (int)Math.Ceiling(request.Amount / (double)reaction.Output.Amount);

            // go through each input for the reaction that's needed
            // to create this amount of output
            foreach (var p in reaction.RequiredInputs)
            {
                RunFactory(new InputOutputChemical { Amount = p.Amount * reactionCount, Chemical = p.Chemical });
            }

            // if the reaction creates more output than was in the request,
            // store it for another potential use
            if (reaction.Output.Amount * reactionCount > request.Amount)
            {
                if (ExtraMaterials.ContainsKey(reaction.Output.Chemical))
                {
                    ExtraMaterials[reaction.Output.Chemical] += (reaction.Output.Amount * reactionCount) - request.Amount;
                }
                else
                {
                    ExtraMaterials.Add(reaction.Output.Chemical, (reaction.Output.Amount * reactionCount) - request.Amount);
                }
            }
        }
    }

    public struct InputOutputChemical
    {
        public int Amount { get; set; }
        public string Chemical { get; set; }
    }

    public class Reaction
    {
        public Reaction()
        {
            RequiredInputs = new List<InputOutputChemical>();
        }

        public List<InputOutputChemical> RequiredInputs { get; set; }
        public InputOutputChemical Output { get; set; }
    }

    public class Day14
    {
        public Day14()
        {
        }

        public long Part1()
        {
            var input = InputUtils.GetDayInputLines(14);
            Nanofactory r = new Nanofactory(input);
            r.RunFactory(new InputOutputChemical { Amount = 1, Chemical = "FUEL" });

            return r.OreCount;
        }

        public long Part2()
        {
            var input = InputUtils.GetDayInputLines(14);

            // a trillion ore
            const long oreStorage = 1_000_000_000_000;

            Nanofactory r = new Nanofactory(input);

            long fuelCount = 0;

            // produce lots at a time
            int fuelToProduce = 1000;
            Dictionary<string, int> tempExtra = null;
            long tempOreCount = 0;

            while (fuelToProduce >= 1)
            {
                while (r.OreCount < oreStorage)
                {
                    tempExtra = new Dictionary<string, int>(r.ExtraMaterials);
                    tempOreCount = r.OreCount;
                    r.RunFactory(new InputOutputChemical() { Amount = fuelToProduce, Chemical = "FUEL" });
                    fuelCount += fuelToProduce;
                }

                // it wasn't an exact match, scale back and rerun
                // the reactions to produce less fuel
                if (fuelToProduce >= 1)
                {
                    r.ExtraMaterials = new Dictionary<string, int>(tempExtra);
                    r.OreCount = tempOreCount;
                    fuelCount -= fuelToProduce;
                    fuelToProduce /= 10;
                }
            }

            return fuelCount;
        }
    }
}
