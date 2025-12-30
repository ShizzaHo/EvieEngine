
[Назад](managers.md)
# MessageManager

Пользовались Scratch? Так вот, этот компонент копирует функционал сообщений из скретча.
Вы можете отправлять сообщения из любого объекта вашей игры, и подписывать другие объекты на получение этого сообщения, все крайне просто и практично.
Но не злоупотребляйте этим, все-же такой метод обмена чреват тем что связанность проекта становится настолько большой что вы тупо потом не разберетесь где, что, и куда отправляется 

MessageManager это синглтон, т.е вы можете обращаться к нему из любого скрипта просто написав: 
```cs
MessageManager.Instance
```

### Subscribe
```cs
MessageManager.Instance.Subscribe(string message, Action listener);
```
Создает подписку на сообщение и в случае отправки выполняет `callback` метод

## Subscribe\<T\>
```cs
MessageManager.Instance.Subscribe(string message, Action<T> listener);
```
Перегрузка обычного Subscribe но дает возможность передать какой либо аргумент с выбранным вами типом

### Unsubscribe
```cs
MessageManager.Instance.Unsubscribe(string message, Action listener);
```
Отписывает метод от сообщения, указываете в точности как было в подписке, сообщение и его `callback`

## Unsubscribe\<T\>
```cs
MessageManager.Instance.Unsubscribe<T>(string message, Action<T> listener);
```
Тоже самое что и Unsubscribe но для подписки с аргументами

## SendMessage
```cs
MessageManager.Instance.SendMessage(string message);
```
Отправляет сообщение подписчикам

## SendMessage\<T\>
```cs
MessageManager.Instance.SendMessage(string message, T arg);
```
Отправляет сообщение подписчикам с аргументом