using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PrintService.Utility
{
    public class FileHelper
    {
        #region invalid char in file name
        private static readonly char[] InvalidFileNameChars = new[]
        {
        '"',

        '<',

        '>',

        '|',

        '\0',

        '\u0001',

        '\u0002',

        '\u0003',

        '\u0004',

        '\u0005',

        '\u0006',

        '\a',

        '\b',

        '\t',

        '\n',

        '\v',

        '\f',

        '\r',

        '\u000e',

        '\u000f',

        '\u0010',

        '\u0011',

        '\u0012',

        '\u0013',

        '\u0014',

        '\u0015',

        '\u0016',

        '\u0017',

        '\u0018',

        '\u0019',

        '\u001a',

        '\u001b',

        '\u001c',

        '\u001d',

        '\u001e',

        '\u001f',

        ':',

        '*',

        '?',

        '\\',

        '/'
        };
        #endregion

        /// <summary>
        /// Remove invalid char when try get a file name
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string CleanInvalidFileName(string fileName)
        {
            fileName = fileName + "";

            fileName = InvalidFileNameChars.Aggregate(fileName, (current, c) => current.Replace(c + "", ""));
            if (fileName.Length > 1)

                if (fileName[0] == '.')
                    fileName = "dot" + fileName.TrimStart('.');

            return fileName;

        }

        /// <summary>
        /// Create folder with giving path if it doesn't existed
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public static string CreateDir(string directory)
        {
            try
            {
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
            }
            catch (DirectoryNotFoundException)
            {
                throw;
            }
            return directory;
        }

        /// <summary>
        /// Delete all files in sepcified folder
        /// </summary>
        /// <param name="dir"></param>
        public static void DeleteFilesInDir(string dir)
        {
            if (Directory.Exists(dir))
            {
                foreach (string fileName in Directory.GetFiles(dir))
                {
                    File.Delete(fileName);
                }
            }
        }

    }
}
