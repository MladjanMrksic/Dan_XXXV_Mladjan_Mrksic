using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

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
            //Creating, naming and starting the threads in a particular order
            Thread t1 = new Thread(FirstThreadJob);
            Thread t2 = new Thread(SecondThreadJob);
            t2.Name = "Thread_Generator";
            t1.Start();
            t2.Start();
            t2.Join();
            t1.Join();
            //Looping through participantsList and starting each thread
            foreach (var participant in participantList)
            {
                participant.Start();
            }
            Console.ReadLine();
        }
        //FirstThreadJob only contains one method for setting up the game
        public static void FirstThreadJob()
        {
            GameSetup();
            Console.WriteLine("User chose " + numberOfParticipants + " participants.\nRandom number is chosen.\nGood luck!");
        }
        //SecondThreadJob creates threads, names them and adds them to the participantsList
        public static void SecondThreadJob()
        {
            int temp = GameSetup();
            for (int i = 1; i < temp+1; i++)
            {
                t = new Thread(GuessingMethod);
                t.Name = string.Format("Participant_" + i);
                participantList.Add(t);
            }
        }
        //GuessingMethod is the main logic behind this game.
        public static void GuessingMethod()
        {
            //While loop keeps the participants guessing until the correct number is found
            while (correctGuess == false)
            {
                //Lock ensures that only one thread can access the critical section
                lock (l)
                {
                    //Random number is generated and compared against the scretNumber
                    int guess = rnd.Next(1, 100);
                    Console.WriteLine(Thread.CurrentThread.Name + " is guessing with number " + guess);
                    if (secretNumber == guess)
                    {
                        //If the number is correct, correctGuess is changed to true so no more tries are attempted
                        Console.WriteLine(Thread.CurrentThread.Name + " won");
                        correctGuess = true;
                    }
                    else
                    {
                        //If the number is not correct, it's checked if it has the same parity as the secretNumber
                        if (secretNumber % 2 == guess % 2)
                            Console.WriteLine(Thread.CurrentThread.Name + " guessed number parity");
                        else
                            Console.WriteLine("Wrong guess");
                    }
                }
                Thread.Sleep(100);
            }
        }
        //GameSetup serves as get/set method for numberOfParticipants.
        public static int GameSetup()
        {
            //This lock makes sure only one of two threads can access this critical section at a time
            lock (l)
            {
                //If the numberOfParticipants in not set, this part of the code is executed
                if (numberOfParticipants == 0)
                {
                    int temp = 0;
                    do
                    {
                        Console.WriteLine("How many participants will there be?");
                    } while (int.TryParse(Console.ReadLine(), out temp) != true);
                    numberOfParticipants = temp;
                    do
                    {
                        Console.WriteLine("Please enter a number from 1-100 that participants need to guess.");
                    } while (int.TryParse(Console.ReadLine(), out secretNumber) != true || Enumerable.Range(1, 100).Contains(secretNumber) != true);                    
                    return 0;
                }
                //If the numberOfParticipants is set to a value, this part returns the value
                else
                    return numberOfParticipants;
            }
        }
    }
}
