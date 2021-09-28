/* tslint:disable */
import { TableFieldChoice } from './table-field-choice';
import { TableFieldType } from './table-field-type';
export interface TableFormField {
  choices?: null | Array<TableFieldChoice>;
  hidden?: boolean;
  id?: null | string;
  label?: null | string;
  readOnly?: boolean;
  required?: boolean;
  type?: TableFieldType;
}
