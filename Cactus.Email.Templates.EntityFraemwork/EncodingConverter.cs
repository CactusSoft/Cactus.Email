using System;
using System.Text;
using Cactus.Email.Templates.EntityFraemwork.Database;
using Cactus.Email.Templates.EntityFraemwork.Logging;
using Cactus.Email.Templates.EntityFraemwork.Managers;

namespace Cactus.Email.Templates.EntityFraemwork
{
    public static class EncodingConverter
    {
        private static readonly ILog Logger = LogProvider.GetLogger(typeof(TemplatesManager));

        public static Encoding CastToEncoding(EncodingType encodingType)
        {
            switch (encodingType)
            {
                case EncodingType.Default: return Encoding.Default;
                case EncodingType.Unicode: return Encoding.Unicode;
                case EncodingType.UTF8: return Encoding.UTF8;
                case EncodingType.ASCII: return Encoding.ASCII;
                case EncodingType.UTF7: return Encoding.UTF7;
                case EncodingType.UTF32: return Encoding.UTF32;
                case EncodingType.BigEndianUnicode: return Encoding.BigEndianUnicode;
                default:
                    var errorMessage = "Couldn't defined type of encoding";
                    Logger.Error(errorMessage);
                    throw new ArgumentOutOfRangeException(nameof(encodingType), errorMessage);
            }
        }

        public static EncodingType CastToEncodingType(Encoding encoding)
        {
            if (encoding == Encoding.Default) return EncodingType.Default;
            if (encoding == Encoding.Unicode) return EncodingType.Unicode;
            if (encoding == Encoding.UTF8) return EncodingType.UTF8;
            if (encoding == Encoding.ASCII) return EncodingType.ASCII;
            if (encoding == Encoding.UTF7) return EncodingType.UTF7;
            if (encoding == Encoding.UTF32) return EncodingType.UTF32;
            if (encoding == Encoding.BigEndianUnicode) return EncodingType.BigEndianUnicode;

            var errorMessage = "Couldn't defined type of encoding";
            Logger.Error(errorMessage);
            throw new ArgumentOutOfRangeException(nameof(encoding), errorMessage);
        }
    }
}
