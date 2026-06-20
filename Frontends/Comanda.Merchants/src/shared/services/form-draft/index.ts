import { LocalStorageService } from '../local-storage';

export const formDraftService = {
  get: <T>(key: string): T | null => {
    const data = LocalStorageService.getItem<T>(`form-draft:${key}`);
    return data ?? null;
  },

  write: <T>(key: string, value: T): void => {
    LocalStorageService.setItem(`form-draft:${key}`, value);
  },

  clear: (key: string): void => {
    LocalStorageService.removeItem(`form-draft:${key}`);
  }
};
