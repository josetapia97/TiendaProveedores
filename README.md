# Furniture Store API 🛋️

Furniture Store API es una aplicación .NET dedicada a gestionar los datos de una tienda de muebles. Aquí, encontrarás una descripción general de las funcionalidades y pasos clave para desarrolladores.

## Funcionalidades Principales

### 1. 🏗️ Arquitectura

- Creación del proyecto `Furniture.shared` para compartir lógica entre distintos proyectos.
- Desarrollo del proyecto `Furniture.Data` como la capa de acceso a datos.
- Integración de paquetes esenciales en `Furniture.Data`, como MEFC 6, MEFC Design 6, MEFC sqlite 6, y MEFC tools 6.
- Configuración del contexto de datos (DbContext) para interactuar con la base de datos usando Entity Framework.

### 2. 🗃️ Base de Datos

- Utilización de Entity Framework y MEFC para crear y gestionar la base de datos.
- Ejecución de migraciones para actualizar y mantener la consistencia de la base de datos.

### 3. 🔐 Seguridad y Autenticación

- Implementación de JWT (JSON Web Token) para autorización en las peticiones.
- Integración de seguridad en la API con Microsoft.AspNetCore.Identity.EntityFrameworkCore.
- Manejo de secretos para garantizar la seguridad de la información sensible.

### 5. 🌐 Microservicios

- Exploración de la arquitectura de microservicios y su aplicación en la API.
- Empleo de JWT para procesar solicitudes en el servidor sin pasos adicionales.

### 6. 📝 Documentación y Debugging

- Uso de herramientas como [jwt.io](https://jwt.io/) para debugear JWT.
- Consideraciones sobre manejo de secretos y configuración de Firebase CLI.

### 7. 🚀 Desarrollo Adicional

- Creación de entidades adicionales y actualización del modelo de datos.
- Desarrollo de CRUD básicos y filtrado avanzado en los controladores de la API.
- Manejo de solicitudes y respuestas en cada clase.

### 8. 🔒 Seguridad Adicional

- Implementación de autenticación y autorización en los controladores.
- Manejo de tokens de acceso y autorización de diferentes endpoints.

### 9. 📧 Integración de Email y Verificación

- Configuración de Gmail como servidor SMTP para el envío de correos electrónicos.
- Integración de MailKit para la manipulación de emails.
- Envío de correos de verificación en el proceso de registro.


### Herramientas Utilizadas

- [Insomnia](https://insomnia.rest/) - Cliente HTTP para probar las API.
- [Postman](https://www.postman.com/) - Plataforma de colaboración para el desarrollo de API.
- [Swagger](https://swagger.io/) - Herramienta para diseñar, construir y documentar API REST.
- [YopMail](http://www.yopmail.com/) - Correo electrónico temporal para pruebas.
- [jwt.io](https://jwt.io/) - Debugging y análisis de tokens JWT.


---

