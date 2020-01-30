using System;
using System.Linq;
using System.Collections.Generic;
using AdventOfCode;
using System.IO;
using System.Text.RegularExpressions;

namespace aoc2018
{
    public enum GuardAction
    {
        StartShift,
        FallAsleep,
        WakeUp
    }

    public class GuardEvent
    {
        public int GuardId { get; set; }
        public DateTime EventTime { get; set; }
        public GuardAction Action { get; set; }
    }

    public class Day4
    {
        // guardId -> (minute -> times asleep) dictionary
        Dictionary<int, Dictionary<int, int>> guardStatuses = new Dictionary<int, Dictionary<int, int>>();

        public Day4()
        {
            var lines = InputUtils.GetDayInputLines(2018, 4);

            List<GuardEvent> events = new List<GuardEvent>();
            Regex r = new Regex(@"\[(.+)\](.+)");

            foreach (var line in lines)
            {
                var match = r.Match(line);

                GuardEvent ge = new GuardEvent();
                ge.EventTime = DateTime.Parse(match.Groups[1].Value);

                string actionString = match.Groups[2].Value;
                if (actionString.Contains("begins"))
                {
                    // e.g.
                    // Guard #1783 begins shift
                    ge.Action = GuardAction.StartShift;
                    ge.GuardId = int.Parse(actionString.Split('#')[1].Substring(0, 4).Trim());
                }
                else if (actionString.Contains("wakes"))
                {
                    ge.Action = GuardAction.WakeUp;
                }
                else if (actionString.Contains("falls"))
                {
                    ge.Action = GuardAction.FallAsleep;
                }

                events.Add(ge);
            }

            var orderedEvents = events.OrderBy(p => p.EventTime);

            int lastGuardId = 0;
            foreach (var oe in orderedEvents)
            {
                if (oe.GuardId == 0)
                {
                    oe.GuardId = lastGuardId;
                }
                else
                {
                    lastGuardId = oe.GuardId;
                }
            }

            // find the guard that has the most minutes
            // asleep, and what minute he spends asleep the most often

            // check each pair of events
            // shift start -> sleep: add awake minutes
            // sleep -> wake: add sleep minutes
            // wake -> sleep: add awake minutes
            // wake up -> shift start: add awake minutes
            // sleep -> shift start: add sleep minutes
            var eventsList = orderedEvents.ToList();
            for (int i = 0; i < eventsList.Count - 1; i++)
            {
                if (!guardStatuses.ContainsKey(eventsList[i].GuardId))
                {
                    guardStatuses.Add(eventsList[i].GuardId, new Dictionary<int, int>());
                }

                var e = eventsList[i];
                var eNext = eventsList[i + 1];

                if (e.Action == GuardAction.FallAsleep)
                {
                    for (int m = e.EventTime.Minute; m < eNext.EventTime.Minute; m++)
                    {
                        if (guardStatuses[e.GuardId].ContainsKey(m))
                        {
                            guardStatuses[e.GuardId][m]++;
                        }
                        else
                        {
                            guardStatuses[e.GuardId].Add(m, 1);
                        }
                    }
                }
            }
        }

        public int Part1()
        {
            // part 1 - guard that has the most minutes asleep
            int counter = 0;
            int guardId = 0;
            foreach (var gs in guardStatuses)
            {
                int cnt = gs.Value.Sum(p => p.Value);
                if (cnt > counter)
                {
                    counter = cnt;
                    guardId = gs.Key;
                }
            }

            // which minute the guard is asleep the most
            int guardMax = guardStatuses[guardId].Values.Max();
            int guardMinute = guardStatuses[guardId].First(p => p.Value == guardMax).Key;

            return guardId * guardMinute;
        }

        public int Part2()
        {
            // Strategy 2: Of all guards, which guard is most frequently asleep on the same minute?
            // What is the ID of the guard you chose multiplied by the minute you chose?

            // highest value of guardStatuses[*][*]
            int highest = 0;
            int part2GuardId = 0;
            int part2Minute = 0;
            foreach (var g in guardStatuses)
            {
                foreach (var gg in g.Value)
                {
                    if (gg.Value > highest)
                    {
                        highest = gg.Value;
                        part2GuardId = g.Key;
                        part2Minute = gg.Key;
                    }
                }
            }

            return part2GuardId * part2Minute;
        }
    }
}
