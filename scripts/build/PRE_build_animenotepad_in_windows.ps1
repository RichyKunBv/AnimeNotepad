<#
    AnimeNotepad Build Script - Windows Edition
    Adaptado para PowerShell en Windows
#>

# Verificar que se ejecuta en Windows (aviso si no)
if (-not $IsWindows -and $PSVersionTable.Platform -ne 'Win32NT') {
    Write-Host "Aviso: Este script está diseñado para Windows. Si lo ejecutas en otro SO, probablemente falle." -ForegroundColor Yellow
    Read-Host "`nPulsa Enter para continuar..."
}

# Definir rutas (usando el perfil de usuario de Windows)
$PROJECT_DIR   = "$env:USERPROFILE\AnimeNotepad"
$WIN_X64_DIR   = "$env:USERPROFILE\AnimeNotepadX86_64Windows"
$WIN_ARM_DIR   = "$env:USERPROFILE\AnimeNotepadARM64Windows"
$LINUX_X64_DIR = "$env:USERPROFILE\AnimeNotepadX86_64Linux"
$LINUX_ARM_DIR = "$env:USERPROFILE\AnimeNotepadARM64Linux"

# Para macOS (solo se crean carpetas; el empaquetado no es posible en Windows)
$MACOS_ARM_APP = "$env:USERPROFILE\AnimeNotepadARM64macOS\AnimeNotepad.app"
$MACOS_X64_APP = "$env:USERPROFILE\AnimeNotepadX86_64macOS\AnimeNotepad.app"

# ------------------------------------------------------------
# Funciones
# ------------------------------------------------------------
function carpeta_macOS {
    Write-Host "Creando carpetas para macOS..."

    # Crear los directorios necesarios (equivale a mkdir -p con llaves)
    $macOSDirs = @(
        "$MACOS_ARM_APP\Contents\MacOS",
        "$MACOS_ARM_APP\Contents\Resources",
        "$MACOS_X64_APP\Contents\MacOS",
        "$MACOS_X64_APP\Contents\Resources"
    )
    foreach ($dir in $macOSDirs) {
        New-Item -ItemType Directory -Force -Path $dir | Out-Null
    }

    # Copiar Info.plist.template a ambas apps (ignorar errores si no existe)
    $template = "$PROJECT_DIR\src\Info.plist.template"
    if (Test-Path $template) {
        Copy-Item -Force $template "$MACOS_ARM_APP\Contents\Info.plist"
        Copy-Item -Force $template "$MACOS_X64_APP\Contents\Info.plist"
    } else {
        Write-Host "Aviso: No se encontró Info.plist.template base"
    }

    Write-Host "Carpetas de macOS creadas"
}

function actualizar_macOS {
    Write-Host "=== Iniciando compilación de AnimeNotepad para macOS desde Windows ==="

    # Extraer versión del .csproj
    $csprojPath = "$PROJECT_DIR\src\AnimeNotepad.csproj"
    if (-not (Test-Path $csprojPath)) {
        Write-Host "Error: No se encontró $csprojPath" -ForegroundColor Red
        return
    }
    $csprojContent = Get-Content $csprojPath -Raw
    if ($csprojContent -match '<Version>(.*?)</Version>') {
        $VERSION = $Matches[1]
    } else {
        Write-Host "Error: No se encontró la etiqueta <Version> en el .csproj" -ForegroundColor Red
        return
    }

    Write-Host "Versión detectada: $VERSION"
    Write-Host "Publicando binarios..."

    Set-Location "$PROJECT_DIR\src"

    dotnet publish -c Release -r osx-arm64 --self-contained -p:PublishSingleFile=true
    dotnet publish -c Release -r osx-x64 --self-contained -p:PublishSingleFile=true

    Write-Host "Copiando archivos y actualizando Info.plist..."

    # --- ARM64 ---
    # Copiar binarios a la estructura .app (se sobreescribe si existe)
    Copy-Item -Recurse -Force "$PROJECT_DIR\src\bin\Release\net10.0\osx-arm64\publish\*" "$MACOS_ARM_APP\Contents\MacOS\"
    # Copiar icono (si existe)
    if (Test-Path "$PROJECT_DIR\src\Assets\icon.icns") {
        Copy-Item -Force "$PROJECT_DIR\src\Assets\icon.icns" "$MACOS_ARM_APP\Contents\Resources\"
    }

    # Asegurar que Info.plist existe
    if (-not (Test-Path "$MACOS_ARM_APP\Contents\Info.plist")) {
        Copy-Item -Force "$PROJECT_DIR\src\Info.plist.template" "$MACOS_ARM_APP\Contents\Info.plist"
    }

    # Actualizar versión con Python (plistlib nativo)
    $plistPath = "$MACOS_ARM_APP\Contents\Info.plist"
    if (Test-Path $plistPath) {
        python -c "import plistlib, sys; p=sys.argv[1]; d=plistlib.load(open(p,'rb')); d['CFBundleVersion']=sys.argv[2]; d['CFBundleShortVersionString']=sys.argv[2]; plistlib.dump(d,open(p,'wb'))" $plistPath $VERSION
        if ($LASTEXITCODE -ne 0) { Write-Host "Aviso: No se pudo actualizar Info.plist ARM64" }
    }

    # --- x64 --- (lo mismo)
    Copy-Item -Recurse -Force "$PROJECT_DIR\src\bin\Release\net10.0\osx-x64\publish\*" "$MACOS_X64_APP\Contents\MacOS\"
    if (Test-Path "$PROJECT_DIR\src\Assets\icon.icns") {
        Copy-Item -Force "$PROJECT_DIR\src\Assets\icon.icns" "$MACOS_X64_APP\Contents\Resources\"
    }

    if (-not (Test-Path "$MACOS_X64_APP\Contents\Info.plist")) {
        Copy-Item -Force "$PROJECT_DIR\src\Info.plist.template" "$MACOS_X64_APP\Contents\Info.plist"
    }

    $plistPath = "$MACOS_X64_APP\Contents\Info.plist"
    if (Test-Path $plistPath) {
        python -c "import plistlib, sys; p=sys.argv[1]; d=plistlib.load(open(p,'rb')); d['CFBundleVersion']=sys.argv[2]; d['CFBundleShortVersionString']=sys.argv[2]; plistlib.dump(d,open(p,'wb'))" $plistPath $VERSION
        if ($LASTEXITCODE -ne 0) { Write-Host "Aviso: No se pudo actualizar Info.plist x64" }
    }

    Write-Host "=== ¡Listo! AnimeNotepad v$VERSION empaquetado para macOS desde Windows ==="
    Write-Host "Aviso: El .app NO está firmado ni tiene atributos extendidos (xattr/codesign)."
    Write-Host "       Deberás firmarlo en una Mac antes de distribuirlo."
}

