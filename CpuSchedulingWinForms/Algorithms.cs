using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CpuSchedulingWinForms
{
      public static class Algorithms
{

    public static void fcfsAlgorithm(string userInput)
    {
        int np = Convert.ToInt16(userInput);
        int npX2 = np * 2;

        double[] bp = new double[np];
        double[] wtp = new double[np];
        string[] output1 = new string[npX2];
        double twt = 0.0, awt;
        int num;
        Console.WriteLine();
        Console.WriteLine("\nFirst-Come-First-Served: ");
        for (num = 0; num <= np - 1; num++)
        {
            //MessageBox.Show("Enter Burst time for P" + (num + 1) + ":", "Burst time for Process", MessageBoxButtons.OK, MessageBoxIcon.Question);
            Console.WriteLine("\nEnter Burst time for P" + (num + 1) + ":");


            // Microsoft.VisualBasic.Interaction.InputBox("Enter Burst time: ","Burst time for P" + (num + 1),-1, -1);
            var input = Console.ReadLine();



            bp[num] = Convert.ToInt32(input);
        }

        for (num = 0; num <= np - 1; num++)
        {
            if (num == 0)
            {
                wtp[num] = 0;
                Console.WriteLine("\nWaiting Time of Process" + (num + 1) + ":");
                Console.WriteLine(wtp[num]);
            }
            else
            {
                wtp[num] = wtp[num - 1] + bp[num - 1];
                Console.WriteLine("\nWaiting Time of Process " + (num + 1) + ":");
                Console.WriteLine(wtp[num]);
            }
        }

        for (num = 0; num <= np - 1; num++)
        {
            twt = twt + wtp[num];
            Console.WriteLine("\nTotal Waiting Time of Process " + (num + 1) + ":");
            Console.WriteLine(twt);
            Console.WriteLine();
            //total waiting time sum for each

        }


        awt = twt / np;
        Console.WriteLine("\nAverage Waiting Time of Processes:");
        Console.WriteLine(awt);


         Process[] processes = Process.GetProcesses();

        // to store CPU utilization for each process
        Dictionary<string, double> processCpuUtilizations = new Dictionary<string, double>();

        // Iterate through each process
        foreach (Process process in processes)
        {
            try
            {
                // Get the CPU usage of the process
                ProcessThread thread = new ProcessThread { num = process.Id };
                float cpuTime = 0;

                // Call the GetProcessorUtilization method to calculate the CPU usage
                cpuTime = GetProcessorUtilization(thread);

                // Add the CPU utilization to the dictionary
                if (!processCpuUtilizations.ContainsKey(process.ProcessName))
                {
                    processCpuUtilizations.Add(process.ProcessName, cpuTime);
                }
                else
                {
                    processCpuUtilizations[process.ProcessName] += cpuTime;
                }
            }
            catch (Exception)
            {
                // Handle any exceptions that may occur (e.g., process no longer exists)
                Console.WriteLine($"Error getting CPU usage for {process.ProcessName}");
            }
        }

        // Display the CPU utilization for each process
        Console.WriteLine("Process\tCPU Utilization (%)");
        foreach (KeyValuePair<string, double> entry in processCpuUtilizations)
        {
            Console.WriteLine($"{entry.Key}\t{entry.Value}");
        }
    }

    //  method to get the CPU utilization of a process
    public static float GetProcessorUtilization(ProcessThread thread)
    {
        ProcessThread proc = thread;
        ProcessThread p = new ProcessThread { ProcessID = proc.ProcessID };
        PerformanceCounter pc = new PerformanceCounter("Process", "% Processor Time", p.ProcessName, true);

        // initial CPU usage value
        pc.NextValue();

        Thread.Sleep(1000);

        // Get the current CPU usage value
        float cpuUsage = pc.NextValue();
        return cpuUsage;

    }



    public static void sjfAlgorithm(string userInput)
    {
        int np = Convert.ToInt16(userInput);

        double[] bp = new double[np];
        double[] wtp = new double[np];
        double[] p = new double[np];
        double twt = 0.0, awt;
        int x, num;
        double temp = 0.0;
        bool found = false;

        Console.WriteLine("\nShortest Job First");

        for (num = 0; num <= np - 1; num++)
        {
            Console.WriteLine("\nEnter Burst time for P" + (num + 1) + ":");


            // Microsoft.VisualBasic.Interaction.InputBox("Enter Burst time: ","Burst time for P" + (num + 1),-1, -1);
            var input = Console.ReadLine();



            bp[num] = Convert.ToInt32(input);
        }

        for (num = 0; num <= np - 1; num++)
        {
            p[num] = bp[num];
        }

        for (x = 0; x <= np - 2; x++)
        {
            for (num = 0; num <= np - 2; num++)
            {
                if (p[num] > p[num + 1])
                {
                    temp = p[num];
                    p[num] = p[num + 1];
                    p[num + 1] = temp;
                }
            }
        }

        for (num = 0; num <= np - 1; num++)
        {
            if (num == 0)
            {
                for (x = 0; x <= np - 1; x++)
                {
                    if (p[num] == bp[x] && found == false)
                    {
                        wtp[num] = 0;
                        Console.WriteLine("\nWaiting Time of Process " + (num + 1) + ":");
                        Console.WriteLine(
                            wtp[num]); //Console.WriteLine("\nWaiting time for P" + (x + 1) + " = " + wtp[num]);
                        bp[x] = 0;
                        found = true;
                    }
                }

                found = false;
            }
            else
            {
                for (x = 0; x <= np - 1; x++)
                {
                    if (p[num] == bp[x] && found == false)
                    {
                        wtp[num] = wtp[num - 1] + p[num - 1];
                        Console.WriteLine("\nWaiting Time of Process " + (num + 1) + ":");
                        Console.WriteLine(wtp[num]);
                        bp[x] = 0;
                        found = true;
                    }
                }

                found = false;
            }
        }

        for (num = 0; num <= np - 1; num++)
        {
            twt = twt + wtp[num];
            Console.WriteLine("\nTotal Waiting Time of Process " + (num + 1) + ":");
            Console.WriteLine(twt);
        }

        Console.WriteLine("\nTotal Waiting Time of Processes:");
        Console.WriteLine(twt);
    }


    public static void priorityAlgorithm(string userInput)
    {
        int np = Convert.ToInt16(userInput);


        double[] bp = new double[np];
        double[] wtp = new double[np + 1];
        int[] p = new int[np];
        int[] sp = new int[np];
        int x, num;
        double twt = 0.0;
        double awt;
        int temp = 0;
        bool found = false;
        Console.WriteLine("\nPriority: ");
        for (num = 0; num <= np - 1; num++)
        {
            /*string input =
                Microsoft.VisualBasic.Interaction.InputBox("Enter burst time: ",
                    "Burst time for P" + (num + 1),
                    "",
                    -1, -1);

            bp[num] = Convert.ToInt64(input);*/

            Console.WriteLine("\nEnter Burst time for P" + (num + 1) + ":");


            // Microsoft.VisualBasic.Interaction.InputBox("Enter Burst time: ","Burst time for P" + (num + 1),-1, -1);
            var input = Console.ReadLine();



            bp[num] = Convert.ToInt32(input);

        }


        for (num = 0; num <= np - 1; num++)
        {
            Console.WriteLine("Enter priority: ",
                "Priority for P" + (num + 1),
                "",
                -1, -1);
            string input2 = Console.ReadLine();


            p[num] = Convert.ToInt16(input2);
        }

        for (num = 0; num <= np - 1; num++)
        {
            sp[num] = p[num];
        }

        for (x = 0; x <= np - 2; x++)
        {
            for (num = 0; num <= np - 2; num++)
            {
                if (sp[num] > sp[num + 1])
                {
                    temp = sp[num];
                    sp[num] = sp[num + 1];
                    sp[num + 1] = temp;
                }
            }
        }

        for (num = 0; num <= np - 1; num++)
        {
            if (num == 0)
            {
                for (x = 0; x <= np - 1; x++)
                {
                    if (sp[num] == p[x] && found == false)
                    {
                        wtp[num] = 0;
                        Console.WriteLine("\nWaiting Time of Process " + (x + 1) + ":");
                        Console.WriteLine(
                            wtp[num]); //Console.WriteLine("\nWaiting time for P" + (x + 1) + " = " + wtp[num]);
                        temp = x;
                        p[x] = 0;
                        found = true;
                    }
                }

                found = false;
            }
            else
            {
                for (x = 0; x <= np - 1; x++)
                {
                    if (sp[num] == p[x] && found == false)
                    {
                        wtp[num] = wtp[num - 1] + bp[temp];
                        Console.WriteLine("\nWaiting time for P" + (x + 1));
                        Console.WriteLine(
                            wtp[num]);
                        temp = x;
                        p[x] = 0;
                        found = true;
                    }
                }

                found = false;
            }
        }

        for (num = 0; num <= np - 1; num++)
        {
            Console.WriteLine("\nTotal Waiting Time of Process " + (num + 1) + ":");
            twt = twt + wtp[num];
        }

    }

    public static void roundRobinAlgorithm(string userInput)
    {
        int np = Convert.ToInt16(userInput);
        int i, counter = 0;
        double total = 0.0;
        double timeQuantum;
        double waitTime = 0, turnaroundTime = 0;
        double averageWaitTime, averageTurnaroundTime;
        double[] arrivalTime = new double[np];
        double[] burstTime = new double[10];
        double[] temp = new double[10];
        int x = np;int timeQuantumInput =0;
        Console.WriteLine("\nRound Robin: ");

        for (i = 0; i < np; i++)
        {
            Console.WriteLine("\nEnter Arrival time for P" + (i + 1) + ":");


            // Microsoft.VisualBasic.Interaction.InputBox("Enter Burst time: ","Burst time for P" + (num + 1),-1, -1);
            var arrivalInput = Console.ReadLine();



            arrivalTime[i] = Convert.ToInt32(arrivalInput);

            Console.WriteLine("\nEnter Burst time for P" + (i + 1) + ":");


            // Microsoft.VisualBasic.Interaction.InputBox("Enter Burst time: ","Burst time for P" + (num + 1),-1, -1);
            var burstInput = Console.ReadLine();



            burstTime[i] = Convert.ToInt32(burstInput);

            /* string arrivalInput =
                     Microsoft.VisualBasic.Interaction.InputBox("Enter arrival time: ",
                                                        "Arrival time for P" + (i + 1),
                                                        "",
                                                        -1, -1);

             arrivalTime[i] = Convert.ToInt64(arrivalInput);

             string burstInput =
                     Microsoft.VisualBasic.Interaction.InputBox("Enter burst time: ",
                                                        "Burst time for P" + (i + 1),
                                                        "",
                                                        -1, -1);

             burstTime[i] = Convert.ToInt64(burstInput);*/

            temp[i] = burstTime[i];
        }



        Console.WriteLine("Enter time quantum: ");
       ;

        timeQuantum = 2;
       timeQuantum = Convert.ToInt64(timeQuantumInput);


        for (total = 0, i = 0; x != 0;)
        {
            if (temp[i] <= timeQuantum && temp[i] > 0)
            {
                total = total + temp[i];
                temp[i] = 0;
                counter = 1;
            }
            else if (temp[i] > 0)
            {

                temp[i] = temp[i] - timeQuantum;
                total = total + timeQuantum;
                // Console.WriteLine("\nTotal Waiting Time of P" + (i + 1) + ": " + total);
            }

            if (temp[i] == 0 && counter == 1)
            {
                x--;
                //printf("nProcess[%d]tt%dtt %dttt %d", i + 1, burst_time[i], total - arrival_time[i], total - arrival_time[i] - burst_time[i]);
                Console.WriteLine("Turnaround time for Process " + (i + 1) + " : " + (total - arrivalTime[i]),
                    "Turnaround time for Process " + (i + 1));
                Console.WriteLine("Wait time for Process " + (i + 1) + " : " + (total - arrivalTime[i] - burstTime[i]),
                    "Wait time for Process " + (i + 1));
                turnaroundTime = (turnaroundTime + total - arrivalTime[i]);
                waitTime = (waitTime + total - arrivalTime[i] - burstTime[i]);
                counter = 0;
            }

            if (i == np - 1)
            {
                i = 0;
            }
            else if (arrivalTime[i + 1] <= total)
            {
                i++;
            }
            else
            {
                i = 0;
            }
        }

        averageWaitTime = Convert.ToInt64(waitTime * 1.0 / np);
        averageTurnaroundTime = Convert.ToInt64(turnaroundTime * 1.0 / np);
        Console.WriteLine("Average wait time for " + np + " processes: " + averageWaitTime + " sec(s)", "");
        Console.WriteLine("Average turnaround time for " + np + " processes: " + averageTurnaroundTime + " sec(s)", "");
    }




    public static void srtfAlgorithm(string userInput)
    {
        int np = Convert.ToInt16(userInput);
        int npX2 = np * 2;
        int input;
        double[] bp = new double[np];
        double[] wtp = new double[np];
        string[] output1 = new string[npX2];
        double twt = 0.0, awt;


        int i, completedCounter = 0, processesLeft = np;
        double total = 0.0;

        double waitTime = 0;
           double[] turnaroundTime=new double[np]; ;
        double averageWaitTime, averageTurnaroundTime;

       double[] arrivalTime = new double[np];

        double[] burstTime = new double[np];
         int num=arrivalTime.Length;

        double[] remainingTime = new double[np];
        int currentTime = 0;
        int remainingTime2 = 0;
        double[] completionTime = new double[np];
        double[] tat = new double[np];
        double[] wt = new double[np];
        double[] completedCounters = new double[np+1];
        double[] ready = new double[np+1];
        int endTime = npX2;

        int completedP =0;
        int shortestRemainingTimeIndex = 0;

        int smallest;
        double[] temp = new double[np];
        int x = np;





        Console.WriteLine("\nShortest Remaining Time First:");
       for (num = 0; num <= np - 1; num++)
        {
            Console.WriteLine("\nEnter Arrival time for P" + (num + 1) + ":");


            // Microsoft.VisualBasic.Interaction.InputBox("Enter Burst time: ","Burst time for P" + (num + 1),-1, -1);
            var arrivalInput = Console.ReadLine();



            arrivalTime[num] = Convert.ToInt32(arrivalInput);

            Console.WriteLine("\nEnter Burst time for P" + (num + 1) + ":");


            // Microsoft.VisualBasic.Interaction.InputBox("Enter Burst time: ","Burst time for P" + (num + 1),-1, -1);
            var burstInput = Console.ReadLine();



            burstTime[num] = Convert.ToInt32(burstInput);
            /* //MessageBox.Show("Enter Burst time for P" + (num + 1) + ":", "Burst time for Process", MessageBoxButtons.OK, MessageBoxIcon.Question);
             //Console.WriteLine("\nEnter Burst time for P" + (num + 1) + ":");

             string arrivalInput =
             Microsoft.VisualBasic.Interaction.InputBox("Enter arrival time: ",
                                                "Arrival time of P" + (num + 1),
                                                "",
                                                -1, -1);

             arrivalTime[num] = Convert.ToInt64(arrivalInput);

             //var input = Console.ReadLine();
             //bp[num] = Convert.ToInt32(input);
         }
         for (num = 0; num <= np - 1; num++)
         {
             //MessageBox.Show("Enter Burst time for P" + (num + 1) + ":", "Burst time for Process", MessageBoxButtons.OK, MessageBoxIcon.Question);
             //Console.WriteLine("\nEnter Burst time for P" + (num + 1) + ":");

             string burstInput =
                 Microsoft.VisualBasic.Interaction.InputBox("Enter Burst time: ",
                     "Burst time of P" + (num + 1),
                     "",
                     -1, -1);

             burstTime[num] = Convert.ToInt64(burstInput);

             //var input = Console.ReadLine();
             //bp[num] = Convert.ToInt32(input);


*/
        }


        for(i = 0; i < num; i++)
        {
            remainingTime[i]=burstTime[i];
        }

        while (completedP < num)
        {


            shortestRemainingTimeIndex = -1;
            int minRemainingTime = int.MaxValue;
            for (int a = 0; a< num; a++)
            {
                if (remainingTime[a] > 0 && arrivalTime[a] <= currentTime && remainingTime[a] < minRemainingTime)
                {

                    minRemainingTime = (int)remainingTime[a];
                    shortestRemainingTimeIndex = a;
                }
            }

            if (shortestRemainingTimeIndex == -1)
            {
                currentTime++;
                continue;
            }

            remainingTime[shortestRemainingTimeIndex]--;
            currentTime++;

            if (remainingTime[shortestRemainingTimeIndex] == 0)
            {

                completedP++;
                completionTime[shortestRemainingTimeIndex] = currentTime;
                turnaroundTime[shortestRemainingTimeIndex] = currentTime - arrivalTime[shortestRemainingTimeIndex];
                wt[shortestRemainingTimeIndex] = turnaroundTime[shortestRemainingTimeIndex] -burstTime[shortestRemainingTimeIndex];
            }
        }

        // Calculate and display results
        Console.WriteLine("Process\tArrival\tBurst\tComplete\tWait\tTurnaround");
        for (int d = 0; d < num; d++)
        {
            Console.WriteLine(
                $"{d + 1}\t{arrivalTime[d]}\t{burstTime[d]}\t{completionTime[d]}\t\t{wt[d]}\t{turnaroundTime[d]}");
        }

        // Calculate and display average waiting and turnaround times
        double twt1 = 0;
        double totalTurnaround = 0;
        for (int d = 0; d < num; d++)
        {

            twt1 += wt[d];
            totalTurnaround += turnaroundTime[d];
        }

        Console.WriteLine($"Average Waiting Time: {twt1 / num}");
        Console.WriteLine($"Average Turnaround Time: {totalTurnaround / num}");









        //num=current Process, np = total number of processes
        //if at[num] = 0, execute, until next at[num] , if bt[num] < bt[num] execute
        //until next at[num] , if bt[num] < bt[num] execute
        //if all executed, start over with bt[num]<bt[num]








            }





        public static void lrtfAlgorithm(string userInput)
        {
            int np1 = Convert.ToInt16(userInput);
            int npX2 = np1 * 2;
            int input;
            double[] bp = new double[np1];
            double[] wtp = new double[np1];
            string[] output1 = new string[npX2];
            double twt = 0.0, awt;


            int i, completedCounter = 0, processesLeft = np1;
            double total = 0.0;

            double waitTime = 0;
            double[] turnaroundTime=new double[np1]; ;
            double averageWaitTime, averageTurnaroundTime;

            double[] arrivalTime = new double[np1];

            double[] burstTime1 = new double[np1];
            int num=arrivalTime.Length;

            double[] remainingTime = new double[np1];
            int currentTime = 0;
            int remainingTime2 = 0;
            double[] completionTime = new double[np1];
            double[] tat = new double[np1+1];
            double[] wt = new double[np1+1];
            double[] completedCounters = new double[np1+1];
            double[] ready = new double[np1+1];
            int endTime = npX2;

            int completedP =0;
            int longestRemainingTimeIndex = 0;



            Console.WriteLine("\nLongest Remaining Time First:");
                for (num = 0; num <= np1 - 1; num++)
                {Console.WriteLine("\nEnter Arrival time for P" + (num+1) + ":");


                    // Microsoft.VisualBasic.Interaction.InputBox("Enter Burst time: ","Burst time for P" + (num + 1),-1, -1);
                    var arrivalInput = Console.ReadLine();



                    arrivalTime[num] = Convert.ToInt32(arrivalInput);

                    Console.WriteLine("\nEnter Burst time for P" + (num+1) + ":");


                    // Microsoft.VisualBasic.Interaction.InputBox("Enter Burst time: ","Burst time for P" + (num + 1),-1, -1);
                    var burstInput = Console.ReadLine();



                    burstTime1[num] = Convert.ToInt32(burstInput);
                    /*//MessageBox.Show("Enter Burst time for P" + (num + 1) + ":", "Burst time for Process", MessageBoxButtons.OK, MessageBoxIcon.Question);
                    //Console.WriteLine("\nEnter Burst time for P" + (num + 1) + ":");

                    string input =
                        Microsoft.VisualBasic.Interaction.InputBox("Enter Burst time: ",
                            "Burst time for P" + (num + 1),
                            "",
                            -1, -1);

                    bp1[num] = Convert.ToInt64(input);

                    //var input = Console.ReadLine();
                    //bp[num] = Convert.ToInt32(input);*/
                }


            /*
                if(array[i] < min)
                {
                    min = array[i];
                }*/
            int counter = 1;
           // double maxRemainingTime=0;


        for(i = 0; i < num; i++)
        {
            remainingTime[i]=burstTime1[i];
        }


        while (completedP < num)
        {


            longestRemainingTimeIndex = -1;
            double maxRemainingTime = remainingTime.Max(); //figure out how to turn complete table around and fix wait/turnaround
            for (int a = 0; a < num; a++)
            {


              /*  for (int j = 0; j < num; j++)
                {
                    maxRemainingTime = remainingTime.Max();
                }
*/


                if (remainingTime[a] > 0 && arrivalTime[a] <= currentTime && remainingTime[a]>= maxRemainingTime)
                {

                    maxRemainingTime = remainingTime[a];
                    longestRemainingTimeIndex = a;
                }

            }

            if (longestRemainingTimeIndex == -1)
            {
                currentTime++;
                continue;
            }

            remainingTime[longestRemainingTimeIndex]--;
            currentTime++;
            counter--;

            if (remainingTime[longestRemainingTimeIndex] == 0)
            {

                completedP++;
                completionTime[longestRemainingTimeIndex] = currentTime;
                turnaroundTime[longestRemainingTimeIndex] = currentTime - arrivalTime[longestRemainingTimeIndex];
                wt[longestRemainingTimeIndex] = turnaroundTime[longestRemainingTimeIndex] -burstTime1[longestRemainingTimeIndex];
            }
        }

        // Calculate and display results
        Console.WriteLine("Process\tArrival\tBurst\tComplete\tWait\tTurnaround");
        for (int d = 0; d < num; d++)
        {
            Console.WriteLine(
                $"{d + 1}\t{arrivalTime[d]}\t{burstTime1[d]}\t{completionTime[d]}\t\t{wt[d]}\t{turnaroundTime[d]}");
        }

        // Calculate and display average waiting and turnaround times
        double twt1 = 0;
        double totalTurnaround = 0;
        for (int d = 0; d < num; d++)
        {

            twt1 += wt[d];
            totalTurnaround += turnaroundTime[d];
        }

        Console.WriteLine($"Average Waiting Time: {twt1 / num}");
        Console.WriteLine($"Average Turnaround Time: {totalTurnaround / num}");


                }











        static void Main(String[] args)
        {
            fcfsAlgorithm("4");
            sjfAlgorithm("4");
            priorityAlgorithm("4");

          roundRobinAlgorithm("4");
            srtfAlgorithm("4");
            lrtfAlgorithm("4");

           Process[] processes = Process.GetProcesses();

        // to store CPU utilization for each process
        Dictionary<string, double> processCpuUtilizations = new Dictionary<string, double>();

        // iterate through each process
        foreach (Process process in processes)
        {
            try
            {
                //  CPU usage of the process
                ProcessThread thread = new ProcessThread { num = process.Id };
                float cpuTime = 0;

                //  calculate the CPU usage
                cpuTime = GetProcessorUtilization(thread);

                //the CPU utilization to the dictionary
                if (!processCpuUtilizations.ContainsKey(process.ProcessName))
                {
                    processCpuUtilizations.Add(process.ProcessName, cpuTime);
                }
                else
                {
                    processCpuUtilizations[process.ProcessName] += cpuTime;
                }
            }
            catch (Exception)
            {
                // exceptions that may occur
                Console.WriteLine($"Error getting CPU usage for {process.ProcessName}");
            }
        }

        //  the CPU utilization for each process
        Console.WriteLine("Process\tCPU Utilization (%)");
        foreach (KeyValuePair<string, double> entry in processCpuUtilizations)
        {
            Console.WriteLine($"{entry.Key}\t{entry.Value}");
        }
    }

    //  method to get the CPU utilization of a process
    public static float GetProcessorUtilization(ProcessThread thread)
    {
        ProcessThread proc = thread;
        ProcessThread p = new ProcessThread { ProcessID = proc.ProcessID };
        PerformanceCounter pc = new PerformanceCounter("Process", "% Processor Time", p.ProcessName, true);

        // initial CPU usage value
        pc.NextValue();

        Thread.Sleep(1000);

        // Get the current CPU usage value
        float cpuUsage = pc.NextValue();
        return cpuUsage;

    }


        }


        }

    }


