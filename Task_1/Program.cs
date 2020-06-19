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
        public static readonly Random rnd = new Random();
        public static int numberOfParticipants;
        public static int secretNumber;
        public static List<Thread> participantList = new List<Thread>();
        public static bool correctGuess = false;
        static void Main(string[] args)
        {
            Console.WriteLine("\t\t\tWelcome to Number Guessing Game!");
        }
        public static void FirstThreadJob()
        {            
            do
            {
                Console.WriteLine("How many participants will there be?");
            } while (int.TryParse(Console.ReadLine(), out numberOfParticipants)!= true);
            secretNumber = rnd.Next(1, 100);
        }
        public static void SecondThreadJob()
        {
            for (int i = 0; i < numberOfParticipants; i++)
            {
                Thread t = new Thread(guessingMethod);
                t.Name = string.Format("Participant_" + i + 1);
                participantList.Add(t);
            }
        }
        public static void guessingMethod(object guess)
        {
            Console.WriteLine(Thread.CurrentThread.Name + " is guessing with number " + (int)guess);
            if (secretNumber == (int)guess)
            {
                Console.WriteLine(Thread.CurrentThread.Name + " won");
            }
        }
    }
}
