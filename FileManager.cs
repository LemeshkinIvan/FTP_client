using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ftpExample
{
    static class FileManager
    {
        public static Dictionary<string, string> ShowWhatHaveThisDirectory(string path)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();

            var directories = Directory.GetDirectories(path);
            var files = Directory.GetFiles(path);
            
            if(path.Length != 3)
            {
                data.Add(path, "Назад");
            }
           
            foreach (var d in directories)
            {
                data.Add(d, "папка");
            }

            foreach (var f in files)
            {
                data.Add(f, "файл");
            }

            return data;
        }

        public static void createFile(string path)
        {
            File.Create(path);
        }

        public static void createFolder(string path)
        {
             Directory.CreateDirectory(path);
        }

        static string getExtension(string name)
        {
            FileInfo fi = new FileInfo(name);
            string type = fi.Extension;
            return type;
        }
        static public void deleteFile(string filePath)
        {
            if (getExtension(filePath) != null)
            {
                File.Delete(filePath);
            }
            else
            {
                Directory.Delete(filePath);
            }
        }
        public static void renameFile(string currentPath, string old_name, string name)
        {
            FileAttributes attr = File.GetAttributes(currentPath + old_name);

            string oldPath = currentPath + old_name;
            string new_name = name + getExtension(old_name);

            if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
            {
                Directory.Move(oldPath, new_name);
            }
            else
            {
                File.Move(oldPath, new_name);
            }     
        }

        public static void copyFile(string currentPath, string old_name, string name, bool re_recording)
        {
            FileAttributes attr = File.GetAttributes(currentPath + old_name);

            string oldPath = currentPath + old_name;
            string new_name = name + getExtension(old_name);

            if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
            {
                //(oldPath, new_name);
            }
            else
            {
                File.Copy(oldPath, new_name, re_recording);               
            }      
        }

        public static void moveFile()
        {

        }
    }
}