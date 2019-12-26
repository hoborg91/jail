using System.Collections.Generic;

namespace Jail.Design.FileSystem {
    /// <summary>
    /// Provides common methods for file system. Can be used to
    /// lower the class dependency on the file system and unit testing.
    /// </summary>
    public interface IFileSystemApi {
        /// <summary>
        /// Determines whether the specified file exists.
        /// </summary>
        bool FileExists(string path);

        /// <summary>
        /// Determines whether the given path refers to an existing directory on disk.
        /// </summary>
        bool DirectoryExists(string path);

        /// <summary>
        /// Returns the names of files (including their paths) in the specified directory.
        /// </summary>
        string[] GetFiles(string path);

        /// <summary>
        /// Returns an object representing stream reader for the file
        /// located at the given path. Do not forget to dispose it or 
        /// cover in "using" block.
        /// </summary>
        IStreamReader OpenForRead(string path);

        /// <summary>
        /// Returns an object representing stream writer for the file
        /// located at the given path. Do not forget to dispose it or 
        /// cover in "using" block.
        /// </summary>
        IStreamWriter OpenForWrite(string path);

        /// <summary>
        /// Opens a text file, reads all lines of the file, and then closes the file.
        /// </summary>
        string ReadAllText(string path);

        /// <summary>
        /// Opens a file, reads all lines of the file with the specified encoding, 
        /// and then closes the file.
        /// </summary>
        string ReadAllText(string path, System.Text.Encoding encoding);

        /// <summary>
        /// Creates a new file, writes the specified string to the file, and then closes
        /// the file. If the target file already exists, it is overwritten.
        /// </summary>
        void WriteAllText(string path, string contents);

        /// <summary>
        /// Creates a new file, writes the specified string to the file using the specified
        /// encoding, and then closes the file. If the target file already exists, it is
        ///  overwritten.
        /// </summary>
        void WriteAllText(string path, string contents, System.Text.Encoding encoding);

        /// <summary>
        /// Creates a new file, writes a collection of strings to the file, and then closes
        /// the file.
        /// </summary>
        void WriteAllLines(string path, IEnumerable<string> contents);

        /// <summary>
        /// Creates a new file by using the specified encoding, writes a collection of strings
        /// to the file, and then closes the file.
        /// </summary>
        void WriteAllLines(string path, IEnumerable<string> contents, System.Text.Encoding encoding);

        /// <summary>
        /// Deletes the specified file.
        /// </summary>
        /// <param name="path">The name of the file to be deleted. Wildcard characters are not supported.</param>
        void DeleteFile(string path);

        /// <summary>
        /// Returns the directory information for the specified path string.
        /// </summary>
        /// <param name="path">The path of a file or directory.</param>
        string GetDirectoryName(string path);
    }
}
