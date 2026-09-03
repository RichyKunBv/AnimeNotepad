#!/bin/bash

set -euo pipefail

# es para hacer una mini sincronizacion, realmente ignorenlo ya que es algo mas
# para mi que para ustedes, nada mas para mas agilidad
# El script es solo para Linux y MacOS, no es compatible con Windows.

nombre="AnimeNotepad"
repo_url="https://github.com/RichyKunBv/$nombre.git"
repos_base="$HOME/Repos"
old_location="$HOME/$nombre"
ubicacion=""

ensure_repos_dir() {
    if [ ! -d "$repos_base" ]; then
        echo "Creando carpeta predeterminada de repositorios en $repos_base..."
        mkdir -p "$repos_base"
    fi
}

detect_repository_location() {
    if [ -d "$repos_base/$nombre/.git" ]; then
        ubicacion="$repos_base/$nombre"
    elif [ -d "$old_location/.git" ]; then
        ensure_repos_dir

        if [ -e "$repos_base/$nombre" ]; then
            echo "Existe un repositorio en $old_location y también en $repos_base/$nombre."
            echo "Por favor mueve manualmente el contenido o elimina uno de ellos."
            exit 1
        fi

        echo "Moviendo repositorio existente de $old_location a $repos_base/$nombre..."
        mv "$old_location" "$repos_base/"
        ubicacion="$repos_base/$nombre"
    else
        ubicacion="$repos_base/$nombre"
    fi
}

detect_repository_location

if [ -d "$ubicacion" ]; then
    cd "$ubicacion"
fi

check_branch() {
    local current=$(git rev-parse --abbrev-ref HEAD 2>/dev/null)
    if [ "$current" = "HEAD" ]; then
        echo "⚠️  ADVERTENCIA: No estás en una rama válida (Estás en 'detached HEAD' o atascado en un rebase)."
        echo "Por favor, soluciona esto o usa la opción 3 para cambiar a una rama válida (ej. main) antes de actualizar."
        return 1
    fi
    return 0
}

descargar() {
    check_branch || return
    echo "Obteniendo últimos cambios del repositorio..."
    local branch=$(git rev-parse --abbrev-ref HEAD)
    if git pull origin "$branch" --rebase; then
        echo "Actualización exitosa."
    else
        echo "Error al obtener los cambios. Revisa tu conexión o posibles conflictos."
    fi
}

publicar() {
    check_branch || return
    
    if [ -n "$(git status --porcelain)" ]; then
        git add .
        read -e -p "   >> Introduce el mensaje del commit: " mensaje

        if [ -z "$mensaje" ]; then
            echo "El mensaje no puede estar vacío. Cancelando publicación..."
            return
        fi

        git commit -m "$mensaje"
    else
        echo "El código ya está empaquetado (commit). Intentando subir a la nube..."
    fi

    local branch=$(git rev-parse --abbrev-ref HEAD)
    git pull origin "$branch" --rebase

    if git push origin "$branch"; then
        echo "Publicación exitosa."
    else
        echo "Error al publicar. Revisa el mensaje de error y vuelve a intentarlo."
        return
    fi
}

configurar() {
    clear
    echo "=== Clonar y Configurar Entorno de $nombre ==="
    ensure_repos_dir

    if [ ! -d "$ubicacion/.git" ]; then
        echo -e "\nDescargando el código en $repos_base..."
        cd "$repos_base" || return
        git clone "$repo_url" "$nombre"
    else
        echo -e "\nLa carpeta ya existe en $ubicacion. Saltando la clonación..."
    fi

    cd "$ubicacion" || { echo "Error al entrar a la carpeta"; return; }

    echo ""
    echo -e "\nConfigurando editor..."
    git config --global core.editor "nano"

    echo -e "\n¡Entorno de $nombre configurado y listo para programar!"
    echo "Nota: Al hacer tu primer 'push', te va a pedir tus credenciales como nombre de usuario, correo y contraseña (la contraseña no es la de tu cuenta, es un Token de Acceso Personal que se genera en las configuraciones de GitHub)."
}

