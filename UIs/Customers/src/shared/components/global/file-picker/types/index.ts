import type { ChangeEvent, InputHTMLAttributes, RefObject } from 'react';

export type FileMetadata = {
  name: string;
  size: number;
  type: string;
  url: string;
  id: string;
};

export type FileWithPreview = {
  id: string;
  file?: File | FileMetadata;
  name: string;
  url: string;
  type: string;
  publicId?: string;
  isPrimary?: boolean;
};

export type FilePickerOptions = {
  files: FileWithPreview[];
  onFilesChange: (files: FileWithPreview[]) => void;
  onFilesAdded?: (addedFiles: FileWithPreview[]) => void;

  maxFiles?: number;
  maxSize?: number;
  accept?: string;
  multiple?: boolean;
};

export type FilePickerState = {
  errors: string[];
};

export type FilePickerActions = {
  addFiles: (files: FileList | File[]) => void;
  removeFile: (id: string) => void;
  setPrimaryFile: (id: string) => void;
  clearFiles: () => void;
  clearErrors: () => void;
  handleFileChange: (e: ChangeEvent<HTMLInputElement>) => void;
  openFileDialog: () => void;
  getInputProps: (
    props?: InputHTMLAttributes<HTMLInputElement>
  ) => InputHTMLAttributes<HTMLInputElement> & {
    ref: RefObject<HTMLInputElement | null>;
  };
};
