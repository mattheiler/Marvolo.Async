using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Marvolo.Async
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Async.RunSynchronously(DoThings);
        }

        private static readonly AsyncLock Stuff = new AsyncLock();

        public static async Task DoThings()
        {
            // better
            try
            {
                var tasks = new[]
                {
                    DoThing1(),
                    DoThing2(),
                    DoThing3()
                };

                // state.Steps.Completed = 0;
                // state.Steps.Count = tasks.Length;

                Async.WaitAllWithProgress(Console.WriteLine /* steps => state.StepsCompleted = steps */, 0 /* state.StepsCompleted */, steps => steps + 1, tasks);
            }
            catch (AggregateException e)
            {
                Console.WriteLine(string.Join(Environment.NewLine, e.Flatten().InnerExceptions.Select(exception => exception.Message)));
            }

            Console.WriteLine(); // line break

            // better yet
            try
            {
                Async.WaitAllWithProgress(Console.WriteLine /* progress => state.Progress = progress */, new[]
                {
                    DoThing1(),
                    DoThing2(),
                    DoThing3()
                });
            }
            catch (AggregateException e)
            {
                Console.WriteLine(string.Join(Environment.NewLine, e.Flatten().InnerExceptions.Select(exception => exception.Message)));
            }

            Console.WriteLine(); // line break

            // best
            try
            {
                await Async.WhenAllWithProgress(Console.WriteLine /* progress => state.Progress = progress */, new[]
                {
                    DoThing1(),
                    DoThing2(),
                    DoThing3()
                });
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine(string.Join(Environment.NewLine, e.Message));
            }

            Console.WriteLine(); // line break

            // lock
            using (await Stuff.LockAsync())
            {
                // do stuff
            }

            Console.WriteLine(); // line break

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }

        public static Task<IEnumerable<string>> GetLetters()
        {
            return Task.FromResult(Enumerable.Range('A', 26).Select(@char => ((char) @char).ToString()));
        }

        public static async Task DoThing1()
        {
            await Task.Yield();
        }

        public static async Task DoThing2()
        {
            await Task.Delay(100);
            throw new InvalidOperationException();
        }

        public static async Task DoThing3()
        {
            await Task.Delay(100);
            throw new InvalidOperationException();
        }
    }
}