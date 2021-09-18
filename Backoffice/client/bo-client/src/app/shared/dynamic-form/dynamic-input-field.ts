import {FormControl} from "@angular/forms";

export enum InputFieldType {
  TEXT = 'text',
  CHECKBOX = 'checkbox',
  NUMBER = 'number',
  DATE = 'date',
}

export interface DynamicInputField {
  type: InputFieldType,
  label: string,
  control: FormControl,
  name: string
}
