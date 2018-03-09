using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Marvolo.Async
{
    [DebuggerStepThrough]
    public static class Async
    {
        public static async Task Do(this Task source, Action function)
        {
            await source;
            function();
        }

        public static async Task<TResult> Do<TResult>(this Task<TResult> source, Action<TResult> function)
        {
            var result = await source;
            function(result);
            return result;
        }

        public static AsyncAwaitable Run(Func<Task> func)
        {
            return new AsyncAwaitable(Task.Run(func));
        }

        public static AsyncAwaitable<TResult> Run<TResult>(Func<Task<TResult>> func)
        {
            return new AsyncAwaitable<TResult>(Task.Run(func));
        }

        public static void RunSynchronously(Func<Task> func)
        {
            Run(func).GetAwaiter().GetResult();
        }

        public static TResult RunSynchronously<TResult>(Func<Task<TResult>> func)
        {
            return Run(func).GetAwaiter().GetResult();
        }

        public static Task<TResult> Select<TSource, TResult>(this Task<TSource> source, Func<TSource, TResult> resultSelector)
        {
            return source.Then(resultSelector);
        }

        public static Task<TResult> SelectMany<TSource, TIntermediate, TResult>(this Task<TSource> source, Func<TSource, Task<TIntermediate>> intermediateSelector, Func<TSource, TIntermediate, TResult> resultSelector)
        {
            return source.Then(async result => resultSelector(result, await intermediateSelector(result)));
        }

        public static async Task<TResult> Then<TResult>(this Task source, Func<TResult> resultSelector)
        {
            await source;
            return resultSelector();
        }

        public static async Task<TResult> Then<TSource, TResult>(this Task<TSource> source, Func<TSource, TResult> resultSelector)
        {
            return resultSelector(await source);
        }

        public static async Task<TResult> Then<TSource, TResult>(this Task<TSource> source, Func<TSource, Task<TResult>> resultSelector)
        {
            return await resultSelector(await source);
        }

        public static void WaitAllWithProgress(Action<double> progress, ICollection<Task> tasks)
        {
            WaitAllWithProgress(new Progress<double>(count => progress(count / tasks.Count)), 0, count => ++count, tasks);
        }

        public static void WaitAllWithProgress<TProgress>(Action<TProgress> progress, TProgress seed, Func<TProgress, TProgress> progressed, params Task[] tasks)
        {
             WaitAllWithProgress(progress, seed, progressed, tasks.AsEnumerable());
        }

        public static void WaitAllWithProgress<TProgress>(Action<TProgress> progress, TProgress seed, Func<TProgress, TProgress> progressed, IEnumerable<Task> tasks)
        {
             WaitAllWithProgress(new Progress<TProgress>(progress), seed, progressed, tasks);
        }

        public static void WaitAllWithProgress<TProgress>(IProgress<TProgress> progress, TProgress seed, Func<TProgress, TProgress> progressed, params Task[] tasks)
        {
             WaitAllWithProgress(progress, seed, progressed, tasks.AsEnumerable());
        }

        public static void WaitAllWithProgress<TProgress>(IProgress<TProgress> progress, TProgress seed, Func<TProgress, TProgress> progressed, IEnumerable<Task> tasks)
        {
            Task.WaitAll(tasks.Select(task => task.Do(() => progress.Report(seed = progressed(seed)))).ToArray());
        }

        public static Task WhenAllWithProgress(Action<double> progress, ICollection<Task> tasks)
        {
            return WhenAllWithProgress(new Progress<double>(count => progress(count / tasks.Count)), 0, count => count + 1, tasks);
        }

        public static Task<TResult[]> WhenAllWithProgress<TResult>(Action<double> progress, ICollection<Task<TResult>> tasks)
        {
            return WhenAllWithProgress(new Progress<double>(count => progress(count / tasks.Count)), 0, (count, result) => count + 1, tasks);
        }

        public static Task WhenAllWithProgress<TProgress>(Action<TProgress> progress, TProgress seed, Func<TProgress, TProgress> progressed, params Task[] tasks)
        {
            return WhenAllWithProgress(progress, seed, progressed, tasks.AsEnumerable());
        }

        public static Task WhenAllWithProgress<TProgress>(Action<TProgress> progress, TProgress seed, Func<TProgress, TProgress> progressed, IEnumerable<Task> tasks)
        {
            return WhenAllWithProgress(new Progress<TProgress>(progress), seed, progressed, tasks);
        }

        public static Task WhenAllWithProgress<TProgress>(IProgress<TProgress> progress, TProgress seed, Func<TProgress, TProgress> progressed, params Task[] tasks)
        {
            return WhenAllWithProgress(progress, seed, progressed, tasks.AsEnumerable());
        }

        public static Task WhenAllWithProgress<TProgress>(IProgress<TProgress> progress, TProgress seed, Func<TProgress, TProgress> progressed, IEnumerable<Task> tasks)
        {
            return Task.WhenAll(tasks.Select(task => task.Do(() => progress.Report(seed = progressed(seed)))));
        }

        public static Task<TResult[]> WhenAllWithProgress<TProgress, TResult>(Action<TProgress> progress, TProgress seed, Func<TProgress, TResult, TProgress> progressed, params Task<TResult>[] tasks)
        {
            return WhenAllWithProgress(progress, seed, progressed, tasks.AsEnumerable());
        }

        public static Task<TResult[]> WhenAllWithProgress<TProgress, TResult>(Action<TProgress> progress, TProgress seed, Func<TProgress, TResult, TProgress> progressed, IEnumerable<Task<TResult>> tasks)
        {
            return WhenAllWithProgress(new Progress<TProgress>(progress), seed, progressed, tasks);
        }

        public static Task<TResult[]> WhenAllWithProgress<TProgress, TResult>(IProgress<TProgress> progress, TProgress seed, Func<TProgress, TResult, TProgress> progressed, params Task<TResult>[] tasks)
        {
            return WhenAllWithProgress(progress, seed, progressed, tasks.AsEnumerable());
        }

        public static Task<TResult[]> WhenAllWithProgress<TProgress, TResult>(IProgress<TProgress> progress, TProgress seed, Func<TProgress, TResult, TProgress> progressed, IEnumerable<Task<TResult>> tasks)
        {
            return Task.WhenAll(tasks.Select(task => task.Do(result => progress.Report(seed = progressed(seed, result)))));
        }
    }
}