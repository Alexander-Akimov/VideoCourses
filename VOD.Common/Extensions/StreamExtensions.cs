using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace VOD.Common.Extensions
{
    public static class StreamExtensions
    {
        public static async Task SerializeToJsonAndWriteAsync<T>(this Stream stream, T objectToWrite, Encoding encoding,
            int bufferSize, bool leaveOpen, bool resetStream = false)
        {

            if (stream == null) throw new ArgumentNullException(nameof(stream));
            if (!stream.CanWrite) throw new ArgumentNullException("Can't write to this stream.");
            if (encoding == null) throw new ArgumentNullException(nameof(encoding));

            using (var streamWriter = new StreamWriter(stream, encoding, bufferSize, leaveOpen))
            using (var jsonTextWriter = new JsonTextWriter(streamWriter))
            {
                var jsonSerialiser = new JsonSerializer();
                jsonSerialiser.Serialize(jsonTextWriter, objectToWrite);
                await jsonTextWriter.FlushAsync();
            }

            if (resetStream && stream.CanSeek)
                stream.Seek(0, SeekOrigin.Begin);
        }
        public static T ReadAndDeserializeFromJson<T>(this Stream stream)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            if (!stream.CanRead) throw new ArgumentNullException("Can't read from this stream.");

            using (var streamReader = new StreamReader(stream, new UTF8Encoding(), detectEncodingFromByteOrderMarks: true, 1024, false))
            using (var jsonTextReader = new JsonTextReader(streamReader))
            {
                var jsonSerialiser = new JsonSerializer();
                return jsonSerialiser.Deserialize<T>(jsonTextReader);
            }
        }
    }
}
