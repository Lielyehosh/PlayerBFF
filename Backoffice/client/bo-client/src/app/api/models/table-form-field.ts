/* tslint:disable */
import { TableFieldChoice } from './table-field-choice';
import { TableFieldType } from './table-field-type';
export interface TableFormField {
  choices?: null | Array<TableFieldChoice>;
  hidden?: boolean;
  id: string;
  label: string;
  readOnly?: boolean;
  required?: boolean;
  type: TableFieldType;
}
