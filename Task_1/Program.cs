using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Task_1
{
    class Program
    {
        public static Thread t;
        public static readonly Random rnd = new Random();
        public static int numberOfParticipants;
        public static int secretNumber;
        public static List<Thread> participantList = new List<Thread>();
        public static bool correctGuess = false;
        public static readonly object l = new object();
        static void Main(string[] args)
        {  
            Console.WriteLine("\t\t\tWelcome to Number Guessing Game!");
            Thread t1 = new Thread(FirstThreadJob);
            Thread t2 = new Thread(SecondThreadJob);
            t2.Name = "Thread_Generator";
            t1.Start();
            t2.Start();
            t2.Join();
            foreach (var participant in participantList)
            {
                lock (l)
                {
                    participant.Start();
                }          
            }
            Console.ReadLine();
        }
        public static void FirstThreadJob()
        {
            SetSetNumberOfPeople();  
            secretNumber = rnd.Next(1, 100);
            Console.WriteLine("User chose "+numberOfParticipants+" participants.\nRandom number is generated.\nGood luck!");
        }
        public static void SecondThreadJob()
        {
            int temp = SetSetNumberOfPeople();
            for (int i = 1; i < temp+1; i++)
            {
                t = new Thread(guessingMethod);
                t.Name = string.Format("Participant_" + i);
                participantList.Add(t);
            }
        }
        public static void guessingMethod()
        {
            while (correctGuess == false)
            {
                int guess = rnd.Next(1, 100);
                Console.WriteLine(Thread.CurrentThread.Name + " is guessing with number " + guess);
                if (secretNumber == guess)
                {
                    Console.WriteLine(Thread.CurrentThread.Name + " won");
                    correctGuess = true;
                }
                else
                {
                    if (secretNumber % 2 == guess % 2)
                    {
                        Console.WriteLine(Thread.CurrentThread.Name + " guessed number parity");
                    }
                    else
                    {
                        Console.WriteLine("Wrong guess");
                    }
                }
                Thread.Sleep(100);
            }
        }
        public static int SetSetNumberOfPeople()
        {
            lock (l)
            {
                if (numberOfParticipants == 0)
                {
                    int temp = 0;
                    do
                    {
                        Console.WriteLine("How many participants will there be?");
                    } while (int.TryParse(Console.ReadLine(), out temp) != true);
                    numberOfParticipants = temp;
                    return 0;
                }
                else
                {
                    return numberOfParticipants;
                }               
            }
        }
    }
}
