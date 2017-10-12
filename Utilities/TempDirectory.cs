using System;
using System.IO;

namespace Structura.Shared.Utilities
{
    public class TempDirectory : IDisposable
    {
        private readonly bool _autoDelete;
        readonly string _fullName;

        public TempDirectory(bool autoDelete = true) : this(Path.Combine(Path.GetTempPath(), Path.GetRandomFileName()))
        {
            _autoDelete = autoDelete;
        }

        public TempDirectory(string directory)
        {
            _fullName = directory?.ToAbsolutePath() ?? throw new ArgumentNullException(nameof(directory));
            Directory.CreateDirectory(_fullName);
        }

        public void Dispose()
        {
            if (_autoDelete) TryDelete(_fullName);
        }

        public string FullName => _fullName;

        void TryDelete(string path)
        {
            try
            {
                Directory.Delete(path, true);
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch
            {
                // at least we tried!
            }
        }
    }
}