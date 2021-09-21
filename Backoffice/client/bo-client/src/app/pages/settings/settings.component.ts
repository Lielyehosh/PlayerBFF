import {ViewChild} from '@angular/core';
import {Component, OnInit} from '@angular/core';
import {FormBuilder, Validators} from "@angular/forms";
import {InputFieldType} from "../../shared/dynamic-form/dynamic-input-field";
import {SettingsService} from "../../core/services/api/settings.service";

@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.scss']
})
export class SettingsComponent implements OnInit {
  formScheme: any = [
    {
      type: InputFieldType.TEXT,
      control: this.fb.control({value: "Liel's Site", disabled: true}),
      label: "Site Title",
      name: 'title'
    },
    {
      type: InputFieldType.CHECKBOX,
      control: this.fb.control({value: true, disabled: true}),
      label: "Secure",
      name: 'secure'
    }
  ];
  canEdit: boolean = true;
  editMode: boolean = false;

  constructor(private fb: FormBuilder,
              private settingsService: SettingsService) {

  };


  ngOnInit(): void {
  }

  onSubmit($event: any) {
    console.log($event);
    this.settingsService.postEditSettings({
      SiteTitle: $event.title
    }).subscribe(res => {
      console.log(res);
    })
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
