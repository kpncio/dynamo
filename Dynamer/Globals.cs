using System.Net;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Dynamer
{
    public class Json
    {
        public static void Read()
        {
            // Logic to read Config.json with Newtonsoft.JSON

            Console.WriteLine("Reading config file...");

            try
            {
                if (File.Exists("Config.json"))
                {
                    string json = File.ReadAllText("Config.json");

                    JObject token = JObject.Parse(json);

                    Globals.Email = (string)token["Email"]!;
                    Globals.ApiKey = (string)token["ApiKey"]!;
                    Globals.Domain = (string)token["Domain"]!;
                    Globals.IPv4 = (bool)token["IPv4"]!;
                    Globals.Rate = (int)token["Rate"]!;
                }
                else
                {
                    Write();
                    Read();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("[ERROR]: Could not read config file...");
                Console.WriteLine(ex.ToString());
            }
        }

        public static void Write()
        {
            // Logic to write Config.json with Newtonsoft.JSON

            Console.WriteLine("Writing config file...");

            try
            {
                StreamWriter sw = new StreamWriter("Config.json");
                using (JsonWriter w = new JsonTextWriter(sw))
                {
                    w.Formatting = Formatting.Indented;

                    w.WriteStartObject();

                    w.WritePropertyName("Email");
                    w.WriteValue(Globals.Email);

                    w.WritePropertyName("ApiKey");
                    w.WriteValue(Globals.ApiKey);

                    w.WritePropertyName("Domain");
                    w.WriteValue(Globals.Domain);

                    w.WritePropertyName("IPv4");
                    w.WriteValue(Globals.IPv4);

                    w.WritePropertyName("Rate");
                    w.WriteValue(Globals.Rate);

                    w.WriteEndObject();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("[ERROR]: Could not write config file...");
                Console.WriteLine(ex.ToString());
            }
        }
    }

    public static class Globals
    {
        // Global variables...

        public static string? Existing { get; set; } = null;

        public static string? Email { get; set; } = null;
        public static string? ApiKey { get; set; } = null;
        public static string? Domain { get; set; } = null;
        public static bool? IPv4 { get; set; } = null;
        public static int? Rate { get; set; } = null;
    }

    public static class Utilities
    {
        public static void ForceInternet()
        {
            // Only allow users to continue with a valid internet connection...

            while (!Internet())
            {
                Console.WriteLine("[ERROR]: Connot connect to Cloudflare...");
                Console.WriteLine("Press enter to proceed...");
                Console.ReadLine();
            }
        }

        public static bool Internet()
        {
            // Get internet connectivity status...

            try
            {
                string url = "https://api.cloudflare.com/client/v4";

                var request = (HttpWebRequest)WebRequest.Create(url);
                request.KeepAlive = false;
                request.Timeout = 2000;
                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
