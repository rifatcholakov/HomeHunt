import { createElement } from "./dom.js";

const createButton = (text, label, onClick) => {
    const button = createElement("button", "btn btn-sm flex-fill", text);

    button.type = "button";
    button.setAttribute("aria-label", label);
    button.addEventListener("click", onClick);

    return button;
};

export const buildTile = (tile, { source, name, fallback, onMakeCover, onRemove }) => {
    const column = createElement("div", "col");
    const card = createElement("figure", "card image-card h-100 mb-0 border-2 shadow-sm");
    const media = createElement("div", "position-relative");
    const ratio = createElement("div", "ratio ratio-4x3");
    const image = createElement("img", "image-fit");
    const badge = createElement("span", "badge position-absolute top-0 start-0 m-2");
    const status = createElement("span", "badge bg-secondary position-absolute top-50 start-50 translate-middle", "Will be removed");
    const body = createElement("figcaption", "card-body p-2");
    const caption = createElement("p", "card-text small text-muted text-truncate mb-2", name);
    const actions = createElement("div", "d-flex gap-2");

    image.alt = name;
    image.src = source;
    image.draggable = false;
    image.dataset.fallbackSrc = fallback;

    caption.title = name;

    tile.coverButton = createButton("Make cover", `Make ${name} the cover image`, () => onMakeCover(tile));
    tile.removeButton = createButton("Remove", `Remove ${name}`, () => onRemove(tile));

    ratio.append(image);
    media.append(ratio, badge, status);
    actions.append(tile.coverButton, tile.removeButton);
    body.append(caption, actions);
    card.append(media, body);
    column.append(card);

    Object.assign(tile, { element: column, card, image, badge, status });

    return tile;
};

export const updateTile = (tile, position) => {
    const isCover = position === 0;

    tile.element.dataset.removed = String(tile.removed);
    tile.card.classList.toggle("border-primary", isCover);
    tile.image.classList.toggle("opacity-50", tile.removed);

    tile.badge.hidden = tile.removed;
    tile.badge.className = `badge position-absolute top-0 start-0 m-2 ${isCover ? "bg-primary" : "bg-dark bg-opacity-75"}`;
    tile.badge.textContent = isCover ? "Cover" : `${position + 1}`;

    tile.status.hidden = !tile.removed;

    tile.coverButton.hidden = isCover || tile.removed;
    tile.coverButton.classList.add("btn-outline-primary");

    tile.removeButton.textContent = tile.removed ? "Undo" : "Remove";
    tile.removeButton.classList.toggle("btn-outline-danger", !tile.removed);
    tile.removeButton.classList.toggle("btn-outline-secondary", tile.removed);
};
