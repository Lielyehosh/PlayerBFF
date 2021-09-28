import {Input} from '@angular/core';
import {Component, EventEmitter, OnInit, Output} from '@angular/core';
import {FormBuilder, FormControl, FormGroup, Validators} from '@angular/forms';
import {DynamicInputField} from "./dynamic-input-field";
import {TableForm} from "../../api/models/table-form";

@Component({
  selector: 'app-dynamic-form',
  templateUrl: './dynamic-form.component.html',
  styleUrls: ['./dynamic-form.component.scss']
})
export class DynamicFormComponent implements OnInit {
  get formScheme(): TableForm {
    // @ts-ignore
    return this._formScheme;
  }


  inputFields: Array<DynamicInputField> = [];
  @Output('submit') submit: EventEmitter<any> = new EventEmitter<any>();
  @Output('cancel') cancel: EventEmitter<any> = new EventEmitter<any>();

  @Input() viewOnly: boolean = true;
  private _formScheme?: TableForm;
  @Input() set formScheme(value: TableForm) {
    this._formScheme = value;
    // @ts-ignore
    this.inputFields = this.generateInputFields(this._formScheme)
  }
  form: FormGroup | null = null;

  constructor(private fb: FormBuilder) {
  }


  ngOnInit(): void {
    this.form = this.fb.group({
      ...this.convertArrayToObject(this.inputFields, 'name', 'control')
    });
    this.form.valueChanges.subscribe(console.log);
  }

  submitBtnClicked() {
    if (this.form?.valid) {
      this.submit.emit(this.form?.value);
    }
  }

  cancelBtnClicked() {
    this.cancel.emit(null);
  }

  setViewOnlyMode() {
    this.inputFields.forEach(field => {
      this.form?.controls[field.name].disable();
    });
  }

  setEditMode() {
    this.inputFields.forEach(field => {
      this.form?.controls[field.name].enable();
    });
  }

  private convertArrayToObject(array: any, key: any, value: any) {
    const initialValue = {};
    return array.reduce((obj: any, item: any) => {
      return {
        ...obj,
        [item[key]]: item[value],
      };
    }, initialValue);
  };

  public generateInputFields(_formScheme: TableForm) {
    return _formScheme.fields?.map((field) => {
      return {
        label: field.label,
        name: field.id,
        type: field.type,
        control: this.fb.control({disabled: field.readOnly, value: ''})
      }
    });
  }
}
