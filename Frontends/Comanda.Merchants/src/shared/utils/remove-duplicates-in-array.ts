export function removeDuplicatesInArray<T>(array: T[], key: keyof T) {
  const uniqueArray = array.filter((item, index) => {
    return array.findIndex((t) => t[key] === item[key]) === index;
  });

  return uniqueArray;
}
