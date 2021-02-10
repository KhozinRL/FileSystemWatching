using System;
using System.IO;

namespace FileSystemWatch
{
    class Program
    {
        const int spaceLen = 60;

        static string logPath;
        static void Main(string[] args)
        {
            string path;

            try
            {
                path = args[0];
                logPath = args[1];
            }
            catch (IndexOutOfRangeException e)
            {
                Console.WriteLine("Ошибка, введите параметры!\n" + e.Message);
                return;
            }

            Console.WriteLine("Адрес директории: {0}", path);
            Console.WriteLine("Адрес log файла: {0}", logPath);

            string space1 = new string(' ', DateTime.Now.ToString().Length - 11);
            string space2 = new string(' ', spaceLen);

            using (StreamWriter sw = new StreamWriter(logPath)) {
                sw.WriteLine("Date, Time:" + space1 + "\tName:" + space2 + "\tChange Type: ");
            }

            using (FileSystemWatcher watcher = new FileSystemWatcher(path, "*.cs"))
            {
                watcher.Changed += OnChanged;
                watcher.Created += OnChanged;
                watcher.Deleted += OnChanged;
                watcher.Renamed += OnRenamed;
                watcher.EnableRaisingEvents = true;
                Console.ReadKey();
            }
        }

        private static void OnRenamed(object sender, RenamedEventArgs e)
        {
            using (StreamWriter sw = new StreamWriter(logPath, true))
            {
                string name = new string(e.OldName + " -> " + e.Name);
                string space = new string(' ', spaceLen - name.Length + 5);
                sw.WriteLine(DateTime.Now + "\t" + name + space + "\t" + e.ChangeType);
            }
        }

        private static void OnChanged(object sender, FileSystemEventArgs e)
        {
            using (StreamWriter sw = new StreamWriter(logPath, true))
            {
                string space = new string(' ', spaceLen - e.Name.ToString().Length + 5);
                sw.WriteLine(DateTime.Now + "\t" + e.Name + space + "\t" + e.ChangeType);
            }
        }
    }
}
