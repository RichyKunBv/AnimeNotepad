#!/bin/bash

# Es para hacer las compilaciones desde Linux, realmente ignorenlo ya que es algo mas para mi que para ustedes, nada mas para mas facilidad
# Tambien como yo trabajo en Linux los comandos son GNU/Linux y no funcionarian bien en macOS (sin adaptar) y mucho menos en Windows
# Ermano que tardado es hacer scripts :VVVV

if [[ "$(uname)" != "Linux" ]]; then
    echo "Aviso: Linux no detectado. Si lo estas usando en macOS o Windows es probable que no funcione correctamente. Es bajo tu responsabilidad. Ya que esta diseñado para Linux."

    echo -e "\nPulsa cualquier tecla para continuar..."
    read -n 1 -s -r
fi

# Definir las rutas
PROJECT_DIR="$HOME/AnimeNotepad"

MACOS_ARM_APP="$HOME/AnimeNotepadARM64macOS/AnimeNotepad.app"
MACOS_X64_APP="$HOME/AnimeNotepadX86_64macOS/AnimeNotepad.app"

WIN_X64_DIR="$HOME/AnimeNotepadX86_64Windows/"
WIN_ARM_DIR="$HOME/AnimeNotepadARM64Windows/"

LINUX_X64_DIR="$HOME/AnimeNotepadX86_64Linux/"
LINUX_ARM_DIR="$HOME/AnimeNotepadARM64Linux/"

# Para macOS
carpeta_macOS() {
    echo "Creando carpetas para macOS..."
    mkdir -p "$MACOS_X64_APP/Contents/"{MacOS,Resources}
    mkdir -p "$MACOS_ARM_APP/Contents/"{MacOS,Resources}
    
    # IMPORTANTE: Copiar el Info.plist base para tener algo que editar después
    cp "$PROJECT_DIR/src/Info.plist.template" "$MACOS_X64_APP/Contents/Info.plist" 2>/dev/null || echo "Aviso: No se encontró Info.plist.template base"
    cp "$PROJECT_DIR/src/Info.plist.template" "$MACOS_ARM_APP/Contents/Info.plist" 2>/dev/null
    
    echo "Carpetas de macOS creadas"
}

