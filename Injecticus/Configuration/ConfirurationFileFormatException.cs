using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Injectikus.Configuration
{
    /// <summary>
    /// Исключение некорректного формата XML-файла конфигурации контейнера
    /// </summary>
    public class ConfirurationFileFormatException : Exception
    {
        public ConfirurationFileFormatException(string? message) : base(message)
        {
        }
    }
}
