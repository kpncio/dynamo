#include "Dynamer.h"

#include <curlpp/cURLpp.hpp>
#include <curlpp/Options.hpp>

using namespace std;

void setConfig(string config);
void getConfig(string config);

int main()
{
    string config = "dynamer.conf";

    if (!filesystem::exists(config)) {
        setConfig(config);
    }
    else {
        getConfig(config);
    }

	cout << "Hello CMake." << endl;

	return 0;
}

void setConfig(string config) {
    vector<string> defaultConfig = {
        "# This is the default coniguration of KPNC Dynamer",
        "# Information: https://github.com/kpncio/dynamer",
        "",
        "###########",
        "# DYNAMER #",
        "###########",
        "",
        "# Polling rate for Dynamer in seconds.",
        "# A rate of 0 will only check on startup (useful for AWS EC2).",
        "rate=0",
        "",
        "# Whether or not to use the IPv6 address (A vs AAAA).",
        "ipv6=false",
        "",
        "##############",
        "# CLOUDFLARE #",
        "##############",
        "",
        "# Cloudflare account email address.",
        "email=someone@example.com",
        "",
        "# Cloudflare API key (needs access to zone/dns records).",
        "apikey=abcdef0123456789",
        "",
        "# The zone ID of the records you want to update.",
        "zone=abcdef0123456789",
        "",
        "# The account ID of the records you want to update.",
        "record=abcdef0123456789",
        "",
        "# The domain you want to update (ie. kpnc.io, app.kpnc.io).",
        "# You may even use @ to specify the apex.",
        "domain=@",
        "",
        "# Whether or not to proxy the domain through Cloudflare.",
        "proxied=true",
        "",
        "# Time To Live of the domain in seconds (1 specifies auto).",
        "ttl=1"
    };

    ofstream outfile(config);

    for (string str : defaultConfig) {
        outfile << str << endl;
    }

    outfile.close();
}

void getConfig(string config) {
    ifstream cFile(config);

    if (cFile.is_open()) {
        string line;

        while (getline(cFile, line)) {
            regex reg(" ");
            
            line = regex_replace(line, reg, "");

            if (line[0] == '#' || line.empty()) {
                continue;
            }

            auto delimiter = line.find("=");
            auto key = line.substr(0, delimiter);
            auto value = line.substr(delimiter + 1);

            cout << "Set Setting: " << key << ", " << value << endl;
        }
    }
    else {
        cerr << "Error: No config file found..." << endl;
    }
}