clonar() {
    ensure_repos_dir

    if [ -d "$ubicacion/.git" ]; then
        echo "El repositorio ya está clonado en $ubicacion."
        return
    fi

    echo -e "\nClonando el repositorio en $repos_base..."
    cd "$repos_base" || return
    git clone "$repo_url" "$nombre"
    cd "$ubicacion" || return
}

cambiar_rama() {
    if [ ! -d "$ubicacion/.git" ]; then
        echo "El repositorio no ha sido clonado todavía."
        return
    fi
    
    cd "$ubicacion" || return

    echo -e "\nRamas disponibles:"
    git --no-pager branch -a || true
    echo ""
    read -e -p "   >> Introduce el nombre de la rama: " nueva_rama
    
    if [ -n "$nueva_rama" ]; then
        if git checkout "$nueva_rama"; then
            echo "Rama cambiada a $nueva_rama exitosamente."
        else
            echo "Error al cambiar de rama. Verifica el nombre."
        fi
    else
        echo "Operación cancelada."
    fi
}

solucionar_errores() {
    clear
    echo "=== Mini Solucionador de Errores Git ==="
    echo "Selecciona el problema que quieres resolver:"
    echo "  1) Estoy atascado actualizando (Cancelar Rebase/Merge y volver atrás)"
    echo "  2) Quiero deshacer todos mis cambios locales y limpiar"
    echo "  3) Mis archivos bloquean una actualización (Guardar cambios y Abortar)"
    echo "  4) Ya resolví los conflictos de código a mano (Continuar actualización)"
    echo "  X) Volver al menú principal"
    read -e -p "   >> Introduce tu elección: " err_choice
    echo ""

    case "$err_choice" in
        1)
            git rebase --abort 2>/dev/null || echo "No había rebase en progreso."
            git merge --abort 2>/dev/null || echo "No había merge en progreso."
            echo "Hecho."
            ;;
        2)
            read -e -p "¿Estás seguro? Perderás TODO el trabajo no guardado. (s/n): " confirm
            if [[ "$confirm" == "s" || "$confirm" == "S" ]]; then
                git reset --hard HEAD
                git clean -fd
                echo "Cambios descartados y repositorio limpio."
            else
                echo "Operación cancelada."
            fi
            ;;
        3)
            git add .
            git stash
            git rebase --abort 2>/dev/null || true
            echo "Archivos guardados en el 'stash' y rebase cancelado."
            ;;
        4)
            echo "Marcando archivos como resueltos y continuando..."
            git add .
            GIT_EDITOR=true git rebase --continue 2>/dev/null || echo "No había rebase pendiente."
            GIT_EDITOR=true git merge --continue 2>/dev/null || true
            echo "¡Listo! Proceso continuado."
            ;;
        X|x) return ;;
        *) echo "Opción inválida." ;;
    esac
}

press_any_key() {
    echo -e "\nPulsa cualquier tecla para volver al menú..."
    read -n 1 -s -r
}

show_menu() {
    clear
    local current_branch="Ninguna"
    if [ -d "$ubicacion/.git" ]; then
        cd "$ubicacion" || return
        current_branch=$(git rev-parse --abbrev-ref HEAD 2>/dev/null || echo "Desconocida")
    fi
    
    echo -e "=== Repo: $nombre | Rama: $current_branch ==="
    echo -e "   1) Actualizar local (Pull)"
    echo -e "   2) Actualizar el repo (Push)"
    echo -e "   3) Cambiar rama"
    echo -e "   4) Mini Solucionador de Errores"
    echo -e "   0) Configurar"
    echo -e "   9) Clonar"
    echo -e "   X) Salir"
    read -e -p "   >> Introduce tu elección: " choice
    echo ""

    case "$choice" in
        1) descargar; press_any_key ;;
        2) publicar; press_any_key ;;
        3) cambiar_rama; press_any_key ;;
        4) solucionar_errores; press_any_key ;;
        0) configurar; press_any_key ;;
        9) clonar; press_any_key ;;
        X|x) echo "Saliendo... ¡Hasta pronto!"; exit 0 ;;
        *) echo "Opción inválida. Por favor, intenta de nuevo."; sleep 2 ;;
    esac
}

while true; do
    show_menu
done
