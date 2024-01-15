using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcurrencyBasics
{
    internal class BarberShop
    {
        object lockObject = new object();
        int n;
        bool hairCutActive = false;
        bool barberSleeping = true;
        Queue<int> queue = new Queue<int>();

        public BarberShop(int n)
        {
            this.n = n;
        }

        public void GetHairCut()
        {
            while (true)
            {
                lock (lockObject)
                {
                    while (queue.Count == 0 || hairCutActive)
                    {
                        if (queue.Count == 0)
                        {
                            barberSleeping = true;
                            Console.WriteLine("Barber sleeping");
                        }
                        Monitor.Wait(lockObject);
                    }
                    int curr = queue.Dequeue();
                    hairCutActive = true;
                    Console.WriteLine($"Customer: {curr} getting hair cut");
                    Thread.Sleep(100);
                    hairCutActive = false;
                    Monitor.PulseAll(lockObject);
                }
            }
        }

        public void CustomerEnter(int identity)
        {
            lock (lockObject)
            {
                int cust = queue.Count();
                while (cust == n)
                {
                    Console.WriteLine("Queue full");
                    return;
                }
                queue.Enqueue(identity);
                Console.WriteLine($"Customer: {identity} entered");
                if (barberSleeping)
                {
                    barberSleeping = false;
                    Console.WriteLine("Barber woken up");
                }
                Monitor.PulseAll(lockObject);
            }
        }
    }

    internal class BarberShopFunction
    {
        public static void BarberShopMainFunc()
        {
            BarberShop barberShop = new BarberShop(5);
            Thread thread1 = new Thread(() =>
            {
                for (int i = 0; i < 4; i++)
                {
                    int curr = i;
                    barberShop.CustomerEnter(curr);
                }
            });
            Thread thread2 = new Thread(() =>
            {
                for (int i = 0; i < 5; i++)
                {
                    int curr = i;
                    barberShop.GetHairCut();
                }
            });
            
            Thread thread3 = new Thread(() =>
            {
                for (int i = 5; i < 10; i++)
                {
                    Thread.Sleep(1000);
                    int curr = i;
                    barberShop.CustomerEnter(curr);
                }
            });
            thread1.Start();
            thread2.Start();
            thread3.Start();
            thread1.Join();
            thread2.Join();
            thread3.Join();
        }
    }
}