function carpeta_windows {
    Write-Host "Creando carpetas para Windows..."
    New-Item -ItemType Directory -Force -Path $WIN_X64_DIR | Out-Null
    New-Item -ItemType Directory -Force -Path $WIN_ARM_DIR | Out-Null
    Write-Host "Carpetas de Windows creadas"
}

function actualizar_windows {
    Write-Host "=== Iniciando compilación de AnimeNotepad para Windows ==="

    # Obtener versión del .csproj usando expresión regular
    $csprojPath = "$PROJECT_DIR\src\AnimeNotepad.csproj"
    if (-not (Test-Path $csprojPath)) {
        Write-Host "Error: No se encontró $csprojPath" -ForegroundColor Red
        return
    }
    $csprojContent = Get-Content $csprojPath -Raw
    if ($csprojContent -match '<Version>(.*?)</Version>') {
        $VERSION = $Matches[1]
    } else {
        Write-Host "Error: No se encontró la etiqueta <Version> en el .csproj" -ForegroundColor Red
        return
    }

    Write-Host "Versión detectada: $VERSION"
    Write-Host "Publicando binarios..."

    Set-Location "$PROJECT_DIR\src"

    dotnet publish -c Release -r win-arm64 --self-contained -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -p:DebugType=None
    dotnet publish -c Release -r win-x64   --self-contained -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -p:DebugType=None

    Write-Host "Copiando ejecutables..."

    # ARM64
    Copy-Item -Recurse -Force "$PROJECT_DIR\src\bin\Release\net10.0\win-arm64\publish\*" $WIN_ARM_DIR
    Remove-Item -Force "$WIN_ARM_DIR\*.pdb" -ErrorAction SilentlyContinue

    # x64
    Copy-Item -Recurse -Force "$PROJECT_DIR\src\bin\Release\net10.0\win-x64\publish\*" $WIN_X64_DIR
    Remove-Item -Force "$WIN_X64_DIR\*.pdb" -ErrorAction SilentlyContinue

    Write-Host "=== ¡Listo! AnimeNotepad v$VERSION empaquetado para Windows (x64/ARM64) ==="
}

function carpeta_linux {
    Write-Host "Creando carpetas para Linux..."
    New-Item -ItemType Directory -Force -Path $LINUX_X64_DIR, $LINUX_ARM_DIR | Out-Null

    # Copiar plantilla .desktop (si existe)
    $desktopTemplate = "$PROJECT_DIR\src\AnimeNotepad.desktop"
    if (Test-Path $desktopTemplate) {
        Copy-Item $desktopTemplate $LINUX_X64_DIR
        Copy-Item $desktopTemplate $LINUX_ARM_DIR
    } else {
        Write-Host "Aviso: No se encontró AnimeNotepad.desktop base"
    }
    Write-Host "Carpetas de Linux creadas"
}

