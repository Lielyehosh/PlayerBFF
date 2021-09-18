import {FormControl} from "@angular/forms";

export enum InputFieldType {
  TEXT = "text",
  CHECKBOX = "checkbox",
  NUMBER = "number"
}

export interface DynamicInputField {
  id: string,
  type: InputFieldType,
  label: string,
  control: FormControl,
  name: string
}
