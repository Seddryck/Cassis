﻿using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Cassis.Testing.Core
{
    class FileOnDisk
    {
        /// <summary>
        /// Copies the contents of input to output. Doesn't close either stream.
        /// </summary>
        public static void CopyStream(Stream input, Stream output)
        {
            byte[] buffer = new byte[8 * 1024];
            int len;
            while ((len = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, len);
            }
        }

        /// <summary>
        /// Create a temporary file based on the embedded file
        /// </summary>
        /// <param name="filename">Name of the file (with extension) to create</param>
        /// <param name="resource">Full path (including namespace) of the resource to copy on hard drive</param>
        /// <returns>The fullpath of the created file</returns>
        public static string CreatePhysicalFile(string filename, string resource)
        {
            //Build the fullpath for the file to read
            var fullpath = GetDirectoryPath() + filename;

            //delete it if already existing
            if (File.Exists(fullpath))
                File.Delete(fullpath);

            // A Stream is needed to read the XLS document.
            using (Stream stream = Assembly.GetExecutingAssembly()
                                           .GetManifestResourceStream(resource))
            {
                //Open another stream to persist the file on disk
                using (Stream file = File.OpenWrite(fullpath))
                {
                    if (file == null)
                        throw new ArgumentException(string.Format("Resource named '{0}' not found", resource));
                    CopyStream(stream, file);
                }
            }
            return fullpath;
        }

        /// <summary>
        /// Returns the Directory into which the test are executed
        /// </summary>
        /// <returns>An existing temporary path to execute the tests</returns>
        public static string GetDirectoryPath()
        {
            //Build the fullpath for the file to read
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\";
        }
    }
}
