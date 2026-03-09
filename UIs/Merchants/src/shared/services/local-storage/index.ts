function setLocalStorage<T>(key: string, value: T): void {
  if (!isLocalStorageAvailable()) throw new Error('storage is not available');

  try {
    const serialized = JSON.stringify(value);
    localStorage.setItem(key, serialized);
  } catch {
    return;
  }
}

function getLocalStorage<T>(key: string): T | undefined {
  if (!isLocalStorageAvailable()) throw new Error('storage is not available');

  try {
    const item = localStorage.getItem(key);

    return item ? (JSON.parse(item) as T) : undefined;
  } catch {
    return undefined;
  }
}

const removeLocalStorage = (key: string): void => {
  if (!isLocalStorageAvailable()) throw new Error('storage is not available');

  try {
    localStorage.removeItem(key);
  } catch {
    return;
  }
};

const clearLocalStorage = (): void => {
  if (!isLocalStorageAvailable()) throw new Error('storage is not available');

  try {
    localStorage.clear();
  } catch {
    return;
  }
};

const isLocalStorageAvailable = (): boolean => {
  return (
    typeof window !== 'undefined' && typeof window.localStorage !== 'undefined'
  );
};

export const LocalStorageService = {
  getItem: getLocalStorage,
  setItem: setLocalStorage,
  removeItem: removeLocalStorage,
  clear: clearLocalStorage
};
