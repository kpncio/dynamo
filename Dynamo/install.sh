echo "Updating Package Manager..."
sudo apt update
sudo apt upgrade -y

echo "Installing .NET Dependencies..."
sudo apt install -y dotnet-sdk-7.0
sudo apt install -y dotnet-runtime-7.0

echo "Installing Dynamo as a Service..."
sudo cp dynamo.service /etc/systemd/system/dynamo.service
sudo systemctl daemon-reload
sudo systemctl enable dynamo
sudo systemctl start dynamo

echo "Installation Completed..."