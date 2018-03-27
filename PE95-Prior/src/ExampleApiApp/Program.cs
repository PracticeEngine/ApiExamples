using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ExampleApiApp
{
    /// <summary>
    /// This is an example API Connection using a Console Application
    /// Add the 'PracticeEngine.WebApi' nuget package.  This just eases the connection process by using a Handler we have pre-written.
    /// !!! NUGET is not used for systems running 9.6 and above !!!
    /// </summary>
    class Program
    {
        // 1. Set/Read your connection details as necessary (typically you want these in a config file somewhere)
        const string apiBaseAddress = "https://yourserver/PE/";
        const string appID = "??????????????????";
        const string appKey = "??????????????????";

        static void Main(string[] args)
        {
            var p = new Program();
            p.RunExamples();
        }

        public Program()
        {

        }

        public void RunExamples()
        { 
            // 2. Create a Handler from our Nuget package, passing in the AppId and App Key
            var handler = new PE.ApiKey.PEApiKeyHandler(appID, appKey);


            // 3. Create standard Http Client with the Handler
            var peClient = HttpClientFactory.Create(handler);

            // Run Examples
            Console.WriteLine();
            Console.WriteLine("Running Synchronous Example");
            SyncronousExample(peClient);

            Console.WriteLine();
            Console.WriteLine("Running Asynchronous Example");
            var task = AsyncronousExample(peClient);
            task.Wait();


            // Just for Run-ability/Readability
            Console.WriteLine();
            Console.WriteLine("PRESS ENTER KEY TO EXIT THE DEMO");
            Console.ReadLine();
        }

        void SyncronousExample(HttpClient peClient)
        {
            // 4. Call the StaffMember/Me function to see who you have logged in as
            var task = peClient.GetStringAsync(apiBaseAddress + "api/StaffMember/Me");

            // 5. Get the Result
            var response = task.Result;

            // 6. Convert from JSON
            JObject obj = JObject.Parse(response);

            // 7. Write out your Name from the PEStaff Object that was Returned
            Console.WriteLine("Synchronous Returned {0}", obj.Property("StaffName").Value);

        }

        async Task AsyncronousExample(HttpClient peClient)
        {
            // 4. Call the StaffMember/Me function to see who you have logged in as
            var response = await peClient.GetStringAsync(apiBaseAddress + "api/StaffMember/Me");

            // 5. Convert from JSON
            JObject obj = JObject.Parse(response);

            // 6. Write out your Name from the PEStaff Object that was Returned
            Console.WriteLine("Asynchronous Returned {0}", obj.Property("StaffName").Value);
        }
    }
}
