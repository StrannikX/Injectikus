using System;
using System.Collections.Generic;
using System.Text;

namespace Injectikus
{
    /// <summary>
    /// Сообщение, уведомляющее о том, что запрошенная зависимость не может быть разрешена контейнером
    /// </summary>
    public class DependencyIsNotResolvableByContainerException : Exception
    {
        /// <summary>
        /// Тип зависимости, которую не удалось разрешить
        /// </summary>
        public Type RequestedType { get; }

        /// <summary>
        /// Создаёт исключение для неразрешенной зависимости типа <paramref name="requestedType"/>
        /// </summary>
        /// <param name="requestedType">Тип зависимости, которую не удалось разрешить</param>
        public DependencyIsNotResolvableByContainerException(Type requestedType)
            : base($"Container can not resolve dependency for type {requestedType.FullName}!")
        {
            RequestedType = requestedType;
        }

        /// <summary>
        /// Создаёт исключение с сообщение <paramref name="message"/> для неразрешенной зависимости типа <paramref name="requestedType"/>
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <param name="requestedType">Тип зависимости, которую не удалось разрешить</param>
        public DependencyIsNotResolvableByContainerException(Type requestedType, string message) : base(message)
        {
            RequestedType = requestedType;
        }
    }
}
