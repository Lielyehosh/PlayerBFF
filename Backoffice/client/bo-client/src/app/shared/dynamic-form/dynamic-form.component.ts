import {Input} from '@angular/core';
import {Component, EventEmitter, OnInit, Output} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {TableForm} from "../../api/models/table-form";
import {TableFormField} from "../../api/models/table-form-field";

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


  @Output('submit') submit: EventEmitter<any> = new EventEmitter<any>();
  @Output('cancel') cancel: EventEmitter<any> = new EventEmitter<any>();

  private _formScheme?: TableForm;
  @Input() set formScheme(value: TableForm) {
    this._formScheme = value;
  }
  @Input() viewOnly: boolean = true;

  form: FormGroup | null = null;

  constructor(private fb: FormBuilder) {
  }


  ngOnInit(): void {
    this.form = this.fb.group(this.generateFormControls(this.formScheme.fields ?? []));
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
    this.form?.disable();
  }

  setEditMode() {
    // TODO - disable the non-editable fields
    this.form?.enable();
  }

  private generateFormControls(array: Array<TableFormField>) {
    const initialValue = {};
    return array.reduce((obj: any, field: TableFormField) => {
      return {
        ...obj,
        [field.id]: this.fb.control({disabled: field.readOnly, value: ''}, [field.required ? Validators.required : Validators.nullValidator]),
      };
    }, initialValue);
  };
}
