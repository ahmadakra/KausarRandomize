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
                var seq = new Sequence { Emotion = Emotion.Positive, Object = 2 };
                trial.Add(seq);

                for (int i = 1; i < 20; i++)
                {
                    Emotion nextEmotion;
                    int nextObject;
                    
                    if (rand.NextDouble() >= 0.5)
                    {
                        // Option 1
                        nextEmotion = seq.Emotion;
                        nextObject = seq.Object == 1 ? 2 : 1;
                    }
                    else
                    {
                        // Option 2
                        nextEmotion = seq.Emotion == Emotion.Positive ? Emotion.Negative : Emotion.Positive;
                        nextObject = seq.Object;
                    }

                    // Next object
                    seq = new Sequence { Emotion = nextEmotion, Object = nextObject };
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

            var n1 = list.Count(e => e.Emotion == Emotion.Negative && e.Object == 1);
            if (n1 != 5)
            {
                // Console.WriteLine($"Wrong count of Negative 1st: {n1}");
                return false;
            }

            var n2 = list.Count(e => e.Emotion == Emotion.Negative && e.Object == 2);
            if (n2 != 5)
            {
                // Console.WriteLine($"Wrong count of Negative 2nd: {n2}");
                return false;
            }

            var p1 = list.Count(e => e.Emotion == Emotion.Positive && e.Object == 1);
            if (p1 != 5)
            {
               // Console.WriteLine($"Wrong count of Positive 1st: {p1}");
                return false;
            }

            var p2 = list.Count(e => e.Emotion == Emotion.Positive && e.Object == 2);
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
                        // Console.WriteLine($"More than 3 consecutive emotions are the same");
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
        // Positive or Negative
        public Emotion Emotion { get; set; }

        // 1st or 2nd
        public int Object { get; set; }

        public override string ToString()
        {
            string objString = Object == 1 ? "1st" : "2nd";
            return $"{Emotion}\t{objString}";
        }
    }

    enum Emotion
    {
        Positive,
        Negative
    }
}
