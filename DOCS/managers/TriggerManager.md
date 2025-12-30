
[Назад](managers.md)
# TriggerManager

Менеджер триггеров, позволяет делать развилки в сюжетах или ограничивать игровые действия, для сравнения могу привести ПОРШНИ из трилогии сталкера, либо QUEST FACT из CyberPunk 2077

TriggerManager это синглтон, т.е вы можете обращаться к нему из любого скрипта просто написав: 
```cs
TriggerManager.Instance
```

### SetState
```cs
TriggerManager.Instance.AddTrigger(string triggerName, bool initialState = false)
```
Добавляет триггер в проект, и позволяет установить его значение по умолчанию (true/false)

### SetTriggerState
```cs
TriggerManager.Instance.SetTriggerState(string triggerName, bool state)
```
Устанавливает значение уже существующему триггеру

## GetTriggerState
```cs
TriggerManager.Instance.GetTriggerState(string triggerName) // return bool
```
Возвращает значение триггера по названию

## GetAllTriggers
```cs
TriggerManager.Instance.GetAllTriggers() // return List<string>
```
Возвращает список ВСЕХ существующих триггеров

# TriggerZone (Script)

Скрипт базирующийся  на OnTriggerEnter (т.е для работы создаете колайдер который будет `isTrigger`)
Позволяет настраивать стокновение с "зоной" которая сработает от определенных условий