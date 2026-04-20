¡Bienvenido! Este es un sistema de gestión de vuelos y reservas desarrollado en **.NET 10** con **MariaDB**, diseñado para ser robusto, escalable y fácil de administrar.

---

## 🛠️ Tecnologías Utilizadas

* **Lenguaje:** C# (.NET 10)
* **ORM:** Entity Framework Core (MySQL/MariaDB Provider)
* **Base de Datos:** MariaDB (vía Podman)
* **Entorno de Desarrollo:** Linux (Bazzite / Fedora)
* **Arquitectura:** Separación por capas (Data, Models, Services, UI)

---

## 🏗️ Estructura del Proyecto

El proyecto está organizado de la siguiente manera para mantener un código limpio y mantenible:

* **`/Data`**: Contexto de la base de datos y configuración de Entity Framework.
* **`/Models`**: Definición de las entidades (Aerolíneas, Aeropuertos, Vuelos, Clientes, Reservas).
* **`/Services`**: Lógica de negocio y manipulación de datos.
* **`/UI`**: Menús interactivos para la consola.
* **`/Migrations`**: Historial de versiones de la base de datos.

---

## 🚀 Configuración del Entorno (Bazzite/Linux)

Para correr este proyecto en un entorno basado en contenedores, sigue estos pasos:

### 1. Levantar la Base de Datos
Utilizamos **Podman** para mantener la base de datos aislada y segura:

```bash
podman run -d \
  --name db-tiquetes \
  -p 3307:3306 \
  -e MARIADB_ROOT_PASSWORD=Loona23 \
  -e MARIADB_DATABASE=tiquetes_aereos \
  -e MARIADB_USER=admin_tiquetes \
  -e MARIADB_PASSWORD=Loona23 \
  mariadb:latest
2. Configurar la Conexión
Asegúrate de que tu appsettings.json apunte al puerto 3307:

JSON
"DefaultConnection": "Server=127.0.0.1;Port=3307;Database=tiquetes_aereos;User=admin_tiquetes;Password=Loona23;"
3. Aplicar Migraciones
Dentro de tu contenedor de desarrollo, ejecuta:

Bash

dotnet ef database update
```
📈 Estado del Proyecto: 8 Tablas Creadas
Actualmente, el sistema cuenta con 8 tablas base que permiten la gestión completa de:

Aerolíneas 🏢

Aeropuertos 🛫

Vuelos 🎫

Clientes 👥

Reservas 📅

...y más relaciones internas.
---

## 👤 Autor
**Andrés Felipe Navas Alvear** *Front-end Developer & Independent Musician* 📍 Bucaramanga, Colombia

---

## 🛠️ Estado del Proyecto
Este proyecto se encuentra **en desarrollo activo** 🏗️.  
Actualmente enfocado en la lógica de servicios y la construcción de la interfaz de usuario en consola.

---

