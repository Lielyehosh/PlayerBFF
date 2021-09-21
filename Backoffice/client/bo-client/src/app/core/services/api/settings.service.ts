import {Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Settings} from "../models/settings";


@Injectable({
  providedIn: 'root'
})
export class SettingsService {

  constructor(private http: HttpClient) {
  }

  postEditSettings(settings: Settings) {
    return this.http.post<Settings>('/api/settings/edit', settings);
  }

}
