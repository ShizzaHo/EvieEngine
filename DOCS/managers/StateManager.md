
[Назад](managers.md)
# StateManager

Менеджер игровых состояний вашего проекта, для аналогии просто напишу так: `Игра`, `Пауза`, `КатСцена`

StateManager это синглтон, т.е вы можете обращаться к нему из любого скрипта просто написав: 
```cs
StateManager.Instance
```


[!!!] При запуске проекта менеджер автоматически устанавливает первое состояние из указанных в инспекторе [!!!]

### SetState
```cs
StateManager.Instance.SetState(string newState);
```
Устанавливает игровое состояние 

## GetCurrentState
```cs
StateManager.Instance.GetCurrentState(); //return string
```
Возвращает текущее состояние

## IsCurrentState
```cs
StateManager.Instance.IsCurrentState(string state); //return bool
```
Сравнивает переданное в аргументе состояние с текущим установленным

## AddState
```cs
StateManager.Instance.AddState(string newState)
```
Создает новое состояние (не устанавливает его после создания)