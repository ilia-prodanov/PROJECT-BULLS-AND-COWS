using System.Diagnostics;

namespace DemoForTests
{
    internal class SpeedTests
    {
        static void Main(string[] args)
        {
            List<int[]> list = new List<int[]>();
            Queue<int[]> queue = new Queue<int[]>();
            int[] arrayExample = new int[] { 1, 2 };
            int iterations = 10_0000;

            Stopwatch sw = new Stopwatch();
           
            sw = CountMilliseconds(typeof(List<>), iterations);
            Console.WriteLine($"List total time in ms: {sw.ElapsedMilliseconds}");

            sw = CountMilliseconds(typeof(Queue<>), iterations);
            Console.WriteLine($"Queue total time in ms: {sw.ElapsedMilliseconds}");

            sw = CountMilliseconds(typeof(HashSet<>), iterations);
            Console.WriteLine($"HashSet total time in ms: {sw.ElapsedMilliseconds}");
        }

        static Stopwatch CountMilliseconds(Type collectionType, int iterations)
        {
            Stopwatch sw = new Stopwatch();
            int[] arrayExample = new int[] {1,2};
            if( collectionType.Name == typeof(List<>).Name)
            {
                sw.Start();
                List<int[]> list = new List<int[]>();
                for(int i = 0; i < iterations; i++)
                {
                    list.Add(arrayExample);
                }
                for (int i = 0; i < iterations; i++)
                {
                    list.RemoveAt(0);
                }
                sw.Stop();
            }
            else if (collectionType.Name == typeof(Queue<>).Name)
            {
                Queue<int[]> queue = new Queue<int[]>();
                sw.Start();
                for (int i = 0; i < iterations; i++)
                {
                    queue.Enqueue(arrayExample);
                }
                for (int i = 0; i < iterations; i++)
                {
                    queue.Dequeue();
                }
                sw.Stop();
            }
            else if(collectionType.Name == typeof(HashSet<>).Name)
            {
                HashSet<int[]> hashSet = new HashSet<int[]>();
                sw.Start();
                for (int i = 0; i < iterations; i++)
                {
                    hashSet.Add(arrayExample);
                }
                for (int i = 0; i < iterations; i++)
                {
                    hashSet.Remove(arrayExample);
                }
                sw.Stop();
            }

            return sw;
        }      
    }
}
