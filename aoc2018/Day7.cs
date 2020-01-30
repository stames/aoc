using System;
using System.Linq;
using System.Collections.Generic;
using AdventOfCode;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace aoc2018
{
    public class Step
    {
        public char StepId { get; set; }

        public int TimeTaken
        {
            get
            {
                return 61 + (StepId - 'A');
            }
        }

        // what must be finished before step is done
        public HashSet<char> StepsThisDependsOn { get; set; }

        // what steps this must be finished before it starts
        public HashSet<char> StepsThisIsInputTo { get; set; }
    }

    public class Day7
    {
        List<string> lines = new List<string>();
        public Day7()
        {
            lines = InputUtils.GetDayInputLines(2018, 7);
        }

        public int Part1()
        {
            List<Step> steps = new List<Step>();

            for (char c = 'A'; c <= 'Z'; c++)
            {
                Step step = new Step();
                step.StepId = c;
                steps.Add(step);
            }

            foreach (var line in lines)
            {
                char mustBeFinishedStep = line[5];
                char canBeginStep = line[36];

                if (steps.First(p => p.StepId == canBeginStep).StepsThisDependsOn == null)
                {
                    steps.First(p => p.StepId == canBeginStep).StepsThisDependsOn = new HashSet<char>();
                }
                steps.First(p => p.StepId == canBeginStep).StepsThisDependsOn.Add(mustBeFinishedStep);

                if (steps.First(p => p.StepId == mustBeFinishedStep).StepsThisIsInputTo == null)
                {
                    steps.First(p => p.StepId == mustBeFinishedStep).StepsThisIsInputTo = new HashSet<char>();
                }
                steps.First(p => p.StepId == mustBeFinishedStep).StepsThisIsInputTo.Add(canBeginStep);

            }

            string output = String.Empty;

            while (true)
            {
                // which step has nothing that needs to happen before it?
                var s = steps.Where(p => p.StepsThisDependsOn == null || p.StepsThisDependsOn.Count == 0);

                if (!s.Any())
                {
                    break;
                }

                var sorted = s.OrderBy(p => p.StepId);
                char stepID = sorted.First().StepId;
                output += stepID;
                steps.RemoveAll(p => p.StepId == stepID);

                foreach (var step in steps)
                {
                    if (step.StepsThisDependsOn != null)
                    {
                        if (step.StepsThisDependsOn.Contains(stepID))
                        {
                            step.StepsThisDependsOn.Remove(stepID);
                        }
                    }
                }
            }

            Console.WriteLine(output);
            return 0;
        }

        public int Part2()
        {
            // part 2 -- loop through second by second, and keep track of the
            // 5 workers in progress at each second. on each tick, identify
            // the state and what has finished, picking the next one from the stack/
            // the same way as part 1
            var dependencies = new List<(string pre, string post)>();

            lines.ToList().ForEach(x => dependencies.Add((x[5].ToString(), x[36].ToString())));

            var allSteps = dependencies.Select(x => x.pre).Concat(dependencies.Select(x => x.post)).Distinct().OrderBy(x => x).ToList();

            // map each worker to its time
            var workers = new List<int> { 0, 0, 0, 0, 0 };

            int currentSecond = 0;

            // when each step finishes
            var doneList = new List<(string step, int finish)>();

            while (allSteps.Any() || workers.Any(w => w > currentSecond))
            {
                doneList.Where(d => d.finish <= currentSecond).ToList().ForEach(x => dependencies.RemoveAll(d => d.pre == x.step));
                doneList.RemoveAll(d => d.finish <= currentSecond);

                var valid = allSteps.Where(s => !dependencies.Any(d => d.post == s)).ToList();

                for (var w = 0; w < workers.Count && valid.Any(); w++)
                {
                    if (workers[w] <= currentSecond)
                    {
                        workers[w] = GetWorkTime(valid.First()) + currentSecond;
                        allSteps.Remove(valid.First());
                        doneList.Add((valid.First(), workers[w]));
                        valid.RemoveAt(0);
                    }
                }

                currentSecond++;
            }

            return currentSecond;
        }

        private static int GetWorkTime(string v)
        {
            return (v[0] - 'A') + 61;
        }
    }
}
