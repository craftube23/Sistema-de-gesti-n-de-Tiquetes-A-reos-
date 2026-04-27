# ✈️ Sistema de Gestión de Tiquetes Aéreos

¡Bienvenido! Este es un sistema de gestión de vuelos y reservas desarrollado en **.NET 10** con **MariaDB**, diseñado bajo una **Arquitectura Multicapa** robusta y organizada para facilitar el mantenimiento y la escalabilidad.¡Bienvenido! Este es un sistema de gestión de vuelos y reservas desarrollado en **.NET 10** con **MariaDB**, diseñado para ser robusto, escalable y fácil de administrar.

---

## 🛠️ Tecnologías Utilizadas

- **Lenguaje:** C# (.NET 10)
- **ORM:** Entity Framework Core (MySQL/MariaDB Provider — Pomelo)
- **Base de Datos:** MariaDB (vía Podman)
- **Entorno de Desarrollo:** Linux (Bazzite / Fedora)
- **Arquitectura:** Separación por capas (Data, Models, Services, UI)

---

## 🏗️ Estructura del Proyecto

El proyecto está organizado de la siguiente manera para mantener un código limpio y mantenible:

```
Sistema-de-gesti-n-de-Tiquetes-Areos-/
├── Data/          → Contexto de la base de datos y configuración de EF Core
├── Models/        → Entidades del dominio (Aerolínea, Vuelo, Cliente, Reserva, etc.)
├── Services/      → Lógica de negocio y manipulación de datos
├── UI/            → Menús interactivos para la consola
├── Migrations/    → Historial de versiones de la base de datos
└── Program.cs     → Punto de entrada de la aplicación
```

---

## 🚀 Configuración del Entorno ( Linux)

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
```

### 2. Configurar la Conexión

Asegúrate de que tu `appsettings.json` apunte al puerto correcto:

```json
"DefaultConnection": "Server=127.0.0.1;Port=3306;Database=tiquetes_aereos;Uid=admin_tiquetes;Pwd=Loona23;" 
```

### 3. Aplicar Migraciones

Dentro de tu contenedor de desarrollo, ejecuta:

```bash
dotnet ef database update
```

### 4. Correr el proyecto

```bash
dotnet run
```

---

## 📋 Módulos del Sistema


| Menú | Módulo     | Función principal                                             |
| ----- | ----------- | -------------------------------------------------------------- |
| 1     | Aerolíneas | Registrar, listar, actualizar y desactivar aerolíneas         |
| 2     | Aeropuertos | Registrar aeropuertos con código IATA, ciudad y país         |
| 3     | Vuelos      | Crear vuelos con distribución de asientos por clase           |
| 4     | Clientes    | Registro con validación de documento, email y teléfono       |
| 5     | Reservas    | Crear, confirmar y cancelar reservas con selección de asiento |
| 6     | Tiquetes    | Emitir tiquetes desde reservas confirmadas                     |
| 7     | Pagos       | Registrar pagos (Tarjeta, Efectivo, PSE)                       |
| 8     | Reportes    | Consultas LINQ: destinos, ingresos, clientes frecuentes        |

---

## 🎫 Sistema de Clases y Asientos

Al registrar un vuelo se define la distribución de asientos por clase. Los precios se calculan automáticamente con un multiplicador sobre el precio base:


| Clase         | Multiplicador | Asientos por fila |
| ------------- | ------------- | ----------------- |
| Económica    | × 1.0        | 6 (A–F)          |
| Ejecutiva     | × 1.8        | 4 (A–D)          |
| Primera Clase | × 3.0        | 3 (A–C)          |

Los asientos pueden tener 4 estados: `DISPONIBLE`, `RESERVADO`, `OCUPADO` y `BLOQUEADO`.

---

## 📈 Estado del Proyecto: 9 Tablas en Base de Datos

El sistema cuenta con las siguientes tablas:

- `Aerolineas` 🏢
- `Aeropuertos` 🛫
- `Vuelos` ✈️
- `Clientes` 👥
- `Reservas` 📅
- `Tiquetes` 🎫
- `Pagos` 💳
- `Asientos` 💺
- `ClasesVuelo` 🏷️

---

## 🤝 Participantes y Contribuciones

Este proyecto fue desarrollado de forma colaborativa con apoyo de herramientas de inteligencia artificial:


| Participante                    | Rol                                             | Contribución |
| ------------------------------- | ----------------------------------------------- | ------------- |
| **Andrés Felipe Navas Alvear** | Autor principal                                 | 45%           |
| **Claude (Anthropic)**          | Asistente IA principal                          | 40%           |
| **Gemini (Google)**             | Asistente IA de apoyo en corrección de errores | 15%           |

**Claude** apoyó en la arquitectura del proyecto, generación de código base, modelos, servicios, menús de consola, sistema de asientos por clase, validaciones y consultas LINQ.

**Gemini** colaboró en la revisión y corrección de errores puntuales durante el desarrollo.

---

## 👤 Autor

**Andrés Felipe Navas Alvear**
*Front-end Developer & Independent Musician*
📍 Bucaramanga, Colombia

---

## 🏗️ Estado del Proyecto

Este proyecto se encuentra **en desarrollo activo** 🏗️

Actualmente con funcionalidad completa en consola, sistema de asientos por clase implementado y módulo de reportes LINQ operativo.

---

## 📝 Historial de Commits

El proyecto sigue la convención de **Gitmoji + Conventional Commits**:

```
🎉 feat(ui): agregar menú principal y completar sistema funcional
📊 feat(reports): agregar consultas LINQ y módulo de reportes
✨ feat(seats): agregar gestión de asientos por clase con mapa visual
✨ feat(modules): agregar servicios y menús de tiquetes y pagos
✨ feat(modules): agregar servicios y menús de clientes y reservas
✨ feat(modules): agregar servicios y menús de aerolíneas, aeropuertos y vuelos
🗄️ feat(data): configurar DbContext y conexión MySQL con EF Core
✨ feat(models): agregar entidades del dominio
🎉 init: inicializar proyecto consola .NET
```

---

*Desarrollado con ❤️ en Bucaramanga, Colombia*
