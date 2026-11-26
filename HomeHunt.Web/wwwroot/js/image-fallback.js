const FALLBACK_SELECTOR = "img[data-fallback-src]";

const useFallback = (image) => {
    const fallback = image.dataset.fallbackSrc;

    delete image.dataset.fallbackSrc;
    image.src = fallback;
};

const handleError = (event) => {
    if (event.target.matches?.(FALLBACK_SELECTOR)) {
        useFallback(event.target);
    }
};

document.addEventListener("error", handleError, true);

document.querySelectorAll(FALLBACK_SELECTOR).forEach((image) => {
    if (image.complete && image.naturalWidth === 0) {
        useFallback(image);
    }
});
