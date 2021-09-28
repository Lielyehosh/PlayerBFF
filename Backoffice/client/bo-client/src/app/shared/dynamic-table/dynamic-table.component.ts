import { Output } from '@angular/core';
import {Component, EventEmitter, Input, OnInit} from '@angular/core';
import {Observable} from "rxjs";
import {AppService} from "../../core/services/api/app.service";

interface ColumnEditorConfig {
  list: [], //Only on list type. Example format: { value: 'Element Value', title: 'Element Title' }
}

interface ColumnEditorSettings {
  type: string, //'text' | 'textarea' | 'completer' | 'list' | 'checkbox'
  config: ColumnEditorConfig
}

interface ColumnSettings {
  title: string;
  class: string;
  width: string; // Column width, example: '20px', '20%'
  hide: boolean,
  editable: boolean,
  type: string, // 'text'|'html'|'custom'
  editor: ColumnEditorSettings
}

@Component({
  selector: 'app-dynamic-table',
  templateUrl: './dynamic-table.component.html',
  styleUrls: ['./dynamic-table.component.scss']
})
export class DynamicTableComponent implements OnInit {
  dataObs?: Observable<Object>;
  settingsObs?: Observable<any>;
  @Input() tableId: string = '';
  @Output('create') create: EventEmitter<any> = new EventEmitter<any>();
  @Output('rowSelect') rowSelect: EventEmitter<any> = new EventEmitter<any>();
  // rowDeselect?: EventEmitter<any>;
  // userRowSelect?: EventEmitter<any>;
  // delete?: EventEmitter<any>;
  // edit?: EventEmitter<any>;
  // custom?: EventEmitter<any>;
  // deleteConfirm?: EventEmitter<any>;
  // editConfirm?: EventEmitter<any>;
  // createConfirm?: EventEmitter<any>;
  // rowHover?: EventEmitter<any>;

  constructor(protected appService: AppService) {
  }

  ngOnInit(): void {
    this.dataObs = this.appService.getTableDataRequest(this.tableId);
    this.settingsObs = this.appService.getTableSettingsRequest(this.tableId);
  }

  onCreate($event: any) {
    this.create?.emit($event);
  }

  onCreateConfirm($event: any) {
  }

  onCustom($event: any) {
  }

  onRowHover($event: any) {
  }

  onEdit($event: any) {
  }

  onDelete($event: any) {

  }

  onDeleteConfirm($event: any) {

  }

  onEditConfirm($event: any) {

  }

  onRowSelected($event: any) {
    this.rowSelect.emit($event);
  }
}
