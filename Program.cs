using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Searchcsgofiles
{
	public static class Global
	{
		public static List<string> Locations = new List<string>();
		private static int threadCount = 0;
		public static Stopwatch Watch = new Stopwatch();
		public static int ThreadCount
		{
			get => threadCount;
			set
			{
				threadCount = value;
				if(threadCount == 0)
				{
					Watch.Stop();
					Console.WriteLine();
					Console.WriteLine($"Finished scan in: {Watch.Elapsed.TotalSeconds} seconds");
					Console.WriteLine();

					foreach (var line in Locations)
					{
						Console.WriteLine(line);
					}
				}
			}
		}
	}

	class Program
	{
		static string Search;
		public static void Main(string[] args)
		{
			string searchPath = @"C:\Program Files (x86)\Steam\steamapps\common\Counter-Strike Global Offensive\csgo";
			if(args.Length == 0)
			{
				Console.WriteLine(@"Use different path?: C:\Program Files (x86)\Steam\steamapps\common\Counter-Strike Global Offensive\csgo");
				Console.WriteLine("Y/N?");
				if(Console.ReadLine().ToLower() == "y")
				{
					Console.WriteLine("Enter Path: ");
					searchPath = Console.ReadLine();
				}
			}
			else
			{
				searchPath = args[0];
			}

			Console.WriteLine("Enter search phrase:");
			Search = Console.ReadLine();

			Global.Watch.Start();
			var files = Directory.EnumerateFiles(searchPath);
			foreach(var path in files)
			{
				var thread = new Thread(() => SearchFile(path));
				thread.Start();
				Global.ThreadCount++;
			}
		}

		public static void SearchFile(string path)
		{
			Console.WriteLine($"reading : {Path.GetFileName(path)}");
			var lines = File.ReadLines(path);
			var i = 1;
			
			foreach (var line in lines)
			{
				if (line.Contains(Search))
				{
					Global.Locations.Add($"[{path}::{i}]");
					Console.WriteLine($"[{path}::{i}]");
				}
				i++;
			}
			Console.WriteLine($"finished: {Path.GetFileName(path)}");
			Global.ThreadCount--;
		}
	}
}
