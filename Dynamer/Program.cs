using Dynamer;

Console.WriteLine("  /////////////////////////////////");
Console.WriteLine(" /// Welcome, to KPNC Dynamer! ///");
Console.WriteLine("/////////////////////////////////");
Console.WriteLine("");

bool arguments = args.Length > 0;

try
{
    Globals.Email = args[0];
    Globals.ApiKey = args[1];
    Globals.Domain = args[2];
    Globals.IPv4 = Convert.ToBoolean(args[3]);
    Globals.Rate = Convert.ToInt32(args[4]);
}
catch
{
    Console.WriteLine("[ERROR]: Invalid argument array, check the project...");
    Console.WriteLine("[INFO]: Args: <Email(string)> <ApiKey(string)> <Domain(string)> <IPv4(boolean)> <Rate(integer)>...");
}

if (File.Exists("Config.json"))
{
    Json.Read();
}

if (Globals.Email == null)
{
    Console.WriteLine("[ERROR]: There is no value for Email...");

    if (arguments)
        Environment.Exit(1);
    else
        Console.Write("What is your Cloudflare email? ");
        Globals.Email = Console.ReadLine();
}

if (Globals.ApiKey == null)
{
    Console.WriteLine("[ERROR]: There is no value for ApiKey...");

    if (arguments)
        Environment.Exit(1);
    else
        Console.Write("What is your Cloudflare API Key? ");
        Globals.ApiKey = Console.ReadLine();
}

if (Globals.Domain == null)
{
    Console.WriteLine("[ERROR]: There is no value for Domain...");

    if (arguments)
        Environment.Exit(1);
    else
        Console.Write("What is your domain? (i.e. www.example.com) ");
        Globals.Domain = Console.ReadLine();
}

if (Globals.IPv4 == null)
{
    Console.WriteLine("[ERROR]: There is no value for IPv4...");

    if (arguments)
        Environment.Exit(1);
    else
        Console.Write("Do you want to force IPv4? (y/n) ");
        Globals.IPv4 = Console.ReadLine() == "n" ? false : true;
}

if (Globals.Rate == null)
{
    Console.WriteLine("[ERROR]: There is no value for Rate...");

    if (arguments)
        Environment.Exit(1);
    else
        Console.Write("How much time during each cycle? (seconds) ");
        Globals.Rate = Convert.ToInt32(Console.ReadLine());
}

if (!File.Exists("Config.json"))
{
    Json.Write();
    Json.Read();
}

while (true)
{
    Worker.Process();

    Thread.Sleep((int)Globals.Rate! * 1000);
}
