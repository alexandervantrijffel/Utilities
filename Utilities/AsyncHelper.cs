using System;
using System.Threading;
using System.Threading.Tasks;

namespace Structura.Shared.Utilities
{
    /// <summary>
    /// Helper class for running async methods synchronously
    /// </summary>
    public static class AsyncHelper
    {
        private static readonly TaskFactory _myTaskFactory = new
            TaskFactory(CancellationToken.None,
                TaskCreationOptions.None,
                TaskContinuationOptions.None,
                TaskScheduler.Default);

        public static TResult RunSync<TResult>(Func<Task<TResult>> func)
        {
            return AsyncHelper._myTaskFactory
                .StartNew<Task<TResult>>(func)
                .Unwrap<TResult>()
                .GetAwaiter()
                .GetResult();
        }

        public static void RunSync(Func<Task> func)
        {
            AsyncHelper._myTaskFactory
                .StartNew<Task>(func)
                .Unwrap()
                .GetAwaiter()
                .GetResult();
        }

        public static async Task<bool> RunWithTimeout(Task task, TimeSpan timeout)
        {
            var result = await Task.WhenAny(task, Task.Delay(timeout));
            var taskCompleted = result == task;
            if (taskCompleted) await task;
            return taskCompleted;
        }

        public static async Task<(bool)> RunWithTimeout<TTaskReturnVal>(Task<TTaskReturnVal> task, TimeSpan timeout)
        {
            var result = await Task.WhenAny(task, Task.Delay(timeout));
            var taskCompleted = result == task;
            if (taskCompleted) await task;
            return (taskCompleted);
        }
    }
}
