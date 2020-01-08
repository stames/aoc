using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AdventOfCode
{
    public enum ComputerState
    {
        Halted,
        WaitingForInput,
        Running,
        None
    }
    public class IntcodeComputer
    {
        public int NextInput { get; set; }
        public bool IsHalted { get; set; }
        public long OutputValue { get; set; }
        public ComputerState State { get; set; }

        private Dictionary<long, long> programMemory;
        private long currentPointer = 0;
        private long relativeBase = 0;
        private bool debugMode;
        private string fileName;

        private Queue<long> inputQueue;
        private Queue<long> outputQueue;

        public IntcodeComputer(string fileName, bool debugMode = false)
        {
            this.fileName = fileName;
            this.debugMode = debugMode;
            Reboot();
        }

        public void Reboot()
        {
            string[] input = File.ReadAllLines(fileName);
            programMemory = new Dictionary<long, long>();

            currentPointer = 0;
            relativeBase = 0;
            OutputValue = 0;
            IsHalted = false;
            State = ComputerState.None;

            inputQueue = new Queue<long>();
            outputQueue = new Queue<long>();

            long ptr = 0;
            foreach (string s in input[0].Split(','))
            {
                programMemory.Add(ptr, long.Parse(s));
                ptr++;
            }
        }

        public void SetMemory(long address, long value)
        {
            programMemory[address] = value;
        }

        public long GetValueAtAddress(long address)
        {
            return programMemory[address];
        }

        public void EnqueueInput(long input)
        {
            inputQueue.Enqueue(input);
        }

        public IEnumerable<long> GetAllOutput()
        {
            long l;
            while (outputQueue.TryDequeue(out l))
            {
                yield return l;
            }
        }

        private long GetParamValue(int paramMode, int val)
        {
            if (paramMode == 0)
            {
                if (!programMemory.ContainsKey(currentPointer + val))
                {
                    programMemory[currentPointer + val] = 0;
                }
                if (!programMemory.ContainsKey(programMemory[currentPointer + val]))
                {
                    programMemory[programMemory[currentPointer + val]] = 0;
                }
                return programMemory[programMemory[currentPointer + val]];
            }
            else if (paramMode == 1)
            {
                if (!programMemory.ContainsKey(currentPointer + val))
                {
                    programMemory[currentPointer + val] = 0;
                }
                return programMemory[currentPointer + val];
            }
            else if (paramMode == 2)
            {
                if (!programMemory.ContainsKey(currentPointer + val))
                {
                    programMemory[currentPointer + val] = 0;
                }
                if (!programMemory.ContainsKey(programMemory[currentPointer + val] + relativeBase))
                {
                    programMemory[programMemory[currentPointer + val] + relativeBase] = 0;
                }
                return programMemory[programMemory[currentPointer + val] + relativeBase];
            }

            throw new InvalidOperationException("Invalid parameter mode");
        }

        long opCodeInput;
        long opCode;
        string opCodeInputStr;
        int thirdParamMode;
        int secondParamMode;
        int firstParamMode;

        private void ParseOpcode()
        {
            opCodeInput = programMemory[currentPointer];
            opCode = opCodeInput % 100;

            opCodeInputStr = opCodeInput.ToString().PadLeft(5, '0');
            thirdParamMode = int.Parse(opCodeInputStr[0].ToString());
            secondParamMode = int.Parse(opCodeInputStr[1].ToString());
            firstParamMode = int.Parse(opCodeInputStr[2].ToString());
        }

        private void Add()
        {
            long first = GetParamValue(firstParamMode, 1);
            long second = GetParamValue(secondParamMode, 2);

            long offset = thirdParamMode == 2 ? relativeBase : 0;

            if (!programMemory.ContainsKey(currentPointer + 3))
            {
                programMemory[currentPointer + 3] = 0;
            }
            programMemory[programMemory[currentPointer + 3] + offset] = first + second;

            currentPointer += 4;

            if (debugMode)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("ADD ");
                if (firstParamMode == 0)
                {
                    // reference mode
                    sb.Append("&" + programMemory[currentPointer + 1]);
                }
            }
        }

        private void Multiply()
        {
            long first = GetParamValue(firstParamMode, 1);
            long second = GetParamValue(secondParamMode, 2);

            long offset = thirdParamMode == 2 ? relativeBase : 0;

            if (!programMemory.ContainsKey(currentPointer + 3))
            {
                programMemory[currentPointer + 3] = 0;
            }
            programMemory[programMemory[currentPointer + 3] + offset] = first * second;
            currentPointer += 4;
        }

        private void JumpIfTrue()
        {
            long first = GetParamValue(firstParamMode, 1);
            long second = GetParamValue(secondParamMode, 2);
            if (first != 0)
            {
                currentPointer = second;
            }
            else
            {
                currentPointer += 3;
            }
        }

        private void JumpIfFalse()
        {
            long first = GetParamValue(firstParamMode, 1);
            long second = GetParamValue(secondParamMode, 2);
            if (first == 0)
            {
                currentPointer = second;
            }
            else
            {
                currentPointer += 3;
            }
        }

        private void LessThan()
        {
            long first = GetParamValue(firstParamMode, 1);
            long second = GetParamValue(secondParamMode, 2);

            long offset = thirdParamMode == 2 ? relativeBase : 0;
            if (!programMemory.ContainsKey(currentPointer + 3))
            {
                programMemory[currentPointer + 3] = 0;
            }
            if (first < second)
            {
                programMemory[programMemory[currentPointer + 3] + offset] = 1;
            }
            else
            {
                programMemory[programMemory[currentPointer + 3] + offset] = 0;
            }

            currentPointer += 4;
        }

        private void EqualsOperation()
        {
            long first = GetParamValue(firstParamMode, 1);
            long second = GetParamValue(secondParamMode, 2);

            long offset = thirdParamMode == 2 ? relativeBase : 0;

            if (!programMemory.ContainsKey(currentPointer + 3))
            {
                programMemory[currentPointer + 3] = 0;
            }

            if (first == second)
            {
                programMemory[programMemory[currentPointer + 3] + offset] = 1;
            }
            else
            {
                programMemory[programMemory[currentPointer + 3] + offset] = 0;
            }

            currentPointer += 4;
        }

        private void AdjustRelativeBase()
        {
            long first = GetParamValue(firstParamMode, 1);

            relativeBase += first;
            currentPointer += 2;
        }

        public long RunUntilNextOutput()
        {
            State = ComputerState.Running;

            while (true)
            {
                ParseOpcode();

                if (opCode == 1)
                {
                    Add();
                }
                else if (opCode == 2)
                {
                    Multiply();
                }
                else if (opCode == 3)
                {
                    // take input
                    long offset = firstParamMode == 2 ? relativeBase : 0;

                    if (!programMemory.ContainsKey(currentPointer + 1))
                    {
                        programMemory[currentPointer + 1] = 0;
                    }
                    if (!programMemory.ContainsKey(programMemory[currentPointer + 1] + offset))
                    {
                        programMemory[programMemory[currentPointer + 1] + offset] = 0;
                    }

                    if (inputQueue.Count == 0)
                    {
                        throw new InvalidOperationException("expected input");
                    }
                    else
                    {
                        programMemory[programMemory[currentPointer + 1] + offset] = inputQueue.Dequeue();
                    }

                    currentPointer += 2;
                }
                else if (opCode == 4)
                {
                    // output
                    long first = GetParamValue(firstParamMode, 1);

                    //Console.WriteLine("OUTPUT {0}", first);
                    OutputValue = first;
                    currentPointer += 2;
                    return OutputValue;
                }
                else if (opCode == 5)
                {
                    JumpIfTrue();
                }
                else if (opCode == 6)
                {
                    JumpIfFalse();
                }
                else if (opCode == 7)
                {
                    LessThan();
                }
                else if (opCode == 8)
                {
                    EqualsOperation();
                }
                else if (opCode == 9)
                {
                    AdjustRelativeBase();
                }
                else if (opCode == 99)
                {
                    // halt
                    IsHalted = true;
                    State = ComputerState.Halted;
                    break;
                }
            }

            return OutputValue;
        }

        public void Run()
        {
            State = ComputerState.Running;

            while (true)
            {
                ParseOpcode();

                if (opCode == 1)
                {
                    Add();
                }
                else if (opCode == 2)
                {
                    Multiply();
                }
                else if (opCode == 3)
                {
                    // take input
                    long offset = firstParamMode == 2 ? relativeBase : 0;

                    if (!programMemory.ContainsKey(currentPointer + 1))
                    {
                        programMemory[currentPointer + 1] = 0;
                    }
                    if (!programMemory.ContainsKey(programMemory[currentPointer + 1] + offset))
                    {
                        programMemory[programMemory[currentPointer + 1] + offset] = 0;
                    }

                    if (inputQueue.Count == 0)
                    {
                        State = ComputerState.WaitingForInput;
                        return;
                    }
                    else
                    {
                        programMemory[programMemory[currentPointer + 1] + offset] = inputQueue.Dequeue();
                    }

                    currentPointer += 2;
                }
                else if (opCode == 4)
                {
                    // output
                    long first = GetParamValue(firstParamMode, 1);

                    //Console.WriteLine("OUTPUT {0}", first);
                    outputQueue.Enqueue(first);
                    currentPointer += 2;
                }
                else if (opCode == 5)
                {
                    JumpIfTrue();
                }
                else if (opCode == 6)
                {
                    JumpIfFalse();
                }
                else if (opCode == 7)
                {
                    LessThan();
                }
                else if (opCode == 8)
                {
                    EqualsOperation();
                }
                else if (opCode == 9)
                {
                    AdjustRelativeBase();
                }
                else if (opCode == 99)
                {
                    // halt
                    IsHalted = true;
                    State = ComputerState.Halted;
                    break;
                }
            }

            return;
        }

    }
}
