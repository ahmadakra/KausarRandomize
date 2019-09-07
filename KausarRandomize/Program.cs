using System;
using System.Collections.Generic;
using System.Linq;

namespace KausarRandomize
{
    class Program
    {
        static void Main(string[] _)
        {
            List<Sequence> trial;
            var rand = new Random();
            int counter = 0;

            // Keep generating random trials until one of them satisfies all conditions in TrialLooksGood()
            do
            {
                counter++;
                trial = new List<Sequence>();

                // First item
                var seq = new Sequence { Emotion = Emotion.Positive, Obj = 2 };
                trial.Add(seq);

                for (int i = 1; i < 20; i++)
                {
                    Emotion nextEmotion;
                    int nextObj;
                    
                    if (rand.NextDouble() >= 0.5)
                    {
                        // Option 1
                        nextEmotion = seq.Emotion;
                        nextObj = seq.Obj == 1 ? 2 : 1;
                    }
                    else
                    {
                        // Option 2
                        nextEmotion = seq.Emotion == Emotion.Positive ? Emotion.Negative : Emotion.Positive;
                        nextObj = seq.Obj;
                    }

                    // Next object
                    seq = new Sequence { Emotion = nextEmotion, Obj = nextObj };
                    trial.Add(seq);
                }

            } while (!TrialLooksGood(trial));

            Console.WriteLine($"Number of loops: {counter}");
            foreach (var item in trial)
            {
                Console.WriteLine(item.ToString());
            }
        }

        private static bool TrialLooksGood(List<Sequence> list)
        {
            if (list.Count != 20)
            {
              //  Console.WriteLine("Wrong Count");
                return false;
            }

            var n1 = list.Count(e => e.Emotion == Emotion.Negative && e.Obj == 1);
            if (n1 != 5)
            {
                // Console.WriteLine($"Wrong count of Negative 1st: {n1}");
                return false;
            }

            var n2 = list.Count(e => e.Emotion == Emotion.Negative && e.Obj == 2);
            if (n2 != 5)
            {
                // Console.WriteLine($"Wrong count of Negative 2nd: {n2}");
                return false;
            }

            var p1 = list.Count(e => e.Emotion == Emotion.Positive && e.Obj == 1);
            if (p1 != 5)
            {
               // Console.WriteLine($"Wrong count of Positive 1st: {p1}");
                return false;
            }

            var p2 = list.Count(e => e.Emotion == Emotion.Positive && e.Obj == 2);
            if (p2 != 5)
            {
             //   Console.WriteLine($"Wrong count of Positive 2nd: {p2}");
                return false;
            }


            var firstHalf = list.Take(10).Count(e => e.Emotion == Emotion.Positive);
            if (firstHalf != 5)
            {
                // Console.WriteLine($"A block contains more positive than negative");
                return false;
            }

            int counter = 0;
            Emotion prev = list.First().Emotion;
            foreach(var item in list.Skip(1))
            {
                if(item.Emotion == prev)
                {
                    counter++;
                    if(counter >= 3)
                    {
                        // Console.WriteLine($"More than 3 consecutive ");
                        return false;
                    }
                }
                else
                {
                    counter = 0;
                    prev = item.Emotion;
                }
            }

            return true;
        }
    }

    struct Sequence
    {
        public Emotion Emotion { get; set; }

        public int Obj { get; set; }

        public override string ToString()
        {
            string objString = Obj == 1 ? "1st" : "2nd";
            return $"{Emotion}\t{objString}";
        }
    }

    enum Emotion
    {
        Positive,
        Negative
    }
}
