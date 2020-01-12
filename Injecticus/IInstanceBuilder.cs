namespace Injectikus
{
    /// <summary>
    /// Построитель экземпляров. Скрывает конкретную стратегию построения экземпляра.
    /// </summary>
    internal interface IInstanceBuilder
    {
        /// <summary>
        /// Создать экземпляр
        /// </summary>
        /// <param name="container">Контейнер, в котором создаётся экземпляр</param>
        /// <returns>Экземпляр класса, с внедрёнными в него зависимостями</returns>
        object BuildInstance(IContainer container);
    }
}
