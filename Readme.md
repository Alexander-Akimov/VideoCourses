TODO:
1) Лучше вынести сервисы в отдельную dll.
Весь маппинг с использованием AutoMapper производить в  одной сборке с сервисами.
Вынести весь маппинг из контроллеров в сервисы.
2) Разделить логику на репозитории и сервисы
	В репозитории вынести всё взаимодействие с БД.
	В сервисы всю бизнес логику.
3) Понять всю логику обновления сущностей.
4) Optional: Создать базовый класс для всех сущностей(Entities).
Это позволит избавится от таких выражений как это: `(int)entity.GetType().GetProperty("Id").GetValue(entity);`

 _dbWriteService.Add(entity);
 await _dbWriteService.SaveChangesAsync();


UserService.AddUserAsync()
	Although not strictly necessary in this scenario, it could be vital
	if you choose to implement email verification later.

UserService.UpdateUserAsync()
	You could add more checks to see that the email is a valid email address,
	but I leave that as an extra exercise for you to solve on your own.