using System;

namespace Jail.Design.FileSystem {
    /// <summary>
    /// Provides the interface similar to the StreamWriter functionality.
    /// </summary>
    public interface IStreamWriter : IDisposable {
        /// <summary>
        /// Writes a string to a stream.
        /// </summary>
        /// <param name="value">The string to write to the stream. If value is null, nothing is written.</param>
        void Write(string value);

        /// <summary>
        /// Writes a string followed by a line terminator to the text string or stream.
        /// </summary>
        /// <param name="value">The string to write. If value is null, only the line terminator is written.</param>
        void WriteLine(string value);
    }
}
