# OldStrategiesForge

**OldStrategiesForge** — это микросервисная платформа для онлайн-игры, разработанной на Unity. Проект включает API, веб-сервис и инфраструктуру для поддержки игрового процесса, включая регистрацию, чаты, матчмейкинг и готовящуюся мультиплеерную логику.

## 🌐 Ссылки

- **Сайт игры:** [https://tacticalheroesdev.ru/](https://tacticalheroesdev.ru/)
- **Swagger API:** [https://tacticalheroesdev.ru/swagger/index.html](https://tacticalheroesdev.ru/swagger/index.html)

## 🌟 Особенности

- **Микросервисная архитектура:** Каждый сервис отвечает за определенную область (авторизация, чаты, матчмейкинг и др.).
- **Авторизация и регистрация:**
  - Подтверждение аккаунта через email.
  - Выдача JWT токенов.
- **Чаты:** Реализация чатов с историей сообщений.
- **Матчмейкинг:** Собственная система подбора игроков.
- **UI-сервис:** Для будущей загрузки клиента игры.
- **Автоматизация:** Docker Compose и GitHub Actions для простого деплоя.
- **Проектирование базы данных:** Хранимые процедуры для эффективного управления данными.
- **Ожидаемые функции:** Проектирование и разработка логики мультиплеера.

## 🚀 Установка и запуск

1. **Клонируйте репозиторий:**
   ```bash
   git clone https://github.com/PANiXiDA/OldStrategiesForge.git
   cd OldStrategiesForge
   ```

2. **Запустите инфраструктуру через Docker Compose:**
   ```bash
   docker-compose up --build
   ```

3. **Доступ к API:**
   Swagger доступен по адресу:
   - [http://localhost:80/swagger/index.html](http://localhost:80/swagger/index.html)

   UI доступен по адресу:
   - [http://localhost:9080](http://localhost:9080)

## 🛠️ Технологии

- **Языки и платформы:** ASP.NET, C#.
- **Базы данных:** PostgreSQL, Redis.
- **Сообщения:** RabbitMQ.
- **DevOps:** Docker Compose, GitHub Actions, AWS.
- **Связь:** gRPC, WebSocket.
- **Устойчивость:** Polly.

## 📜 Лицензия

Этот проект распространяется под лицензией **Apache License 2.0**.  
Полный текст лицензии доступен в файле [LICENSE](LICENSE).

## 📚 Дополнительная информация

Проект находится в активной разработке. Мы будем рады вашим отзывам, предложениям и идеям!

## 📧 Контакты

Если у вас есть вопросы, создайте issue или свяжитесь со мной через [GitHub Issues](https://github.com/PANiXiDA/OldStrategiesForge/issues).
