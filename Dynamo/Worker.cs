using RestSharp;
using Newtonsoft.Json.Linq;

namespace Dynamo
{
    internal class Worker
    {
        public static void Process()
        {
            var client = new RestClient();
            var request = new RestRequest();
            var response = new RestResponse();

            var token = new JObject();
            string type = "";
            string content = "";
            string zone = "";
            string record = "";
            bool proxied = true;
            int ttl = 1;

            // Utilities.ForceInternet();

            try
            {
                string provider = (bool)Globals.IPv4! ? "https://ip.kpnc-servers.net/" : "https://app.kpnc.io/geolocater/cloud";
                client = new RestClient(provider);
                request = new RestRequest();
                request.Method = Method.Get;
                request.AddHeader("Content-Type", "application/json");
                response = client.Execute(request);

                token = JObject.Parse(response.Content!);
                var provided = token["provided"]!;
                type = (string)provided["version"]! == "4" ? "A" : "AAAA";
                content = (string)provided["address"]!;

                Console.WriteLine($"Successfully got address ({type}|{content})...");

                if (Globals.Existing != null && Globals.Existing == content)
                {
                    Console.WriteLine("Current and existing addresses match - not patching...");

                    return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("[ERROR]: Could not get address...");
                Console.WriteLine(ex.ToString());
            }

            try
            {
                client = new RestClient($"https://api.cloudflare.com/client/v4/zones?name={Globals.Domain}&status=active&match=all");
                request = new RestRequest();
                request.Method = Method.Get;
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("X-Auth-Email", Globals.Email!);
                request.AddHeader("X-Auth-Key", Globals.ApiKey!);
                response = client.Execute(request);

                token = JObject.Parse(response.Content!);
                var result = token!.First!.First!.First!;
                zone = (string)result["id"]!;

                if ((bool)token["success"]!)
                {
                    Console.WriteLine($"Successfully got zone ({zone})...");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("[ERROR]: Could not get zone...");
                Console.WriteLine(ex.ToString());
            }

            try
            {
                client = new RestClient($"https://api.cloudflare.com/client/v4/zones/{zone}/dns_records?type={type}&name={Globals.Domain}&match=all");
                request = new RestRequest();
                request.Method = Method.Get;
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("X-Auth-Email", Globals.Email!);
                request.AddHeader("X-Auth-Key", Globals.ApiKey!);
                response = client.Execute(request);

                token = JObject.Parse(response.Content!);
                var result = token!.First!.First!.First!;
                Globals.Existing = (string)result["content"]!;
                record = (string)result["id"]!;
                proxied = (bool)result["proxied"]!;
                ttl = (int)result["ttl"]!;

                if ((bool)token["success"]!)
                {
                    Console.WriteLine($"Successfully got record ({record})...");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("[ERROR]: Could not get record...");
                Console.WriteLine("[INFO]: Please preload a matching record in your Cloudflare dashboard, so that Dynamo can copy and patch existing settings...");
                Console.WriteLine(ex.ToString());
            }

            try
            {
                client = new RestClient($"https://api.cloudflare.com/client/v4/zones/{zone}/dns_records/{record}");
                request = new RestRequest();
                request.Method = Method.Patch;
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("X-Auth-Email", Globals.Email!);
                request.AddHeader("X-Auth-Key", Globals.ApiKey!);
                request.RequestFormat = DataFormat.Json;
                request.AddJsonBody(new { name = Globals.Domain, content, type, comment = "Set by KPNC Dynamo...", proxied, ttl });
                response = client.Execute(request);

                token = JObject.Parse(response.Content!);

                if ((bool)token["success"]!)
                {
                    Console.WriteLine($"Successfully patched record ({Globals.Existing} => {content})...");
                }
                else
                {
                    Console.WriteLine("Did not patch record...");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("[ERROR]: Could not patch record...");
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