function actualizar_linux {
    Write-Host "=== Iniciando compilación de AnimeNotepad para Linux ==="

    $csprojPath = "$PROJECT_DIR\src\AnimeNotepad.csproj"
    if (-not (Test-Path $csprojPath)) {
        Write-Host "Error: No se encontró $csprojPath" -ForegroundColor Red
        return
    }
    $csprojContent = Get-Content $csprojPath -Raw
    if ($csprojContent -match '<Version>(.*?)</Version>') {
        $VERSION = $Matches[1]
    } else {
        Write-Host "Error: No se encontró la etiqueta <Version> en el .csproj" -ForegroundColor Red
        return
    }

    Write-Host "Versión detectada: $VERSION"
    Write-Host "Publicando binarios..."

    Set-Location "$PROJECT_DIR\src"

    dotnet publish -c Release -r linux-arm64 --self-contained -p:IncludeNativeLibrariesForSelfExtract=true -p:DebugType=None -p:PublishSingleFile=true
    dotnet publish -c Release -r linux-x64   --self-contained -p:IncludeNativeLibrariesForSelfExtract=true -p:DebugType=None -p:PublishSingleFile=true

    Write-Host "Copiando archivos y actualizando .desktop..."

    # ARM64
    Copy-Item -Recurse -Force "$PROJECT_DIR\src\bin\Release\net10.0\linux-arm64\publish\*" $LINUX_ARM_DIR
    Copy-Item -Force "$PROJECT_DIR\src\Assets\icon.png" $LINUX_ARM_DIR -ErrorAction SilentlyContinue

    $desktopFile = "$LINUX_ARM_DIR\AnimeNotepad.desktop"
    if (Test-Path $desktopFile) {
        (Get-Content $desktopFile) -replace '^Version=.*', "Version=$VERSION" | Set-Content $desktopFile
    }

    # x64
    Copy-Item -Recurse -Force "$PROJECT_DIR\src\bin\Release\net10.0\linux-x64\publish\*" $LINUX_X64_DIR
    Copy-Item -Force "$PROJECT_DIR\src\Assets\icon.png" $LINUX_X64_DIR -ErrorAction SilentlyContinue

    $desktopFile = "$LINUX_X64_DIR\AnimeNotepad.desktop"
    if (Test-Path $desktopFile) {
        (Get-Content $desktopFile) -replace '^Version=.*', "Version=$VERSION" | Set-Content $desktopFile
    }

    Write-Host "=== ¡Listo! AnimeNotepad v$VERSION empaquetado para Linux (x64/ARM64) ==="
}

function carpeta_todos {
    carpeta_macOS
    carpeta_windows
    carpeta_linux
}

function actualizar_todos {
    actualizar_macOS   # avisará de que no se puede
    actualizar_windows
    actualizar_linux
}

function comprimir_todos {
    Write-Host "Comprimiendo todas las versiones..."
    $dirs = @(
        "AnimeNotepadARM64macOS",
        "AnimeNotepadX86_64macOS",
        "AnimeNotepadARM64Windows",
        "AnimeNotepadX86_64Windows",
        "AnimeNotepadARM64Linux",
        "AnimeNotepadX86_64Linux"
    )

    Set-Location $env:USERPROFILE

    foreach ($dir in $dirs) {
        if (Test-Path $dir) {
            $zipFile = "$dir.zip"
            if (Test-Path $zipFile) {
                Write-Host "Eliminando $zipFile existente..."
                Remove-Item $zipFile -Force
            }
            Compress-Archive -Path $dir -DestinationPath $zipFile -Force
            Write-Host "Comprimido: $zipFile"
        } else {
            Write-Host "Aviso: no existe el directorio $dir, se omite compresión."
        }
    }
    Write-Host "Compresión completada."
}

function press_any_key {
    Write-Host ""
    Read-Host "Pulsa Enter para volver al menú..."
}

# ------------------------------------------------------------
# Menú principal
# ------------------------------------------------------------
function show_menu {
    Set-Location $PROJECT_DIR
    Clear-Host
    Write-Host "--------------------------------------------------"
    Write-Host "   1) Crear estructura de carpetas para macOS"
    Write-Host "   2) Compilar y empaquetar para macOS (solo avisa)"
    Write-Host "--------------------------------------------------"
    Write-Host "   3) Crear estructura de carpetas para Windows"
    Write-Host "   4) Compilar y empaquetar para Windows"
    Write-Host "--------------------------------------------------"
    Write-Host "   5) Crear estructura de carpetas para Linux"
    Write-Host "   6) Compilar y empaquetar para Linux"
    Write-Host "--------------------------------------------------"
    Write-Host "   8) Crear estructura de carpetas para TODOS"
    Write-Host "   9) Compilar y empaquetar para TODOS"
    Write-Host "--------------------------------------------------"
    Write-Host "   X) Salir"
    $choice = Read-Host "   >> Introduce tu elección"
    Write-Host ""

    switch ($choice) {
        "1" { carpeta_macOS; press_any_key }
        "2" { actualizar_macOS; press_any_key }
        "3" { carpeta_windows; press_any_key }
        "4" { actualizar_windows; press_any_key }
        "5" { carpeta_linux; press_any_key }
        "6" { actualizar_linux; press_any_key }
        "8" { carpeta_todos; press_any_key }
        "9" { actualizar_todos; comprimir_todos; press_any_key }
        "X" { Write-Host "Saliendo... ¡Hasta pronto!"; exit 0 }
        "x" { Write-Host "Saliendo... ¡Hasta pronto!"; exit 0 }
        default { Write-Host "Opción inválida. Por favor, intenta de nuevo."; Start-Sleep -Seconds 2 }
    }
}

# Bucle infinito del menú
while ($true) {
    show_menu
}