// for å compile koden til en exe fil, bruk følgende kommando:
// csc /out:FrontendDevSetup.exe Program.cs

// code is chatGPT generated.

using System;
using System.Diagnostics;

class Program
{
    static void Main()
    {
        // Check if the script is running as root (superuser)
        if (!IsRunningAsRoot())
        {
            Console.WriteLine("This script requires superuser privileges. Please run with 'sudo'.");
            return;
        }

        // Check if Visual Studio Code is already installed
        if (!IsProgramInstalled("code"))
        {
            // Install Visual Studio Code
            ExecuteCommand("curl https://packages.microsoft.com/keys/microsoft.asc | gpg --dearmor -o /usr/share/keyrings/microsoft-archive-keyring.gpg");
            ExecuteCommand("echo \"deb [signed-by=/usr/share/keyrings/microsoft-archive-keyring.gpg] https://packages.microsoft.com/repos/code stable main\" | tee /etc/apt/sources.list.d/vscode.list");
            ExecuteCommand("apt-get update");
            ExecuteCommand("apt-get install code");
        }
        else
        {
            Console.WriteLine("Visual Studio Code is already installed.");
        }

        // Check if Git is already installed
        if (!IsProgramInstalled("git"))
        {
            // Install Git
            ExecuteCommand("apt-get install git");
        }
        else
        {
            Console.WriteLine("Git is already installed.");
        }

        // Check if Node.js is already installed
        if (!IsProgramInstalled("node"))
        {
            // Install Node.js LTS and npm
            ExecuteCommand("curl -fsSL https://deb.nodesource.com/setup_lts.x | bash -");
            ExecuteCommand("apt-get install -y nodejs");
        }
        else
        {
            Console.WriteLine("Node.js is already installed.");
        }

        // Prompt for user name
        Console.Write("Enter your Git user name: ");
        string userName = Console.ReadLine();

        // Prompt for user email
        Console.Write("Enter your Git email address: ");
        string userEmail = Console.ReadLine();

        // Configure Git settings with user input
        ExecuteCommand($"git config --global user.name '{userName}'");
        ExecuteCommand($"git config --global user.email '{userEmail}'");
    }

    static bool IsRunningAsRoot()
    {
        using (Process process = new Process())
        {
            process.StartInfo = new ProcessStartInfo
            {
                FileName = "whoami",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            };

            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return output.Trim() == "root";
        }
    }

    static bool IsProgramInstalled(string programName)
    {
        using (Process process = new Process())
        {
            process.StartInfo = new ProcessStartInfo
            {
                FileName = "which",
                Arguments = programName,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            };

            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return !string.IsNullOrEmpty(output) && output.Trim() != "";
        }
    }

    static void ExecuteCommand(string command)
    {
        Process process = new Process()
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "bash",
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            }
        };

        process.Start();
        process.StandardInput.WriteLine(command);
        process.StandardInput.WriteLine("exit");
        process.WaitForExit();
        process.Close();
    }
}