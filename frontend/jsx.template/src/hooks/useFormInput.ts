import { useState } from 'react';

export function useFormInput<T = string>(initialValue: T) {
  const [value, setValue] = useState<T>(initialValue);

  function handleChange(e: React.ChangeEvent<HTMLInputElement>) {
    setValue(e.target.value as T);
  }

  return {
    value,
    setValue,
    onChange: handleChange
  };
}
