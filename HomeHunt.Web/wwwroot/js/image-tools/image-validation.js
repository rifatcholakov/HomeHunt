const MEGABYTE = 1024 * 1024;

export const formatSize = (bytes) => bytes < MEGABYTE
    ? `${Math.max(1, Math.round(bytes / 1024))} KB`
    : `${(bytes / MEGABYTE).toFixed(1)} MB`;

export const fileKey = (file) => `${file.name}|${file.size}|${file.lastModified}`;

export const validateImages = ({ activeCount, files, maxFiles, maxSize }) => {
    const problems = [];

    if (activeCount > maxFiles) {
        problems.push(`A property can have at most ${maxFiles} images.`);
    }

    files
        .filter((file) => file.size > maxSize)
        .forEach((file) => problems.push(`"${file.name}" is larger than ${maxSize / MEGABYTE} MB.`));

    files
        .filter((file) => !file.type.startsWith("image/"))
        .forEach((file) => problems.push(`"${file.name}" is not an image.`));

    return problems;
};
