#!/bin/bash

# Es para hacer las compilaciones desde macOS, realmente ignorenlo ya que es algo mas para mi que para ustedes, nada mas para mas facilidad
# Tambien como yo trabajo en macOS los comandos son en UNIX y no funconarian bien en Linux y mucho menos en Windows
# Ermano que tardado es hacer scripts :VVVV

if [[ "$(uname)" != "Darwin" ]]; then
    echo "Aviso: macOS no detectado, Si lo estas usando en linux es probable que no funcione correctamente. Es bajo tu responsabilidad. Ya que esta diseñado para macOS y no para Linux."

    echo -e "\nPulsa cualquier tecla para continuar..."
    read -n 1 -s -r
fi

# Definir las rutas base al estilo sync.sh
nombre="AnimeNotepad"
repos_base="$HOME/Repos"
old_location="$HOME/$nombre"
PROJECT_DIR=""

ensure_repos_dir() {
    if [ ! -d "$repos_base" ]; then
        echo "Creando carpeta predeterminada de repositorios en $repos_base..."
        mkdir -p "$repos_base"
    fi
}

detect_repository_location() {
    if [ -d "$repos_base/$nombre/.git" ]; then
        PROJECT_DIR="$repos_base/$nombre"
    elif [ -d "$old_location/.git" ]; then
        ensure_repos_dir

        if [ -e "$repos_base/$nombre" ]; then
            echo "Existe un repositorio en $old_location y también en $repos_base/$nombre."
            echo "Por favor mueve manualmente el contenido o elimina uno de ellos."
            exit 1
        fi

        echo "Moviendo repositorio existente de $old_location a $repos_base/$nombre..."
        mv "$old_location" "$repos_base/"
        PROJECT_DIR="$repos_base/$nombre"
    else
        PROJECT_DIR="$repos_base/$nombre"
    fi
}

detect_repository_location

MACOS_ARM_APP="$HOME/AnimeNotepadARM64macOS/AnimeNotepad.app"
MACOS_X64_APP="$HOME/AnimeNotepadX86_64macOS/AnimeNotepad.app"

WIN_X64_DIR="$HOME/AnimeNotepadX86_64Windows/"
WIN_ARM_DIR="$HOME/AnimeNotepadARM64Windows/"

LINUX_X64_DIR="$HOME/AnimeNotepadX86_64Linux/"
LINUX_ARM_DIR="$HOME/AnimeNotepadARM64Linux/"

# Para macOS
carpeta_macOS() {
    echo "Creando carpetas..."
    mkdir -p "$MACOS_X64_APP/Contents/"{MacOS,Resources}
    mkdir -p "$MACOS_ARM_APP/Contents/"{MacOS,Resources}
    
    # IMPORTANTE: Copiar el Info.plist base para que plutil tenga algo que editar después
    cp "$PROJECT_DIR/src/Info.plist.template" "$MACOS_X64_APP/Contents/Info.plist" 2>/dev/null || echo "Aviso: No se encontró Info.plist.template base"
    cp "$PROJECT_DIR/src/Info.plist.template" "$MACOS_ARM_APP/Contents/Info.plist" 2>/dev/null
    
    echo "Carpetas creadas"
}

actualizar_macOS() {
    echo "=== Iniciando compilación de AnimeNotepad ==="

    # 1. Extraer la versión del archivo .csproj usando awk
    VERSION=$(awk -F'[><]' '/<Version>/{print $3}' "$PROJECT_DIR/src/AnimeNotepad.csproj")

    if [ -z "$VERSION" ]; then
        echo "Error: No se pudo encontrar la etiqueta <Version> en el .csproj"
        return
    fi

    echo "Versión detectada: $VERSION"
    echo "Publicando binarios..."

    # 2. Compilar ambas arquitecturas
    cd "$PROJECT_DIR/src"
    dotnet publish -c Release -r osx-arm64 --self-contained -p:PublishSingleFile=true
    dotnet publish -c Release -r osx-x64 --self-contained -p:PublishSingleFile=true

    echo "Copiando archivos y actualizando Info.plist..."

    # 3. Procesar versión ARM64
    cp -a "$PROJECT_DIR/src/bin/Release/net10.0/osx-arm64/publish/." "$MACOS_ARM_APP/Contents/MacOS/"
    cp "$PROJECT_DIR/src/Assets/icon.icns" "$MACOS_ARM_APP/Contents/Resources/"
    # plutil edita el valor de la llave CFBundleVersion de forma segura
    plutil -replace CFBundleVersion -string "$VERSION" "$MACOS_ARM_APP/Contents/Info.plist"
    plutil -replace CFBundleShortVersionString -string "$VERSION" "$MACOS_ARM_APP/Contents/Info.plist"

    # 4. Procesar versión X86_64
    cp -a "$PROJECT_DIR/src/bin/Release/net10.0/osx-x64/publish/." "$MACOS_X64_APP/Contents/MacOS/"
    cp "$PROJECT_DIR/src/Assets/icon.icns" "$MACOS_X64_APP/Contents/Resources/"
    plutil -replace CFBundleVersion -string "$VERSION" "$MACOS_X64_APP/Contents/Info.plist"
    plutil -replace CFBundleShortVersionString -string "$VERSION" "$MACOS_X64_APP/Contents/Info.plist"

    echo "Limpiando atributos y firmando aplicaciones..."

    # 5. Firmar ARM64
    xattr -cr "$MACOS_ARM_APP"
    codesign --force --deep --sign - "$MACOS_ARM_APP"

    # 6. Firmar X86_64
    xattr -cr "$MACOS_X64_APP"
    codesign --force --deep --sign - "$MACOS_X64_APP"

    echo "=== ¡Listo! AnimeNotepad v$VERSION empaquetado para ambas arquitecturas ==="
}

