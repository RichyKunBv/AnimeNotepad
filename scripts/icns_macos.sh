#!/bin/bash

set -euo pipefail

if [ "$#" -ne 1 ]; then
    echo "Error: Debes proporcionar la ruta al archivo PNG de entrada."
    echo "Uso: $0 <ruta_al_png_original>"
    exit 1
fi

INPUT_PNG="$1"

# Obtener la ruta absoluta al directorio raíz del proyecto
PROJECT_ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
OUTPUT_DIR="${PROJECT_ROOT}/src/Assets/macOS icns tests"
ICONSET_DIR="${OUTPUT_DIR}/MiIcono.iconset"

echo "Generando iconos en ${OUTPUT_DIR}..."

# Crear el directorio de salida si no existe
mkdir -p "${OUTPUT_DIR}"
mkdir -p "${ICONSET_DIR}"

sips -z 16 16 "${INPUT_PNG}" --out "${ICONSET_DIR}/icon_16x16.png"
sips -z 32 32 "${INPUT_PNG}" --out "${ICONSET_DIR}/icon_16x16@2x.png"
sips -z 32 32 "${INPUT_PNG}" --out "${ICONSET_DIR}/icon_32x32.png"
sips -z 64 64 "${INPUT_PNG}" --out "${ICONSET_DIR}/icon_32x32@2x.png"
sips -z 128 128 "${INPUT_PNG}" --out "${ICONSET_DIR}/icon_128x128.png"
sips -z 256 256 "${INPUT_PNG}" --out "${ICONSET_DIR}/icon_128x128@2x.png"
sips -z 256 256 "${INPUT_PNG}" --out "${ICONSET_DIR}/icon_256x256.png"
sips -z 512 512 "${INPUT_PNG}" --out "${ICONSET_DIR}/icon_256x256@2x.png"
sips -z 512 512 "${INPUT_PNG}" --out "${ICONSET_DIR}/icon_512x512.png"
sips -z 1024 1024 "${INPUT_PNG}" --out "${ICONSET_DIR}/icon_512x512@2x.png"

echo "Empaquetando en .icns..."
iconutil -c icns "${ICONSET_DIR}" --out "${OUTPUT_DIR}/MiIcono.icns"

# Limpiar carpeta temporal
rm -R "${ICONSET_DIR}"

echo "¡Icono generado exitosamente en: ${OUTPUT_DIR}/MiIcono.icns!"


# Ejemplo de uso:
# ./scripts/icns_macos.sh "/ruta/cualquiera/hacia/tu/imagen_original.png"
