echo "Updating Package Manager..."
sudo apt update
sudo apt upgrade -y

echo "Installing .NET Dependencies..."
sudo apt install -y dotnet-sdk-7.0
sudo apt install -y dotnet-runtime-7.0

echo "Installing Dynamer as a Service..."
sudo cp conducter.service /etc/systemd/system/conducter.service
sudo systemctl daemon-reload
sudo systemctl enable conducter
sudo systemctl start conducter

echo "Installation Completed..."