# Para Windows
carpeta_windows() {
    echo "Creando carpetas para Windows..."
    mkdir -p "$WIN_X64_DIR"
    mkdir -p "$WIN_ARM_DIR"
    echo "Carpetas de Windows creadas"
}

actualizar_windows() {
    echo "=== Iniciando compilación de AnimeNotepad para Windows ==="

    VERSION=$(awk -F'[><]' '/<Version>/{print $3}' "$PROJECT_DIR/src/AnimeNotepad.csproj")

    if [ -z "$VERSION" ]; then
        echo "Error: No se pudo encontrar la etiqueta <Version> en el .csproj"
        return
    fi

    echo "Versión detectada: $VERSION"
    echo "Publicando binarios..."

    cd "$PROJECT_DIR/src"
# Compilar para Windows ARM64
    dotnet publish -c Release -r win-arm64 --self-contained -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -p:DebugType=None
    # Compilar para Windows x64
    dotnet publish -c Release -r win-x64 --self-contained -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -p:DebugType=None

    echo "Copiando ejecutables..."

    # Procesar versión ARM64
    cp -a "$PROJECT_DIR/src/bin/Release/net10.0/win-arm64/publish/." "$WIN_ARM_DIR/"
    rm -rf "$WIN_ARM_DIR"/*.pdb

    # Procesar versión X86_64
    cp -a "$PROJECT_DIR/src/bin/Release/net10.0/win-x64/publish/." "$WIN_X64_DIR/"
    rm -rf "$WIN_X64_DIR"/*.pdb

    echo "=== ¡Listo! AnimeNotepad v$VERSION empaquetado para Windows (x64/ARM64) ==="
}

# Para Linux
carpeta_linux() {
    echo "Creando carpetas para Linux..."
    mkdir -p "$LINUX_X64_DIR"
    mkdir -p "$LINUX_ARM_DIR"
    
    # Copiar el archivo .desktop base para tener algo que editar
    cp "$PROJECT_DIR/src/AnimeNotepad.desktop" "$LINUX_X64_DIR/" 2>/dev/null || echo "Aviso: No se encontró AnimeNotepad.desktop base"
    cp "$PROJECT_DIR/src/AnimeNotepad.desktop" "$LINUX_ARM_DIR/" 2>/dev/null
    
    echo "Carpetas de Linux creadas"
}

actualizar_linux() {
    echo "=== Iniciando compilación de AnimeNotepad para Linux ==="

    VERSION=$(awk -F'[><]' '/<Version>/{print $3}' "$PROJECT_DIR/src/AnimeNotepad.csproj")

    if [ -z "$VERSION" ]; then
        echo "Error: No se pudo encontrar la etiqueta <Version> en el .csproj"
        return
    fi

    echo "Versión detectada: $VERSION"
    echo "Publicando binarios..."

    cd "$PROJECT_DIR/src"
# Compilar para Linux ARM64
    dotnet publish -c Release -r linux-arm64 --self-contained -p:IncludeNativeLibrariesForSelfExtract=true -p:DebugType=None -p:PublishSingleFile=true    
# Compilar para Linux x64
    dotnet publish -c Release -r linux-x64 --self-contained -p:IncludeNativeLibrariesForSelfExtract=true -p:DebugType=None -p:PublishSingleFile=true

    echo "Copiando archivos y actualizando .desktop..."

    # Procesar versión ARM64
    cp -a "$PROJECT_DIR/src/bin/Release/net10.0/linux-arm64/publish/." "$LINUX_ARM_DIR/"
    cp "$PROJECT_DIR/src/Assets/icon.png" "$LINUX_ARM_DIR/"
    
    # Usamos sed para buscar la línea "Version=" y reemplazarla. 
    # El '.bak' asegura compatibilidad tanto si ejecutas esto en macOS como en Linux.
    if [ -f "$LINUX_ARM_DIR/AnimeNotepad.desktop" ]; then
        sed -i.bak "s/^Version=.*/Version=$VERSION/" "$LINUX_ARM_DIR/AnimeNotepad.desktop"
        rm "$LINUX_ARM_DIR/AnimeNotepad.desktop.bak"
    fi

    # Procesar versión X86_64
    cp -a "$PROJECT_DIR/src/bin/Release/net10.0/linux-x64/publish/." "$LINUX_X64_DIR/"
    cp "$PROJECT_DIR/src/Assets/icon.png" "$LINUX_X64_DIR/"
    
    if [ -f "$LINUX_X64_DIR/AnimeNotepad.desktop" ]; then
        sed -i.bak "s/^Version=.*/Version=$VERSION/" "$LINUX_X64_DIR/AnimeNotepad.desktop"
        rm "$LINUX_X64_DIR/AnimeNotepad.desktop.bak"
    fi

    echo "=== ¡Listo! AnimeNotepad v$VERSION empaquetado para Linux (x64/ARM64) ==="
}

