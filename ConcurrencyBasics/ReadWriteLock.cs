using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcurrencyBasics
{
    class ReadreadLock
    {
        private object critical;

        private object readLock = new object();

        private int read = 0;
        private bool write = false;
        public ReadreadLock(object critical)
        {
            this.critical = critical;
        }
        public void AcquireReadLock()
        {
            lock (readLock)
            {
                while (write)
                {
                    Console.WriteLine("Waiting for read lock");
                    Monitor.Wait(readLock);
                }
                read++;
                Console.WriteLine($"Acquiring read lock: {read}");
                //Monitor.Enter(critical);
            }
        }

        public void AcquireWriteLock()
        {
            lock (readLock)
            {
                while (write || read > 0)
                {
                    Console.WriteLine("Waiting for write lock");
                    Monitor.Wait(readLock);
                }
                write = true;
                Console.WriteLine($"Acquiring write lock");
                //Monitor.Enter(critical);
            }
        }

        public void ReleaseReadLock()
        {
            lock (readLock)
            {
                read--;
                if (read == 0)
                {
                    Console.WriteLine("Releasing read lock");
                    Monitor.PulseAll(readLock);
                }
                Console.WriteLine($"Exiting read lock:{read}");
                //Monitor.Exit(critical);
            }
        }

        public void ReleaseWriteLock()
        {
            lock (readLock)
            {
                write = false;
                Console.WriteLine("Releasing write lock");
                Monitor.PulseAll(readLock);
                Console.WriteLine("Exiting write lock");
                //Monitor.Exit(critical);
            }
        }
    }

    class ReadWriteLockFunction
    {
        public static void ReadWriteLock()
        {
            HashSet<int> set = new HashSet<int>();
            ReadreadLock rwLock = new ReadreadLock(set);
            Thread read1 = new Thread(() =>
            {
                for (int i = 0; i < 2; i++)
                {
                    rwLock.AcquireReadLock();
                    Thread.Sleep(100);
                    rwLock.ReleaseReadLock();
                }
            });
            Thread read2 = new Thread(() =>
            {
                for (int i = 0; i < 2; i++)
                {
                    rwLock.AcquireReadLock();
                    Thread.Sleep(100);
                    rwLock.ReleaseReadLock();
                }
            });
            Thread write = new Thread(() =>
            {
                for (int i = 0; i < 2; i++)
                {
                    rwLock.AcquireWriteLock();
                    Thread.Sleep(100);
                    rwLock.ReleaseWriteLock();
                }
            });
            read1.Start();
            read2.Start();
            write.Start();

            read1.Join();
            read2.Join();
            write.Join();
        }
    }
}
