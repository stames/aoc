using System;
using System.Linq;
using System.Collections.Generic;
using AdventOfCode;
using System.IO;

namespace aoc2018
{
    public class Day8
    {
        public class Node
        {
            public Node()
            {
                ChildNodes = new List<Node>();
                MetadataEntries = new List<int>();
            }

            public int ChildNodeCount { get; set; }
            public int MetadataCount { get; set; }
            public List<Node> ChildNodes { get; set; }
            public List<int> MetadataEntries { get; set; }
        }

        public Day8()
        {

        }

        public int Part1()
        {
            var input = InputUtils.GetDayInputString(2018, 8);
            List<int> inputInts = input.Split(' ').Select(p => int.Parse(p)).ToList();

            Node root = BuildTree(inputInts);

            // add up the sum
            int sum = GetMetadataSum(root);
            return sum;
        }

        public int Part2()
        {
            var input = InputUtils.GetDayInputString(2018, 8);
            List<int> inputInts = input.Split(' ').Select(p => int.Parse(p)).ToList();

            Node root = BuildTree(inputInts);

            int sum = GetNodeValue(root);
            return sum;
        }

        private int GetNodeValue(Node node)
        {
            int value = 0;

            if (node.ChildNodeCount == 0)
            {
                value = node.MetadataEntries.Sum();
            }
            else
            {
                foreach (int me in node.MetadataEntries)
                {
                    // me is 1-based index into children
                    if (me <= node.ChildNodeCount)
                    {
                        value += GetNodeValue(node.ChildNodes[me - 1]);
                    }
                }
            }

            return value;
        }


        private int GetMetadataSum(Node node)
        {
            int sum = node.MetadataEntries.Sum();

            foreach(var n in node.ChildNodes)
            {
                sum += GetMetadataSum(n);
            }

            return sum;
        }

        private Node BuildTree(List<int> input)
        {
            Node node = new Node();
            node.ChildNodeCount = input[0];
            node.MetadataCount = input[1];

            input.RemoveAt(0);
            input.RemoveAt(0);

            for (int i = 0; i < node.ChildNodeCount; i++)
            {
                Node n = BuildTree(input);
                node.ChildNodes.Add(n);
            }

            for (int i = 0; i < node.MetadataCount; i++)
            {
                node.MetadataEntries.Add(input[0]);
                input.RemoveAt(0);
            }

            return node;
        }

        
    }
}