carpeta_todos(){
    carpeta_macOS
    carpeta_windows
    carpeta_linux
}

actualizar_todos(){
    actualizar_macOS
    actualizar_windows
    actualizar_linux
}

comprimir_todos(){
        echo "Comprimiendo todas las versiones..."

    if [ -f "$HOME/AnimeNotepadARM64macOS.zip" ]; then
        echo "El archivo AnimeNotepadARM64macOS.zip ya existe, eliminando..."
        rm "$HOME/AnimeNotepadARM64macOS.zip"
    fi
    if [ -f "$HOME/AnimeNotepadX86_64macOS.zip" ]; then
        echo "El archivo AnimeNotepadX86_64macOS.zip ya existe, eliminando..."
        rm "$HOME/AnimeNotepadX86_64macOS.zip"
    fi
    if [ -f "$HOME/AnimeNotepadARM64Windows.zip" ]; then
        echo "El archivo AnimeNotepadARM64Windows.zip ya existe, eliminando..."
        rm "$HOME/AnimeNotepadARM64Windows.zip"
    fi
    if [ -f "$HOME/AnimeNotepadX86_64Windows.zip" ]; then
        echo "El archivo AnimeNotepadX86_64Windows.zip ya existe, eliminando..."
        rm "$HOME/AnimeNotepadX86_64Windows.zip"
    fi
    if [ -f "$HOME/AnimeNotepadARM64Linux.zip" ]; then
        echo "El archivo AnimeNotepadARM64Linux.zip ya existe, eliminando..."
        rm "$HOME/AnimeNotepadARM64Linux.zip"
    fi
    if [ -f "$HOME/AnimeNotepadX86_64Linux.zip" ]; then
        echo "El archivo AnimeNotepadX86_64Linux.zip ya existe, eliminando..."
        rm "$HOME/AnimeNotepadX86_64Linux.zip"
    fi

    cd "$HOME" || exit 1
    for dir in AnimeNotepadARM64macOS AnimeNotepadX86_64macOS AnimeNotepadARM64Windows AnimeNotepadX86_64Windows AnimeNotepadARM64Linux AnimeNotepadX86_64Linux; do
        if [ -d "$dir" ]; then
            zip -r "${dir}.zip" "$dir"
        else
            echo "Aviso: no existe el directorio $dir, se omite compresión."
        fi
    done
    echo "Compresión completada."
}

press_any_key() {
    echo -e "\nPulsa cualquier tecla para volver al menú..."
    read -n 1 -s -r
}

# --- MENÚ PRINCIPAL ---
show_menu() {
    cd "$PROJECT_DIR" || exit 1
    clear
    echo -e "--------------------------------------------------"
    echo -e "   1) Crear estructura de carpetas para macOS"
    echo -e "   2) Compilar y empaquetar para macOS"
    echo -e "--------------------------------------------------"
    echo -e "   3) Crear estructura de carpetas para Windows"
    echo -e "   4) Compilar y empaquetar para Windows"
    echo -e "--------------------------------------------------"
    echo -e "   5) Crear estructura de carpetas para Linux"
    echo -e "   6) Compilar y empaquetar para Linux"
    echo -e "--------------------------------------------------"
    echo -e "   8) Crear estructura de carpetas para TODOS"
    echo -e "   9) Compilar y empaquetar para TODOS"
    echo -e "--------------------------------------------------"
    echo -e "   X) Salir"
    read -p "   >> Introduce tu elección: " choice
    echo ""

    case "$choice" in
        1) carpeta_macOS; press_any_key ;;
        2) actualizar_macOS; press_any_key ;;
        3) carpeta_windows; press_any_key ;;
        4) actualizar_windows; press_any_key ;;
        5) carpeta_linux; press_any_key ;;
        6) actualizar_linux; press_any_key ;;
        8) carpeta_todos; press_any_key ;;
        9) actualizar_todos; comprimir_todos; press_any_key ;;
        X|x) echo "Saliendo... ¡Hasta pronto!"; exit 0 ;;
        *) echo "Opción inválida. Por favor, intenta de nuevo."; sleep 2 ;;
    esac
}

# Bucle infinito para mantener el menú corriendo
while true; do
    show_menu
done