using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace ConsoleApp1
{
    class Program
    {
        // Replace with Values from your Database (setup in the Api Access Page) - need for auth
        const string appID = "??????????????????";
        const string appKey = "??????????????????";
        const string peRootUrl = "https://yourserver";

        // Data to Use as Example ClientID
        const int ContIndex = 782;

        static void Main(string[] args)
        {
            string apiAuthAddress;
            string apiBaseAddress;

            try
            {
                UriBuilder builder = new UriBuilder(peRootUrl);
                builder.Path = "/auth";                 // Important - DO NOT Include trailing '/' on the AuthAddress
                apiAuthAddress = builder.Uri.AbsoluteUri;
                builder.Path = "/pe/";
                apiBaseAddress = builder.Uri.AbsoluteUri;

                // Create the Api Class that handles Auth and makes calls for us
                var api = new PracEngApi(appID, appKey, apiAuthAddress);
                Console.Clear();

                // Just do a basic call
                Console.WriteLine($"Getting Who I Am");
                var whoAmITask = api.ApiGet($"{apiBaseAddress}api/StaffMember/Me");
                var whoAmI = whoAmITask.Result;
                Console.WriteLine(whoAmI.ToString(Formatting.Indented));

                // Use the Api to request something
                Console.WriteLine($"Getting Client Details for {ContIndex}");
                var meTask = api.ApiGet($"{apiBaseAddress}api/Clients/WhoIs/{ContIndex}");
                var me = meTask.Result;
                Console.WriteLine(me.ToString(Formatting.Indented));

                // Use the Api to request something
                Console.WriteLine($"Getting Jobs for ContIndex {ContIndex}");
                var getTask = api.ApiGet($"{apiBaseAddress}api/Jobs/GetJobsForClient/{ContIndex}");
                JArray jobs = (JArray)getTask.Result;

                // output some details of what we found
                Console.WriteLine($"Found {jobs.Count} jobs");
                Console.WriteLine();
                Console.WriteLine($"\tID\tName");
                foreach (JObject job in jobs)
                {
                    Console.WriteLine($"\t{job.GetValue("Job_Idx")}\t{job.GetValue("Job_Name")}");
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(ex);
            }
            Console.WriteLine("Press Enter Key to quit.");
            Console.ReadLine();
        }

    }
}
