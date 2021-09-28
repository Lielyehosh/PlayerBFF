import { Component, Input, OnInit } from '@angular/core';

import { ViewCell } from 'ng2-smart-table';

@Component({
  template: `
    {{renderValue | date}}
  `,
})
export class DateRenderComponent implements ViewCell, OnInit {

  renderValue?: string;

  // @ts-ignore
  @Input() value: string | number;
  @Input() rowData: any;

  ngOnInit() {
    this.renderValue = this.value?.toString();
  }

}
