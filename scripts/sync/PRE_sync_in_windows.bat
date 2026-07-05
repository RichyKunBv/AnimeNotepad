@echo off
setlocal enabledelayedexpansion

:: =====================================================
:: Script de sincronización para Windows
:: Adaptado del original para Linux/macOS
:: =====================================================

set "nombre=AnimeNotepad"
set "repo_url=https://github.com/RichyKunBv/%nombre%.git"
set "ubicacion=%USERPROFILE%\%nombre%"

:: Verificar si la carpeta existe y cambiar a ella (solo por comodidad)
if exist "%ubicacion%" (
    cd /d "%ubicacion%"
)

:: ----------------------------------------------------------
:: Función: Descargar (Pull)
:descargar
echo Obteniendo últimos cambios del repositorio...
git pull origin main
goto :eof

:: ----------------------------------------------------------
:: Función: Publicar (Push)
:publicar
git add .
set /p "mensaje=   >> Introduce el mensaje del commit: "

if "!mensaje!"=="" (
    echo El mensaje no puede estar vacío. Cancelando publicación...
    goto :eof
)

git commit -m "!mensaje!"
git pull origin main --rebase
git push origin main
goto :eof

:: ----------------------------------------------------------
:: Función: Configurar entorno
:configurar
cls
echo === Clonar y Configurar Entorno de %nombre% ===

if not exist "%ubicacion%" (
    echo.
    echo Descargando el código...
    cd /d "%USERPROFILE%"
    git clone "%repo_url%"
) else (
    echo.
    echo La carpeta ya existe. Saltando la clonación...
)

cd /d "%ubicacion%" || (
    echo Error al entrar a la carpeta
    goto :eof
)

echo.
echo Configurando editor...
git config --global core.editor "notepad"

echo.
echo ¡Entorno de %nombre% configurado y listo para programar!
echo Nota: Al hacer tu primer 'push', te pedirá tus credenciales como nombre de usuario, correo y un Token de Acceso Personal (generado en GitHub).
goto :eof

:: ----------------------------------------------------------
:: Función: Clonar
:clonar
echo.
echo Clonando el repositorio...
git clone "%repo_url%"
goto :eof

:: ----------------------------------------------------------
:: Función: Pausa interactiva
:press_any_key
echo.
echo Pulsa cualquier tecla para volver al menú...
pause >nul
goto :eof

:: ----------------------------------------------------------
:: Menú principal
:menu
cls
echo    1) Actualizar local (Pull)
echo    2) Actualizar el repo (Push)
echo    0) Configurar
echo    9) Clonar
echo    X) Salir
echo.
set /p "choice=   >> Introduce tu elección: "

if /i "!choice!"=="1" (
    call :descargar
    call :press_any_key
) else if /i "!choice!"=="2" (
    call :publicar
    call :press_any_key
) else if /i "!choice!"=="0" (
    call :configurar
    call :press_any_key
) else if /i "!choice!"=="9" (
    call :clonar
    call :press_any_key
) else if /i "!choice!"=="X" (
    echo Saliendo... ¡Hasta pronto!
    exit /b 0
) else (
    echo Opción inválida. Por favor, intenta de nuevo.
    timeout /t 2 >nul
)
goto menu