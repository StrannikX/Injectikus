# Injectikus - ещё один контейнер внедрения зависимостей
    
## Базовые примеры использования
```csharp
IContainer container = new BaseContainer();

// Регистрация класса, как реализации интерфейса
container.Bind<I>().To<ClassImplementsI>();

// Есть поддержка фабричных делегатов
container.Bind<I>().ToMethod(container => new ClassImplementsI(container.Get<ClassDependencyType>()));

// И произвольных объектов - поставщиков
class ClassClassImplementsIProvider : ObjectProvider<ClassImplementsI>
{
    public ClassImplementsI Create(IContainer container)
    {
        return new ClassImplementsI(container.Get<ClassDependencyType>());
    }
}
container.Bind<I>().ToProvider(new ClassClassImplementsIProvider());

// Есть возможность использовать объекты-одиночки (Singleton)
container.Bind<I>().Singleton().To<ClassImplementsI>();
container.Bind<I>().Singleton().ToMethod(container => new ClassImplementsI(container.Get<ClassDependencyType>()));
container.Bind<I>().Singleton().ToProvider(new ClassClassImplementsIProvider());

// Также можно указывать конкретные объкеты
// так
container.Bind<I>().Singleton().ToObject(new ClassClassImplementsI());
// или так
container.Bind<I>().Singleton(new ClassClassImplementsI());

// Для указания типа, как реализации самого себя можно использовать метод расширения IContainer.ToThemselves()
container.Bind<ClassImplementsI>().ToThemselves();

// Для получения экземпляра можно использовать методы Get и GetAll
var object = container.Get<I>();
if (container.GetAll<I>(var out obj)) {
    ...
}

// Также можно для одного базового типа зарегистрировать несколько реализаций и получить их массив
container.Bind<I>().To<FirstClassImplementsI>();
container.Bind<I>().To<SecondClassImplementsI>();

I[] objects = container.GetAll<I>();

// С помощью метода CreateInstance<T> можно создать экземпляр класса T, не зарегистрированного в контейнере, с внедрением в него зависимостей из контейнера
var object = container.CreateInstance<SomeClass>();

// Можно проверить, способен ли контейнер разрешить зависимость
if (container.CanResolve<I>())
{
    ...
}

Также у всех вышепреведенных методов присутствуют необобщенные версии, для возможность определять типы в рантайме
container.Bind(typeof(I)).To(typeof());
object @object = container.Get(typeof(I))
```

## Стратегии создания экземпляра класса и внедрения зависимостей
Контейнер поддерживает несколько стратегий создания экземпляра класса и внедрения зависимостей.

+ Внедрение в отмеченный атрибутом конструктор класса
  ```csharp    
  class Car
  {
      [InjectHere]
      public Car(IEngine engine)
      {
          ...
      }
  }
  ```
+ Внедрение зависимостей в помеченные атрибубутом публичные свойства и параметры методов-сеттеров
   ```csharp     
   class Car
   {
       [InjectHere]
       public IEngine Engine { get; set;}

       [InjectHere]
       public void SetTransmission(ITransmission transmission)
       {
           ...
       }
   }
   ```
+ Внедрение зависимости через аргументы одно определенного метода - инициализатора
    ```csharp
  class Car
  {
      [InjectHere]
      public void Init(IEngine engine, ITransmission transmission)
      {
          ...
      }
  }
  ```
+ Внедрение зависимостей через аргументы конструктора с наибольшим количеством разрешимых контейнером аргументов.
  ```csharp
  class Car
  {
      // В зависимости от того, 
      // может ли контейнер в момент создания экземпляра 
      // разрешить типы IEngine и ITransmission, 
      // будет вызван один из трёх конструкторов объекта

      public void Car(IEngine engine, ITransmission transmission)
      {
          ...
      }

      public Car(ITranmission transmission)
      {
          ...
      }

      public Car(IEngine engine)
      {
          ...
      }
  }
  ```

+ Использование конструктора по умолчанию без внедрения зависимостей
  ```csharp
  class DodgeCharger
  {
      public DodgeCharger()
      {
          engine = new V8Engine();
          transmission = new SixSpeedAutomaticTransmission();
      }
  }
  ```

Указать конкретный метод инициализации для класса, можно с помощью атрибута InjectionMethod
```csharp  
[InjectionMethod(DependencyInjectionMethod.ConstructorParametersInjection)]
class Car
{
    [InjectHere]
    public Car(IEngine engine)
    {
        ...
    }
}
```
Если атрибут InjectionMethod не указан, то метод инициализации будет выбран автоматически, в зависимости от того, использованы ли атрибуты библиотеки на каких-либо членах класса и какие конструкторы доступны.

Так же возможно внедрение массива объектов необходимого типа (аналог GetAll). Для этого необходимо использовать аттрибут InjectArray
```csharp 
class Garage
{
    [InjectHere][InjectArray]
    public ICar Cars { get; set; }
}

// Или

class Garage
{
    [InjectHere]
    public Garage([InjectArray] ICar[] cars)
    {
        ...
    }
}
```
Конструкторы, методы инициализации и сеттеры могут иметь опциональные параметры. В таком случае при наличии зависимости в контейнере в них будет передана зависимость, в противном случае - значение по-умолчанию.
```csharp
class Car
{
    public IEngine Engine { get; }
    public ITransmission Transmission { get; }

    [InjectHere]
    public Car(IEngine engine, ITransmission transmission = null)
    {
        Engine = engine;
        Transmission = transmission ?? new MechanicFiveSpeed();
    }
}
```
