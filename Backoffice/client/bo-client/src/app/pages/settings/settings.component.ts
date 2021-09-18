import {ViewChild} from '@angular/core';
import {Component, OnInit} from '@angular/core';
import {FormBuilder, Validators} from "@angular/forms";
import {InputFieldType} from "../../shared/dynamic-form/dynamic-input-field";

@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.scss']
})
export class SettingsComponent implements OnInit {
  formScheme: any = [
    {
      id: 'Field1',
      type: InputFieldType.CHECKBOX,
      control: this.fb.control({value: true, disabled: true}),
      label: "Checkbox",
      name: 'Field1'
    },
    {
      id: 'Field2',
      type: InputFieldType.TEXT,
      control: this.fb.control({value: '', disabled: true}, Validators.required),
      label: "Text",
      name: 'Field2'
    },
    {
      id: 'Field3',
      type: InputFieldType.NUMBER,
      control: this.fb.control({value: '', disabled: true}, Validators.required),
      label: "Number",
      name: 'Field3'
    }
  ];
  canEdit: boolean = true;
  editMode: boolean = false;

  constructor(private fb: FormBuilder) {
  };


  ngOnInit(): void {
  }

  onSubmit($event: any) {
    debugger;
    console.log($event);
  }

  @ViewChild('settingForm') settingForm: any;
  onEditBtnClicked() {
    this.editMode = !this.editMode;
    if (this.editMode) {
      this.settingForm.setEditMode();
    } else {
      this.settingForm.setViewOnlyMode();
    }
  }
}
