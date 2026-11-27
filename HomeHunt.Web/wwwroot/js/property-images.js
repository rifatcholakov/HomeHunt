import { createHidden } from "./image-tools/dom.js";
import { optimizeImage } from "./image-tools/image-optimizer.js";
import { buildTile, updateTile } from "./image-tools/image-tile.js";
import { fileKey, formatSize, validateImages } from "./image-tools/image-validation.js";

const EXISTING_TOKEN = "e";
const NEW_TOKEN = "n";

const manager = document.querySelector("[data-image-manager]");

if (manager) {
    const input = manager.querySelector("input[type=file]");
    const dropzone = manager.querySelector("[data-dropzone]");
    const grid = manager.querySelector("[data-image-tiles]");
    const errors = manager.querySelector("[data-image-errors]");
    const summary = manager.querySelector("[data-image-summary]");
    const orderInputs = manager.querySelector("[data-order-inputs]");
    const removeInputs = manager.querySelector("[data-remove-inputs]");
    const maxFiles = Number(manager.dataset.maxFiles);
    const maxSize = Number(manager.dataset.maxSize);
    const form = input.form;

    const tiles = [];
    let isPreparing = false;
    let isSubmitting = false;

    const existingTiles = () => tiles.filter((tile) => tile.kind === "existing");
    const newTiles = () => tiles.filter((tile) => tile.kind === "new");
    const activeTiles = () => tiles.filter((tile) => !tile.removed);

    const syncForm = () => {
        const transfer = new DataTransfer();
        const fresh = newTiles();

        fresh.forEach((tile) => transfer.items.add(tile.file));
        input.files = transfer.files;

        orderInputs.replaceChildren(...activeTiles().map((tile) =>
            createHidden("ImageOrder", tile.kind === "existing"
                ? `${EXISTING_TOKEN}${tile.id}`
                : `${NEW_TOKEN}${fresh.indexOf(tile)}`)));

        removeInputs.replaceChildren(...existingTiles()
            .filter((tile) => tile.removed)
            .map((tile) => createHidden("RemoveImageIds", tile.id)));
    };

    const render = () => {
        const active = activeTiles();
        const added = newTiles().length;
        const problems = validateImages({
            activeCount: active.length,
            files: newTiles().map((tile) => tile.file),
            maxFiles,
            maxSize
        });

        tiles.forEach((tile) => updateTile(tile, active.indexOf(tile)));
        grid.replaceChildren(...tiles.map((tile) => tile.element));
        syncForm();

        errors.textContent = problems.join(" ");
        input.setCustomValidity(problems.length > 0 ? problems[0] : "");

        summary.textContent = isPreparing
            ? "Preparing images..."
            : `${active.length} of ${maxFiles} images${added > 0 ? `, ${added} new` : ""}.`;
    };

    const tileOptions = (source, name) => ({
        source,
        name,
        fallback: manager.dataset.fallback,
        onMakeCover: (tile) => {
            tiles.splice(tiles.indexOf(tile), 1);
            tiles.unshift(tile);
            render();
        },
        onRemove: (tile) => {
            if (tile.kind === "existing") {
                tile.removed = !tile.removed;
            } else {
                tiles.splice(tiles.indexOf(tile), 1);
            }

            render();
        }
    });

    const addFiles = async (files) => {
        const known = new Set(newTiles().map((tile) => tile.key));
        const fresh = files.filter((file) => file.type.startsWith("image/") && !known.has(fileKey(file)));

        if (fresh.length === 0) {
            render();
            return;
        }

        isPreparing = true;
        render();

        const prepared = await Promise.all(fresh.map(async (file) => ({ key: fileKey(file), file: await optimizeImage(file) })));

        prepared.forEach(({ key, file }) => {
            const url = URL.createObjectURL(file);
            const tile = buildTile({ kind: "new", key, file, removed: false }, tileOptions(url, `${file.name} (${formatSize(file.size)})`));

            tile.image.addEventListener("load", () => URL.revokeObjectURL(url), { once: true });
            tiles.push(tile);
        });

        isPreparing = false;
        render();
    };

    const lockSubmitButton = () => {
        const button = form.querySelector("button[type=submit]");

        isSubmitting = true;

        if (button) {
            button.disabled = true;
            button.replaceChildren(
                Object.assign(document.createElement("span"), { className: "spinner-border spinner-border-sm me-2", role: "status" }),
                document.createTextNode(newTiles().length > 0 ? "Uploading..." : "Saving..."));
        }
    };

    manager.querySelectorAll("[data-existing-image]").forEach((placeholder) => {
        const tile = { kind: "existing", id: placeholder.dataset.id, removed: placeholder.dataset.removed === "true" };

        tiles.push(buildTile(tile, tileOptions(placeholder.dataset.src, "Property image")));
        placeholder.remove();
    });

    if (typeof Sortable === "function") {
        Sortable.create(grid, {
            animation: 180,
            delay: 120,
            delayOnTouchOnly: true,
            filter: "[data-removed=true]",
            preventOnFilter: false,
            ghostClass: "sortable-ghost",
            chosenClass: "sortable-chosen",
            onEnd: () => {
                const byElement = new Map(tiles.map((tile) => [tile.element, tile]));

                tiles.splice(0, tiles.length, ...[...grid.children].map((element) => byElement.get(element)));
                render();
            }
        });
    }

    input.addEventListener("change", () => addFiles([...input.files]));

    const stop = (event) => {
        event.preventDefault();
        event.stopPropagation();
    };

    ["dragenter", "dragover"].forEach((name) => dropzone.addEventListener(name, (event) => {
        stop(event);
        dropzone.classList.add("is-dragover");
    }));

    ["dragleave", "drop"].forEach((name) => dropzone.addEventListener(name, (event) => {
        stop(event);
        dropzone.classList.remove("is-dragover");
    }));

    dropzone.addEventListener("drop", (event) => addFiles([...event.dataTransfer.files]));

    form?.addEventListener("submit", (event) => {
        if (isSubmitting || isPreparing || !input.checkValidity()) {
            event.preventDefault();

            if (!input.checkValidity()) {
                errors.scrollIntoView({ behavior: "smooth", block: "center" });
            }

            return;
        }

        setTimeout(() => {
            if (!event.defaultPrevented) {
                lockSubmitButton();
            }
        }, 0);
    });

    render();
}
