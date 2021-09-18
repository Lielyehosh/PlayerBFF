import {Input} from '@angular/core';
import {Component, EventEmitter, OnInit, Output} from '@angular/core';
import {FormBuilder, FormGroup} from '@angular/forms';
import {DynamicInputField} from "./dynamic-input-field";

@Component({
  selector: 'app-dynamic-form',
  templateUrl: './dynamic-form.component.html',
  styleUrls: ['./dynamic-form.component.scss']
})
export class DynamicFormComponent implements OnInit {

  @Input() inputFields: Array<DynamicInputField> = [];
  @Output('submit') submit: EventEmitter<any> = new EventEmitter<any>();
  form: FormGroup | null = null;

  constructor(private fb: FormBuilder) {
  }

  convertArrayToObject(array: any, key: any, value: any) {
    const initialValue = {};
    return array.reduce((obj: any, item: any) => {
      return {
        ...obj,
        [item[key]]: item[value],
      };
    }, initialValue);
  };

  ngOnInit(): void {
    this.form = this.fb.group({
      ...this.convertArrayToObject(this.inputFields, 'id', 'control')
    });
    this.form.valueChanges.subscribe(console.log);
  }

  submitBtnClicked($event: MouseEvent) {
    if (this.form?.valid) {
      this.submit.emit(this.form?.value);
    }
  }
}
