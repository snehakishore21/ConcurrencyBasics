using System.Reflection.Metadata.Ecma335;

namespace ConcurrencyBasics
{
    class ConcurrencyBasics
    {
        static void Main(string[] args)
        {
            //ReadWriteLockFunction.ReadWriteLock();
            //UberRideFunction.UberRideMainFunc();
            //AsyncExecutorFunction.AsyncExecutorMainFunc();
            //BarberShopFunction.BarberShopMainFunc();

            //Task t = Task.Run(() => NewMethod());
            //Task.WaitAll(t);
        }

        private static async Task NewMethod()
        {
            Console.WriteLine("Start");

            // Asynchronously execute the LongRunningOperation method
            await LongRunningOperation();
            await LongRunningOperation2();
            Console.WriteLine("End");
            Thread.Sleep(1000);
            Console.WriteLine("End2");
            Thread.Sleep(1000);
            Console.WriteLine("End3");
        }

        static async Task LongRunningOperation()
        {
            Console.WriteLine("Operation Start");

            // Simulate a long-running asynchronous operation
            await Task.Delay(2000);

            Console.WriteLine("Operation End");
        }

        static async Task LongRunningOperation2()
        {
            Console.WriteLine("Operation Start2");

            // Simulate a long-running asynchronous operation
            await Task.Delay(2000);

            Console.WriteLine("Operation End2");
        }
    }
}