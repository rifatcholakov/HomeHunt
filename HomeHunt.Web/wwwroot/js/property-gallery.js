const gallery = document.querySelector(".property-gallery");

if (gallery && typeof GLightbox === "function") {
    const lightbox = GLightbox({
        selector: ".glightbox",
        loop: true,
        zoomable: true,
        draggable: true,
        touchNavigation: true,
        openEffect: "zoom",
        closeEffect: "fade",
        slideEffect: "slide"
    });

    gallery.querySelector("[data-gallery-open]")?.addEventListener("click", () => lightbox.openAt(0));
}
