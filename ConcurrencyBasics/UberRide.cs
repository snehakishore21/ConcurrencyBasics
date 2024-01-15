using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcurrencyBasics
{
    internal class UberRide
    {
        private object lockObject = new object();

        private int democrat = 0;

        private int republican = 0;
        public UberRide()
        {
        }

        public void Seated(string pas)
        {
            Console.WriteLine($"{pas} seated");
        }

        public void DemocratSeated()
        {
            lock(lockObject)
            {
                while((republican+democrat)>=4 || republican>=3 || (democrat==2 && republican==1))
                {
                    Monitor.Wait(lockObject);
                    Console.WriteLine("Waiting for Democrat to be seated");
                }
                democrat++;
                Seated("Democrat");
                if(democrat+republican==4)
                {
                    Drive();
                }
                Monitor.PulseAll(lockObject);
            }
        }

        private void Drive()
        {
            lock(lockObject)
            {
                Console.WriteLine("Driving");
                Thread.Sleep(1000);
                democrat = 0;
                republican = 0;
                Monitor.PulseAll(lockObject);
            }
        }

        public void RepublicanSeated()
        {
            lock (lockObject)
            {
                while ((republican + democrat) >= 4 || democrat >= 3 || (republican == 2 && democrat == 1))
                {
                    Monitor.Wait(lockObject);
                    Console.WriteLine("Waiting for Republican to be seated");
                }
                republican++;
                Seated("Republican");
                if (democrat + republican == 4)
                {
                    Drive();
                }
                Monitor.PulseAll(lockObject);
            }
        }
    }

    public class UberRideFunction
    {
        public static void UberRideMainFunc()
        {
            UberRide uberRide = new UberRide();
            Thread thread = new Thread(() =>
            {
                for (int i = 0; i < 1; i++)
                {
                    uberRide.RepublicanSeated();
                }
            });
            Thread thread2 = new Thread(() =>
            {
                for (int i = 0; i < 2; i++)
                {
                    uberRide.DemocratSeated();
                }
            });
            Thread thread3 = new Thread(() =>
            {
                for (int i = 0; i < 1; i++)
                {
                    uberRide.RepublicanSeated();
                }
            });
            Thread thread4 = new Thread(() =>
            {
                for (int i = 0; i < 1; i++)
                {
                    uberRide.DemocratSeated();
                }
            });
            thread.Start();
            thread2.Start();
            thread3.Start();
            thread4.Start();
            thread.Join();
            thread2.Join();
            thread3.Join();
            thread4.Join();
        }
    }
}
