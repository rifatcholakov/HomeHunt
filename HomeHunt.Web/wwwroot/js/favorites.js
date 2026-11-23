const FORM_SELECTOR = ".favorite-form";
const FAVORITES_PAGE_SELECTOR = "[data-favorites-page]";
const FAVORITES_COUNT_SELECTOR = "[data-favorites-count]";
const CARD_SELECTOR = ".col";

const ICON_FAVORITE = "♥";
const ICON_NOT_FAVORITE = "♡";
const LABEL_ADD = "Add to favorites";
const LABEL_REMOVE = "Remove from favorites";

const isFavoritesPage = () => document.querySelector(FAVORITES_PAGE_SELECTOR) !== null;

const updateButton = (form, isFavorite) => {
    const button = form.querySelector("button");
    const label = isFavorite ? LABEL_REMOVE : LABEL_ADD;

    form.dataset.isFavorite = String(isFavorite);
    form.action = isFavorite ? form.dataset.removeUrl : form.dataset.addUrl;

    button.textContent = isFavorite ? ICON_FAVORITE : ICON_NOT_FAVORITE;
    button.classList.toggle("text-danger", isFavorite);
    button.title = label;
    button.setAttribute("aria-label", label);
    button.disabled = false;
};

const removeCard = (form) => {
    const card = form.closest(CARD_SELECTOR);
    const list = card.parentElement;

    card.remove();

    const remaining = list.querySelectorAll(CARD_SELECTOR).length;

    if (remaining === 0) {
        location.reload();
        return;
    }

    const counter = document.querySelector(FAVORITES_COUNT_SELECTOR);

    if (counter) {
        counter.textContent = `${remaining} saved properties`;
    }
};

const sendRequest = (form) => fetch(form.action, {
    method: "POST",
    body: new FormData(form),
    headers: { "X-Requested-With": "XMLHttpRequest" }
});

const handleSubmit = async (event) => {
    const form = event.target.closest(FORM_SELECTOR);

    if (!form) {
        return;
    }

    event.preventDefault();

    const wasFavorite = form.dataset.isFavorite === "true";
    form.querySelector("button").disabled = true;

    try {
        const response = await sendRequest(form);

        if (!response.ok) {
            form.submit();
            return;
        }

        if (wasFavorite && isFavoritesPage()) {
            removeCard(form);
            return;
        }

        updateButton(form, !wasFavorite);
    } catch {
        form.submit();
    }
};

document.addEventListener("submit", handleSubmit);
