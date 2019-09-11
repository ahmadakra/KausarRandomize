using System;
using System.Collections.Generic;
using System.Linq;

namespace KausarRandomize.V2
{
    class Program
    {
        static void Main(string[] _)
        {
            List<Trial> set;
            var rand = new Random();
            int counter = 0;
            var ns = new int[] { 1, 2, 3, 4, 5 };
            var shuffled = ns.OrderBy(e => rand.NextDouble()).ToArray();
            var shuffled2 = ns.OrderBy(e => rand.NextDouble()).ToArray();

            // Keep generating random trials until one of them satisfies all conditions in TrialLooksGood()
            do
            {
                counter++;
                set = new List<Trial>();

                // First item
                var trial = new Trial { Emotion = Emotion.Positive, FS = FS.First };
                set.Add(trial);

                bool repeatedEmotion = false;
                bool repeatedFs = false;

                for (int i = 1; i < 20; i++)
                {
                    Emotion nextEmotion;
                    FS nextFS;

                    //if (repeatedFs && !repeatedEmotion)
                    //{
                    //    // Option 1
                    //    nextEmotion = trial.Emotion;
                    //    nextFS = trial.FS == FS.First ? FS.Second : FS.First;
                    //}
                    //else if (repeatedEmotion && !repeatedFs) // Rule: emotion may not repeat more than twice
                    //{
                    //    // Option 2
                    //    nextEmotion = trial.Emotion == Emotion.Positive ? Emotion.Negative : Emotion.Positive;
                    //    nextFS = trial.FS;
                    //}
                    //else if (repeatedEmotion && repeatedFs) // Rule: emotion may not repeat more than twice
                    //{
                    //    // Option 3: 
                    //    nextEmotion = trial.Emotion == Emotion.Positive ? Emotion.Negative : Emotion.Positive;
                    //    nextFS = trial.FS == FS.First ? FS.Second : FS.First;
                    //}
                    //else 
                    if (rand.NextDouble() >= 0.67) // Rule: FS may not repeat more than twice
                    {
                        // Option 1
                        nextEmotion = trial.Emotion;
                        nextFS = trial.FS == FS.First ? FS.Second : FS.First;
                    }
                    else if (rand.NextDouble() >= 0.33) // Rule: FS may not repeat more than twice
                    {
                        // Option 2
                        nextEmotion = trial.Emotion == Emotion.Positive ? Emotion.Negative : Emotion.Positive;
                        nextFS = trial.FS;
                    }
                    else
                    {
                        // Option 3: 
                        nextEmotion = trial.Emotion == Emotion.Positive ? Emotion.Negative : Emotion.Positive;
                        nextFS = trial.FS == FS.First ? FS.Second : FS.First;
                    }

                    // Check what is repeated
                    repeatedEmotion = nextEmotion == trial.Emotion;
                    repeatedFs = nextFS == trial.FS;

                    // Next item
                    trial = new Trial { Emotion = nextEmotion, FS = nextFS };
                    set.Add(trial);
                }


                //// Mechanical Engineer set
                //set.Add(new Trial { Emotion = Emotion.Positive, FS = FS.First });
                //set.Add(new Trial { Emotion = Emotion.Negative, FS = FS.Second });
                //set.Add(new Trial { Emotion = Emotion.Positive, FS = FS.Second });
                //set.Add(new Trial { Emotion = Emotion.Negative, FS = FS.First });
                //set.Add(new Trial { Emotion = Emotion.Positive, FS = FS.First });
                //set.Add(new Trial { Emotion = Emotion.Negative, FS = FS.Second });
                //set.Add(new Trial { Emotion = Emotion.Positive, FS = FS.Second });
                //set.Add(new Trial { Emotion = Emotion.Positive, FS = FS.First });
                //set.Add(new Trial { Emotion = Emotion.Negative, FS = FS.First });
                //set.Add(new Trial { Emotion = Emotion.Negative, FS = FS.Second });
                //set.Add(new Trial { Emotion = Emotion.Positive, FS = FS.First });
                //set.Add(new Trial { Emotion = Emotion.Positive, FS = FS.Second });
                //set.Add(new Trial { Emotion = Emotion.Negative, FS = FS.Second });
                //set.Add(new Trial { Emotion = Emotion.Negative, FS = FS.First });
                //set.Add(new Trial { Emotion = Emotion.Positive, FS = FS.Second });
                //set.Add(new Trial { Emotion = Emotion.Negative, FS = FS.Second });
                //set.Add(new Trial { Emotion = Emotion.Positive, FS = FS.First });
                //set.Add(new Trial { Emotion = Emotion.Negative, FS = FS.First });
                //set.Add(new Trial { Emotion = Emotion.Negative, FS = FS.Second });
                //set.Add(new Trial { Emotion = Emotion.Positive, FS = FS.First });



            } while (!SequenceLooksGood(set));

            Console.WriteLine("Sequence looks good");

            do
            {
                // Here we assign codes and phrases
                {
                    for (int i = 0; i < set.Count; i++)
                    {
                        if (i % 5 == 0)
                        {
                            shuffled = shuffled.OrderBy(e => rand.NextDouble()).ToArray();
                        }

                        set[i].Code = shuffled[i % 5];
                    }
                }

            } while (!CodesLooksGood(set));

            Console.WriteLine("Codes look good");

            do
            {
                // Here we assign codes and phrases
                {
                    for (int i = 0; i < set.Count; i++)
                    {
                        if (i % 5 == 0)
                        {
                            shuffled = shuffled.OrderBy(e => rand.NextDouble()).ToArray();
                        }

                        set[i].Phrase = shuffled[i % 5];
                    }
                }

            } while (!PhrasesLooksGood(set));

            Console.WriteLine($"Number of loops: {counter}");
            Console.WriteLine();
            Console.WriteLine($"#\tCode\t\t\tSequence\t\tPhrase");
            Console.WriteLine($"----------------------------------------------------------------");

            foreach (var item in set)
            {
                Console.WriteLine(item.ToString());
            }
        }

