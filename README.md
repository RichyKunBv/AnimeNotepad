# AnimeNotepad - Notepad con Temática de Anime

[![Versión](https://img.shields.io/badge/Versión-2.2.0-blue.svg)](https://github.com/RichyKunBv/AnimeNotepad/releases)
[![Codename](https://img.shields.io/badge/Codename-Strelizia-ff69b4.svg)](https://github.com/RichyKunBv/AnimeNotepad/releases/latest)
[![Estable](https://img.shields.io/badge/Estado-Estable-red.svg)](https://github.com/RichyKunBv/AnimeNotepad/releases/latest)
[![Licencia](https://img.shields.io/badge/Licencia-Apache-green.svg)](https://github.com/RichyKunBv/AnimeNotepad/blob/main/LICENSE)
[![Lenguaje](https://img.shields.io/badge/Lenguaje-C%23.NET-lightgrey.svg)](https://dotnet.microsoft.com/es-es/download/dotnet/10.0)
[![GUI](https://img.shields.io/badge/GUI-Avalonia%20UI-purple.svg)](https://avaloniaui.net)

![Linux](https://img.shields.io/badge/Linux-000000?style=for-the-badge&logo=linux&logoColor=white)
![Windows](https://img.shields.io/badge/Windows-000000?style=for-the-badge&logo=windows&logoColor=white)
![macOS](https://img.shields.io/badge/macOS-000000?style=for-the-badge&logo=apple&logoColor=white)

<img width="664" alt="Main" src="res/docs/Principal.png" />

Una sencilla pero divertida aplicación de bloc de notas multiplataforma, con una adorable temática de anime para personalizar tu experiencia.

## 🌟 Acerca de este proyecto

**AnimeNotepad** es una aplicación de escritorio multiplataforma creada como una alternativa a los clásicos blocs de notas. La principal diferencia es su interfaz, que está diseñada con personajes e imágenes de anime para darle un toque único y personal. Es ideal para quienes pasan mucho tiempo escribiendo notas y quieren un entorno más alegre y visualmente atractivo en cualquier sistema operativo.

## ✨ Características

* **Edición de texto completa:** Todas las funcionalidades que esperas de un bloc de notas (abrir, guardar, guardar como, cortar, copiar, pegar, deshacer, rehacer y seleccionar todo).
* **Prevención de pérdida de datos:** Detección de cambios sin guardar (`*`), confirmación inteligente antes de cerrar o abrir un nuevo documento.
* **Barra de estado en tiempo real:** Conteo dinámico de líneas, caracteres totales, posición exacta del cursor (Línea / Columna), estado de guardado y codificación UTF-8.
* **Atajos de teclado nativos:** Soporte completo de combinaciones de teclas estándar (`Ctrl+N`, `Ctrl+O`, `Ctrl+S`, `Ctrl+Shift+S`, `Ctrl+Z`, `Ctrl+Y`, `Ctrl+P`, etc.).
* **Personalización de fuentes y colores con Vista Previa:**
    * Selector de tipografías del sistema con estilos (Negrita, Cursiva) y tamaño ajustable.
    * Paleta de colores para texto con opción de color automático adaptativo al tema.
    * Caja de muestra en vivo (*Live Preview*) antes de aplicar cambios.
* **Zoom sincronizado:** Acercar, alejar y restablecer zoom (100%) manteniendo consistencia con el tamaño de fuente.
* **Impresión:** Envía tus notas directamente a la cola de impresión de tu sistema operativo.
* **Modo Claro y Modo Oscuro:** Adaptación automática al tema de tu sistema operativo, garantizando legibilidad óptima.
* **Interfaz temática:** Detalles visuales y animación inspirados en el anime (Zero Two).


## 🖼️ Capturas de Pantalla

| Splash Screen | Ventana Principal Modo Oscuro | Ventana Principal Modo Claro | Opciones de Fuente y Color |
| :---: | :---: | :---: | :---: |
| <img width="664" alt="SpashScreen" src="res/docs/SplashScreen.png" /> | <img width="664" alt="Main" src="res/docs/Principal Modo Oscuro.png" /> | <img width="664" alt="Main" src="res/docs/Principal Modo Claro.png" /> | <img width="664" alt="FuenteYColor" src="res/docs/fuente y color.png" />

| Impresión | Acerca de | Actualizar |
| :---: | :---: | :---: |
| <img width="664" alt="Impresion" src="res/docs/impresion.png" /> | <img width="664" alt="Acerca de" src="res/docs/acerca de.png" /> | <img width="664" alt="Actualizar" src="res/docs/actualizar.png" /> |

## 🛠️ Tecnologías y Dependencias

El proyecto está desarrollado en **C# (.NET 10)** y se apoya en las siguientes bibliotecas y tecnologías:

- [Avalonia UI](https://avaloniaui.net/): Framework de interfaz de usuario para aplicaciones de escritorio multiplataforma.
- [Internet](https://github.com/RichyKunBv/AnimeNotepad/releases/latest): Herramienta para actualizar la aplicacion

## ⬇️ Descargas / Instalación

Puedes descargar los binarios precompilados listos para usar desde la sección de **[Releases](https://github.com/RichyKunBv/AnimeNotepad/releases/latest)**.

Están disponibles para todos los sistemas operativos principales (**Windows, macOS y Linux**) en las arquitecturas más utilizadas:
- **x64** (Procesadores Intel y AMD)
- **arm64** (Apple Silicon y procesadores ARM)

```sh
# Para macOS, quitar el atributo de cuarentena si no se puede abrir la app
sudo xattr -cr /Applications/AnimeNotepad.app
```

## 🚀 Cómo compilar y ejecutar

### Requisitos Previos
- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)

### Compilación y Ejecución
Asegúrate de ejecutar los comandos desde el directorio raíz del proyecto (donde se encuentra `AnimeNotepad.slnx`):

```bash
cd src
dotnet run
```


Para la creación y empaquetado de la aplicación, dispones de scripts automatizados en el directorio `scripts/`:

- **macOS**: `scripts/build_animenotepad_in_macos.sh` (empaqueta como `.app` nativa y también compila para Windows y Linux).
- **Windows**: `scripts/PRE_build_animenotepad_in_windows.ps1` (facilita la creación de paquetes para múltiples sistemas y arquitecturas).
- **Linux**: `scripts/PRE_build_animenotepad_in_linux.sh` (facilita la creación de paquetes para múltiples sistemas y arquitecturas).

> **Nota:** Todos los scripts se encuentran en la carpeta `scripts/`. En los scripts de Linux y Windows no se puede firmar la aplicación para macOS, ya que este paso es exclusivo y debe realizarse desde una Mac.

## 👤 Autor

* **RichyKunBv** - [GitHub](https://github.com/RichyKunBv)

