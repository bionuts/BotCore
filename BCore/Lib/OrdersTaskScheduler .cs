﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BCore.Lib
{
    class OrdersTaskScheduler : TaskScheduler, IDisposable
    {
        private BlockingCollection<Task> tasksCollection = new BlockingCollection<Task>();
        private readonly Thread mainThread = null;

        public OrdersTaskScheduler()
        {
            mainThread = new Thread(new ThreadStart(Execute));
            if (!mainThread.IsAlive)
            {
                mainThread.Start();
            }
        }

        private void Execute()
        {
            foreach (var task in tasksCollection.GetConsumingEnumerable())
            {
                TryExecuteTask(task);
            }
        }

        private void Dispose(bool disposing)
        {
            if (!disposing) return;
            tasksCollection.CompleteAdding();
            tasksCollection.Dispose();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected override IEnumerable<Task> GetScheduledTasks()
        {
            return tasksCollection.ToArray();
        }

        protected override void QueueTask(Task task)
        {
            if (task != null)
                tasksCollection.Add(task);
        }

        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            return false;
        }
    }
}