        private static int[] Rotate(int[] arr)
        {
            int[] result = new int[arr.Length];
            result[arr.Length - 1] = arr[0];
            for (int i = 1; i < arr.Length; i++)
            {
                result[i - 1] = arr[i];
            }

            return result;
        }

        private static bool SequenceLooksGood(List<Trial> set)
        {
            if (set.Count != 20)
            {
                //Console.WriteLine("Wrong Count");
                return false;
            }

            //if (set.GroupBy(e => new { e.Emotion, e.FS }).Any(e => e.Count() != 5))
            //{
            //    // Console.WriteLine("Wrong Balance");
            //    // Sequences are not balance
            //    return false;
            //}

            var firstHalf = set.Take(10).Count(e => e.Emotion == Emotion.Positive);
            if (firstHalf != 5)
            {
                // Console.WriteLine($"A block contains more positive than negative");
                return false;
            }


            // Positive Positive Positive NOT allowed
            int counter = 0;
            Emotion prevEmotion = set.First().Emotion;
            foreach (var item in set.Skip(1))
            {
                if (item.Emotion == prevEmotion)
                {
                    counter++;
                    if (counter >= 2)
                    {
                        // Console.WriteLine($"More than 3 consecutive emotions are the same");
                        return false;
                    }
                }
                else
                {
                    counter = 0;
                    prevEmotion = item.Emotion;
                }
            }

            // 1st, 1st, 1st NOT allowed
            counter = 0;
            FS prevFS = set.First().FS;
            foreach (var item in set.Skip(1))
            {
                if (item.FS == prevFS)
                {
                    counter++;
                    if (counter >= 2)
                    {
                        // Console.WriteLine($"More than 3 consecutive objects are the same");
                        return false;
                    }
                }
                else
                {
                    counter = 0;
                    prevFS = item.FS;
                }
            }

            return true;
        }