actualizar_macOS() {
    echo "=== Iniciando compilación de AnimeNotepad para macOS ==="

    # 1. Extraer la versión del archivo .csproj usando awk
    VERSION=$(awk -F'[><]' '/<Version>/{print $3}' "$PROJECT_DIR/src/AnimeNotepad.csproj")

    if [ -z "$VERSION" ]; then
        echo "Error: No se pudo encontrar la etiqueta <Version> en el .csproj"
        return
    fi

    echo "Versión detectada: $VERSION"
    echo "Publicando binarios..."

    # 2. Compilar ambas arquitecturas
    cd "$PROJECT_DIR/src" || exit 1
    dotnet publish -c Release -r osx-arm64 --self-contained -p:PublishSingleFile=true
    dotnet publish -c Release -r osx-x64 --self-contained -p:PublishSingleFile=true

    echo "Copiando archivos y actualizando Info.plist..."

    # 3. Procesar versión ARM64
    cp -a "$PROJECT_DIR/src/bin/Release/net10.0/osx-arm64/publish/." "$MACOS_ARM_APP/Contents/MacOS/"
    cp "$PROJECT_DIR/src/Assets/icon.icns" "$MACOS_ARM_APP/Contents/Resources/" 2>/dev/null
    
    # En Linux no tenemos plutil, usamos Python3 (plistlib nativo) para editar el plist de forma segura
    python3 -c "import plistlib, sys; p=sys.argv[1]; d=plistlib.load(open(p,'rb')); d['CFBundleVersion']=sys.argv[2]; d['CFBundleShortVersionString']=sys.argv[2]; plistlib.dump(d,open(p,'wb'))" "$MACOS_ARM_APP/Contents/Info.plist" "$VERSION" 2>/dev/null || echo "Aviso: No se pudo actualizar Info.plist de ARM64"

    # 4. Procesar versión X86_64
    cp -a "$PROJECT_DIR/src/bin/Release/net10.0/osx-x64/publish/." "$MACOS_X64_APP/Contents/MacOS/"
    cp "$PROJECT_DIR/src/Assets/icon.icns" "$MACOS_X64_APP/Contents/Resources/" 2>/dev/null
    
    python3 -c "import plistlib, sys; p=sys.argv[1]; d=plistlib.load(open(p,'rb')); d['CFBundleVersion']=sys.argv[2]; d['CFBundleShortVersionString']=sys.argv[2]; plistlib.dump(d,open(p,'wb'))" "$MACOS_X64_APP/Contents/Info.plist" "$VERSION" 2>/dev/null || echo "Aviso: No se pudo actualizar Info.plist de x64"

    echo "Aviso: Omitiendo firmado de aplicaciones..."
    echo "Las herramientas 'xattr' y 'codesign' son exclusivas de macOS y no están disponibles en Linux."
    echo "La app ha sido empaquetada, pero tendrá que ser firmada manualmente en una Mac si es necesario."

    # 5 & 6 (Omitidos en Linux)
    # xattr -cr "$MACOS_ARM_APP"
    # codesign --force --deep --sign - "$MACOS_ARM_APP"
    # xattr -cr "$MACOS_X64_APP"
    # codesign --force --deep --sign - "$MACOS_X64_APP"

    echo "=== ¡Listo! AnimeNotepad v$VERSION empaquetado para macOS (x64/ARM64) desde Linux ==="
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

    cd "$PROJECT_DIR/src" || exit 1
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

    cd "$PROJECT_DIR/src" || exit 1
    # Compilar para Linux ARM64
    dotnet publish -c Release -r linux-arm64 --self-contained -p:IncludeNativeLibrariesForSelfExtract=true -p:DebugType=None -p:PublishSingleFile=true    
    # Compilar para Linux x64
    dotnet publish -c Release -r linux-x64 --self-contained -p:IncludeNativeLibrariesForSelfExtract=true -p:DebugType=None -p:PublishSingleFile=true

    echo "Copiando archivos y actualizando .desktop..."

    # Procesar versión ARM64
    cp -a "$PROJECT_DIR/src/bin/Release/net10.0/linux-arm64/publish/." "$LINUX_ARM_DIR/"
    cp "$PROJECT_DIR/src/Assets/icon.png" "$LINUX_ARM_DIR/" 2>/dev/null
    
    # En Linux, GNU sed no requiere la extensión .bak para editar en el lugar de forma segura
    if [ -f "$LINUX_ARM_DIR/AnimeNotepad.desktop" ]; then
        sed -i "s/^Version=.*/Version=$VERSION/" "$LINUX_ARM_DIR/AnimeNotepad.desktop"
    fi

    # Procesar versión X86_64
    cp -a "$PROJECT_DIR/src/bin/Release/net10.0/linux-x64/publish/." "$LINUX_X64_DIR/"
    cp "$PROJECT_DIR/src/Assets/icon.png" "$LINUX_X64_DIR/" 2>/dev/null
    
    if [ -f "$LINUX_X64_DIR/AnimeNotepad.desktop" ]; then
        sed -i "s/^Version=.*/Version=$VERSION/" "$LINUX_X64_DIR/AnimeNotepad.desktop"
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

    # Limpiar zips anteriores si existen
    for file in "AnimeNotepadARM64macOS.zip" "AnimeNotepadX86_64macOS.zip" "AnimeNotepadARM64Windows.zip" "AnimeNotepadX86_64Windows.zip" "AnimeNotepadARM64Linux.zip" "AnimeNotepadX86_64Linux.zip"; do
        if [ -f "$HOME/$file" ]; then
            echo "El archivo $file ya existe, eliminando..."
            rm "$HOME/$file"
        fi
    done

    cd "$HOME" || exit 1
    for dir in AnimeNotepadARM64macOS AnimeNotepadX86_64macOS AnimeNotepadARM64Windows AnimeNotepadX86_64Windows AnimeNotepadARM64Linux AnimeNotepadX86_64Linux; do
        if [ -d "$dir" ]; then
            zip -rq "${dir}.zip" "$dir"
            echo "Directorio $dir comprimido con éxito."
        else
            echo "Aviso: no existe el directorio $dir, se omite compresión."
        fi
    done
    echo "=== Compresión completada ==="
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