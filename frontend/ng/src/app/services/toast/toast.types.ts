import TOAST_TYPE from '@/app/constants/toast-types.constants';

export type ToastType = (typeof TOAST_TYPE)[keyof typeof TOAST_TYPE];

export interface ToastMessage {
  id: number;
  type: ToastType;
  message: string;
}
