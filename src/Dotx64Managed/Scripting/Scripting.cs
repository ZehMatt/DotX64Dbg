using System;

namespace Dotx64Dbg
{
    /// <summary>
    /// Wrapper to simplify the scripting environment.
    /// </summary>
    public static partial class Scripting
    {
        public static void Print(string line)
        {
            Console.WriteLine(line);
        }
        public static void Print(string fmt, params object[] args)
        {
            Console.WriteLine(fmt, args);
        }

        /// <summary>
        /// Step into.
        /// </summary>
        public static void Sti()
        {
            Debugger.StepIn();
        }

        /// <summary>
        /// Step into a specified amount of times.
        /// </summary>
        /// <param name="steps">Amount of steps</param>
        public static void Sti(int steps)
        {
            for (int i = 0; i < steps; i++)
                Sti();
        }

        /// <summary>
        /// Step over.
        /// </summary>
        public static void Sto()
        {
            Debugger.StepOver();
        }

        /// <summary>
        /// Step over a specified amount of times.
        /// </summary>
        /// <param name="steps">Amount of steps</param>
        public static void Sto(int steps)
        {
            for (int i = 0; i < steps; i++)
                Sto();
        }

        /// <summary>
        /// Continue execution
        /// </summary>
        public static void Run()
        {
            Debugger.Run();
        }

        /// <summary>
        /// Pause execution.
        /// </summary>
        public static void Pause()
        {
            Debugger.Pause();
        }

        /// <summary>
        /// Stops the debugger
        /// </summary>
        public static void Stop()
        {
            Debugger.Stop();
        }

        /// <summary>
        /// Skip over a specified amount of instructions.
        /// </summary>
        /// <param name="numInstructions">Amount of instructions to skip</param>
        public static void Skip(int numInstructions = 1)
        {
            Debugger.RunCommand($"skip {numInstructions}");
        }
    }
}
