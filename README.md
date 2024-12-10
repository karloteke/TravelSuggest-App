# Travelsuggest API 🌍

¡Bienvenido a **Travelsuggest API**! Este proyecto está construido con **C# y .NET 8**, utilizando una arquitectura RESTful para garantizar una comunicación eficiente entre cliente y servidor. La API maneja solicitudes, procesa datos y asegura que la lógica del negocio sea sólida.

---

## ⚙️ Stack Tecnológico
- Backend: C# y .NET 8, siguiendo una arquitectura en capas para mantener una separación clara de responsabilidades.
- Frontend: TravelSuggest Web, construido con Vue.js. [TravelSuggest-Vue](https://github.com/karloteke/TravelSuggest-Vue)
- Base de Datos: SQL Server con Entity Framework Core como ORM.
- Autenticación: Implementación de JWT para control de accesos seguro.
- Docker: Proyecto dockerizado con Docker Compose para levantar la API, frontend y base de datos con un solo comando.
---
## 📦 Configuración del proyecto

**Clona este repositorio**:
   ```sh
   git clone https://github.com/tu-usuario/travelsuggest.git
   cd travelsuggest
   ```
---
 ### Configura la base de datos: Asegúrate de que la conexión a la base de datos esté configurada en el archivo appsettings.json
---
## 🚀 Arrancar el proyecto
### Opción 1: Iniciar con Docker
**Intrucción creación Docker-compose**
  ```sh
  docker-compose up --build --force-recreate -d
  ```
**Instrucción borrar Docker-compose**
  ```sh
  docker compose down --rmi all --volumes
  ```
---
### Opción 2: Iniciar manualmente 
Si prefieres no usar Docker, puedes iniciar el proyecto manualmente desde tu IDE (por ejemplo, Visual Studio):

- Abre el proyecto en Visual Studio.
- Configura el proyecto
- Haz clic en el botón Run o presiona F5 para ejecutar la API.
El servidor estará disponible en http://localhost:7193/swagger.
---
## 🔗 Colección de Postman
Para probar los endpoints de la API, utiliza la colección de Postman disponible en el siguiente enlace:
[TravelSuggest Postman Collection](https://warped-comet-799750.postman.co/workspace/TravelSuggest-WorkSpace~13045a2a-bca3-4119-80ce-f709ba45cf2a/request/34539488-814aeab4-03e9-4307-885c-48c7ef4fbf87?action=share&creator=34539488&ctx=documentation&active-environment=34539488-cd297461-9a91-48e1-bce6-b7f2f1c08a4b)


