using System;
using System.Collections.Generic;
using System.IO;

namespace Jail.Design.FileSystem {
    /// <summary>
    /// Default implementation of the IFileSystemApi. Uses standard
    /// System.IO ecosystem.
    /// </summary>
    public class FileSystemApi : IFileSystemApi {
        /// <summary>
        /// Determines whether the given path refers to an existing directory on disk.
        /// </summary>
        public bool DirectoryExists(string path) {
            return Directory.Exists(path);
        }

        /// <summary>
        /// Determines whether the specified file exists.
        /// </summary>
        public bool FileExists(string path) {
            return File.Exists(path);
        }

        /// <summary>
        /// Returns the names of files (including their paths) in the specified directory.
        /// </summary>
        public string[] GetFiles(string path) {
            return Directory.GetFiles(path);
        }

        /// <summary>
        /// Deletes the specified file.
        /// </summary>
        /// <param name="path">The name of the file to be deleted. Wildcard characters are not supported.</param>
        public void DeleteFile(string path) {
            File.Delete(path);
        }

        /// <summary>
        /// Returns the directory information for the specified path string.
        /// </summary>
        /// <param name="path">The path of a file or directory.</param>
        public string GetDirectoryName(string path) {
            return Path.GetDirectoryName(path);
        }

        /// <summary>
        /// Returns an object representing stream reader for the file
        /// located at the given path. Do not forget to dispose it or 
        /// cover in "using" block.
        /// </summary>
        public IStreamReader OpenForRead(string path) {
            return new StreamReaderProxy(path);
        }

        /// <summary>
        /// Returns an object representing stream writer for the file
        /// located at the given path. Do not forget to dispose it or 
        /// cover in "using" block.
        /// </summary>
        public IStreamWriter OpenForWrite(string path) {
            return new StreamWriterProxy(path);
        }

        /// <summary>
        /// Opens a text file, reads all lines of the file, and then closes the file.
        /// </summary>
        public string ReadAllText(string path) {
            return File.ReadAllText(path);
        }

        /// <summary>
        /// Opens a file, reads all lines of the file with the specified encoding, 
        /// and then closes the file.
        /// </summary>
        public string ReadAllText(string path, System.Text.Encoding encoding) {
            return File.ReadAllText(path, encoding);
        }

        /// <summary>
        /// Creates a new file, writes the specified string to the file, and then closes
        /// the file. If the target file already exists, it is overwritten.
        /// </summary>
        public void WriteAllText(string path, string contents) {
            File.WriteAllText(path, contents);
        }

        /// <summary>
        /// Creates a new file, writes the specified string to the file using the specified
        /// encoding, and then closes the file. If the target file already exists, it is
        ///  overwritten.
        /// </summary>
        public void WriteAllText(string path, string contents, System.Text.Encoding encoding) {
            File.WriteAllText(path, contents, encoding);
        }

        /// <summary>
        /// Creates a new file, writes a collection of strings to the file, and then closes
        /// the file.
        /// </summary>
        public void WriteAllLines(string path, IEnumerable<string> contents) {
            File.WriteAllLines(path, contents);
        }

        /// <summary>
        /// Creates a new file by using the specified encoding, writes a collection of strings
        /// to the file, and then closes the file.
        /// </summary>
        public void WriteAllLines(string path, IEnumerable<string> contents, System.Text.Encoding encoding) {
            File.WriteAllLines(path, contents, encoding);
        }

        /// <summary>
        /// Proxy for the StreamReader. This class is necessary to
        /// implement the IStreamReader (since StreamReader itself
        /// does not implement it).
        /// </summary>
        private class StreamReaderProxy : IStreamReader {
            private readonly StreamReader _reader;
            private bool _isDisposed = false;
            private readonly object _chest = new object();

            public StreamReaderProxy(string path) {
                this._reader = new StreamReader(path);
            }

            public void Dispose() {
                lock (this._chest) {
                    // Optimistic suggestion: the method is called
                    // reare (only once), thus performance benefit
                    // of double check is little. If this suggestion
                    // is considered to be wrong, then make a copy
                    // of the following check before the lock.
                    if (this._isDisposed)
                        return;
                    this._isDisposed = true;
                    this._reader.Dispose();
                }
            }

            /// <summary>
            /// Reads a line of characters from the current stream and returns the data as a string.
            /// </summary>
            public string ReadLine() {
                lock (this._chest) {
                    // Optimistic suggestion: calls to this method
                    // in a disposed state of the object are rare, 
                    // thus performance benefit of double check 
                    // is little. If this suggestion is considered
                    // to be wrong, then make a copy of the 
                    // following check before the lock.
                    if (this._isDisposed)
                        throw new ObjectDisposedException(nameof(StreamReaderProxy));
                    return this._reader.ReadLine();
                }
            }

            /// <summary>
            /// Reads all characters from the current position to the end of the stream.
            /// </summary>
            public string ReadToEnd() {
                lock (this._chest) {
                    // Optimistic suggestion: calls to this method
                    // in a disposed state of the object are rare, 
                    // thus performance benefit of double check 
                    // is little. If this suggestion is considered
                    // to be wrong, then make a copy of the 
                    // following check before the lock.
                    if (this._isDisposed)
                        throw new ObjectDisposedException(nameof(StreamReaderProxy));
                    return this._reader.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// Proxy for the StreamWriter. This class is necessary to
        /// implement the IStreamWriter (since StreamWriter itself
        /// does not implement it).
        /// </summary>
        private class StreamWriterProxy : IStreamWriter {
            private readonly StreamWriter _writer;
            private bool _isDisposed = false;
            private readonly object _chest = new object();

            public StreamWriterProxy(string path) {
                this._writer = new StreamWriter(path);
            }

            public void Dispose() {
                lock (this._chest) {
                    // Optimistic suggestion: the method is called
                    // reare (only once), thus performance benefit
                    // of double check is little. If this suggestion
                    // is considered to be wrong, then make a copy
                    // of the following check before the lock.
                    if (this._isDisposed)
                        return;
                    this._isDisposed = true;
                    this._writer.Dispose();
                }
            }

            /// <summary>
            /// Writes a string to a stream.
            /// </summary>
            /// <param name="value">The string to write to the stream. If value is null, nothing is written.</param>
            public void Write(string value) {
                lock (this._chest) {
                    // Optimistic suggestion: calls to this method
                    // in a disposed state of the object are rare, 
                    // thus performance benefit of double check 
                    // is little. If this suggestion is considered
                    // to be wrong, then make a copy of the 
                    // following check before the lock.
                    if (this._isDisposed)
                        throw new ObjectDisposedException(nameof(StreamReaderProxy));
                    this._writer.Write(value);
                }
            }

            /// <summary>
            /// Writes a string followed by a line terminator to the text string or stream.
            /// </summary>
            /// <param name="value">The string to write. If value is null, only the line terminator is written.</param>
            public void WriteLine(string value) {
                lock (this._chest) {
                    // Optimistic suggestion: calls to this method
                    // in a disposed state of the object are rare, 
                    // thus performance benefit of double check 
                    // is little. If this suggestion is considered
                    // to be wrong, then make a copy of the 
                    // following check before the lock.
                    if (this._isDisposed)
                        throw new ObjectDisposedException(nameof(StreamReaderProxy));
                    this._writer.WriteLine(value);
                }
            }
        }
    }
}
