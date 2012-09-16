using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace SublimeLogger
{
    public class LoggerImpl : Logger
    {
        private int ErrorCount;
        private int WarningCount;

        public override void Initialize(IEventSource eventSource)
        {
			Console.ForegroundColor = ConsoleColor.White;
			Console.WriteLine("Sublime Text 2 output logger by Jacob Pennock");

            eventSource.ProjectStarted += ProjectStarted;
            eventSource.BuildFinished += BuildFinished;
            eventSource.ErrorRaised += ErrorRaised;
            eventSource.WarningRaised += WarningRaised;
            ErrorCount = 0;
            WarningCount = 0;
        }

        void ProjectStarted(object sender, ProjectStartedEventArgs e)
        {
			// Console.WriteLine("Project Started: " + e.ProjectFile);
        }

        void BuildFinished(object sender, BuildFinishedEventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.White;
            if (e.Succeeded)
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("Compilation SUCCEEDED! Errors:{0} Warnings:{1}",ErrorCount,WarningCount);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Compilation FAILED! Errors:{0} Warnings:{1}",ErrorCount,WarningCount);
            }
            Console.BackgroundColor = ConsoleColor.Black;
        }

        void ErrorRaised(object sender, BuildErrorEventArgs e)
        {
            string fullPath = e.File;
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("{0}({1},{2})  error:{3}  {4}", fullPath, e.LineNumber, e.ColumnNumber, e.Code, e.Message);
            Console.WriteLine();
            ErrorCount++;
            Console.ForegroundColor = ConsoleColor.White;
        }

        void WarningRaised(object sender, BuildWarningEventArgs e)
        {
            string fullPath = e.File.ToString();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            if(e.Code != null)
            {
                Console.WriteLine("{0}({1},{2})  warning:{3}  {4}", fullPath, e.LineNumber, e.ColumnNumber, e.Code, e.Message);
                Console.WriteLine();
                WarningCount++;
            }
            Console.ForegroundColor = ConsoleColor.White;
        }

    }
}
