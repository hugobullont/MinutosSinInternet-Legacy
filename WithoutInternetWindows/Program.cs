using System;
using System.IO;
using System.Net.NetworkInformation;

namespace WithoutInternetWindows
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("MinutosSinInternet - AWS US East Virginia");
            DoPing();
        }

        public static void DoPing()
        {
            bool internet;
            double totalMinutes = 0.0;
            DateTime startTime = DateTime.Now;
            DateTime endTime;
            bool withOutInternet = false;
            var logPath = "MinutosSinInternet.txt";
            using (var writer = File.AppendText(logPath))
            {
                string lineTXT = "Minutos sin Internet - Ping: AWS US East Virginia - Hora de Inicio: " + DateTime.Now.ToString();
                writer.WriteLine(lineTXT);
            }
            while (true)
            {
                Ping pinger = new Ping();
                string line;
                PingReply reply;
                IPStatus status;
                try
                {
                    reply = pinger.Send("quicksight.us-east-1.amazonaws.com");
                    line = DateTime.Now.ToString() + " " + reply.Status.ToString() + " " + reply.RoundtripTime.ToString() + "ms";
                    status = reply.Status;
                }
                catch (Exception ex)
                {
                    line = DateTime.Now.ToString() + " ERROR " + ex.Message;
                    status = IPStatus.DestinationHostUnreachable;
                }
                
                
                if (status == IPStatus.Success) { internet = true; }
                else { internet = false; }
                if (withOutInternet == false && internet == false)
                {
                    startTime = DateTime.Now;
                    withOutInternet = true;
                }
                if (withOutInternet == true && internet == true)
                {
                    endTime = DateTime.Now;
                    withOutInternet = false;
                    using (var writer = File.AppendText(logPath))
                    {
                        TimeSpan timeWithOutInternet = endTime - startTime;
                        totalMinutes += timeWithOutInternet.TotalMinutes;
                        string lineTXT = startTime.ToString() + " - " + endTime.ToString() + " - " + timeWithOutInternet.TotalMinutes.ToString() + " minutos sin internet. Total Acumulado: " + totalMinutes.ToString() + " minutos.";
                        writer.WriteLine(lineTXT);
                    }
                }
                Console.WriteLine(line);
                GC.Collect();
            }
        }
    }
}
