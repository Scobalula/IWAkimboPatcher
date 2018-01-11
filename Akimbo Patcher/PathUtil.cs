using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhilUtil
{
    public class PathUtil
    {
        public static void CreateFilePath(string filePath)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
        }
    }
}
