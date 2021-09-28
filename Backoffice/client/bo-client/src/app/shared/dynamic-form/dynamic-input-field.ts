import {FormControl} from "@angular/forms";
import {TableFieldType} from "../../api/models/table-field-type";

export interface DynamicInputField {
  type: TableFieldType,
  label: string,
  control: FormControl,
  name: string
}
