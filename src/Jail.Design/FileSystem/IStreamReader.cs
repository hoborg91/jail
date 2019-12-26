using System;

namespace Jail.Design.FileSystem {
    /// <summary>
    /// Provides the interface similar to the StreamReader functionality.
    /// </summary>
    public interface IStreamReader : IDisposable {
        /// <summary>
        /// Reads a line of characters from the current stream and returns the data as a string.
        /// </summary>
        string ReadLine();

        /// <summary>
        /// Reads all characters from the current position to the end of the stream.
        /// </summary>
        string ReadToEnd();
    }
}
