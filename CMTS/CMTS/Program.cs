 using System;
 using CMTS.Core;
 using CMTS.Core.implementation;
 using CMTS.Core.Utils;

 namespace CMTS
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string fileName = @"C:\Users\ksivaraj\Desktop\input.txt";
            ConferenceManager conferenceManager=new ConferenceManager(new TalkGenerator(new TalkValidator(new TrackGenerator(new Utils()))));
            try
            {
                conferenceManager.GenerateTrack(fileName);
            }
            catch (Exception e)
            {
                Console.WriteLine($"error -  {e}");
                throw;
            }

            Console.ReadLine();

        }
    }
}
