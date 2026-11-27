const MAX_DIMENSION = 1600;
const JPEG_QUALITY = 0.85;
const OPTIMIZE_ABOVE_BYTES = 500 * 1024;

const canvasToBlob = (canvas) => new Promise((resolve) => canvas.toBlob(resolve, "image/jpeg", JPEG_QUALITY));

export const optimizeImage = async (file) => {
    if (file.type === "image/gif" || !file.type.startsWith("image/")) {
        return file;
    }

    try {
        const bitmap = await createImageBitmap(file);
        const scale = Math.min(1, MAX_DIMENSION / Math.max(bitmap.width, bitmap.height));

        if (scale === 1 && file.size <= OPTIMIZE_ABOVE_BYTES) {
            bitmap.close();
            return file;
        }

        const canvas = document.createElement("canvas");
        canvas.width = Math.round(bitmap.width * scale);
        canvas.height = Math.round(bitmap.height * scale);

        const context = canvas.getContext("2d");
        context.fillStyle = "#fff";
        context.fillRect(0, 0, canvas.width, canvas.height);
        context.drawImage(bitmap, 0, 0, canvas.width, canvas.height);
        bitmap.close();

        const blob = await canvasToBlob(canvas);

        if (!blob || blob.size >= file.size) {
            return file;
        }

        return new File([blob], file.name.replace(/\.[^.]+$/, "") + ".jpg", { type: "image/jpeg", lastModified: file.lastModified });
    } catch {
        return file;
    }
};
