export const createElement = (tag, className = "", text = "") => {
    const element = document.createElement(tag);

    element.className = className;

    if (text) {
        element.textContent = text;
    }

    return element;
};

export const createHidden = (name, value) => {
    const hidden = document.createElement("input");

    hidden.type = "hidden";
    hidden.name = name;
    hidden.value = value;

    return hidden;
};