        private static bool CodesLooksGood(List<Trial> set)
        {
            // Check that codes do not repeat within 3 distance
            int distance = 3;
            for (int i = 0; i < set.Count - distance; i++)
            {
                for (int d = 1; d < distance; d++)
                {
                    if (set[i].Code == set[i + d].Code)
                    {
                        //Console.WriteLine($"Code is repeating within 3 s");
                        return false;
                    }
                }
            }

            // Check that all codes are equally represented in each 10 trial blocks
            if (set.Take(10).GroupBy(e => new { e.Emotion, e.Code }).Any(e => e.Count() > 1))
            {
                //Console.WriteLine($"Equal Representation 1");

                return false;
            }

            if (set.Skip(10).GroupBy(e => new { e.Emotion, e.Code }).Any(e => e.Count() > 1))
            {
                //Console.WriteLine($"Equal Representation 2");

                return false;
            }

            return true;
        }


        private static bool PhrasesLooksGood(List<Trial> set)
        {
            // Check that codes do not repeat within 3 distance
            int distance = 3;
            for (int i = 0; i < set.Count - distance; i++)
            {
                for (int d = 1; d < distance; d++)
                {
                    if (set[i].Phrase == set[i + d].Phrase)
                    {
                        // Console.WriteLine($"Code is repeating within 3 s");
                        return false;
                    }
                }
            }

            // Check that all phrases are equally represented in each 10 trial blocks
            if (set.Take(10).GroupBy(e => new { e.Emotion, e.Phrase }).Any(e => e.Count() > 1))
            {
                return false;
            }

            if (set.Skip(10).GroupBy(e => new { e.Emotion, e.Phrase }).Any(e => e.Count() > 1))
            {
                return false;
            }

            return true;
        }
    }

    class Trial
    {
        // Positive or Negative
        public Emotion Emotion { get; set; }

        public FS FS { get; set; }

        public int Code { get; set; }

        public int Phrase { get; set; }

        public override string ToString()
        {
            string property = Emotion == Emotion.Positive ? _properties[Code].P : _properties[Code].N;
            string obj = _objects[Code];
            string emotion = Emotion.ToString();
            string fs = FS == FS.First ? "1st" : "2nd";
            string phrase = Emotion == Emotion.Positive ? _phrases[Phrase].P : _phrases[Phrase].N;

            return $"{Code}\t{property} {obj}\t\t{emotion} {fs}\t\t{phrase}";
        }

        //public override string ToString()
        //{
        //    string emotion = Emotion.ToString();
        //    string fs = FS == FS.First ? "1st" : "2nd";

        //    return $"{emotion} {fs}";
        //}

        static Dictionary<int, string> _objects = new Dictionary<int, string>
        {
            [1] = "Mold",
            [2] = "Hose",
            [3] = "Wiper",
            [4] = "Pipe",
            [5] = "Pot"
        };

        static Dictionary<int, (string P, string N)> _properties = new Dictionary<int, (string, string)>
        {
            [1] = ("Beige", "Brown"),
            [2] = ("Green", "Black"),
            [3] = ("Purple", "White"),
            [4] = ("Pink", "Yellow"),
            [5] = ("Silver", "Black"),
        };

        static Dictionary<int, (string P, string N)> _phrases = new Dictionary<int, (string, string)>
        {
            [1] = ("Wahh so nice!", "Eeee, this is horrible!"),
            [2] = ("Yay, this is goooood!", "Aiyo, this is bad!"),
            [3] = ("Oooo, I like this!", "Yucks, this is gross!"),
            [4] = ("Wow, I want this!", "Ew, don't touch!"),
            [5] = ("Caaaaaaan!", "No, no!"),
        };
    }

    // 1st and 2nd
    enum FS
    {
        First,
        Second
    }

    enum Emotion
    {
        Positive,
        Negative
    }
}
