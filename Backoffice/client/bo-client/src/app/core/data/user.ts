import {Observable} from "rxjs";

export interface User {
  name?: string;
  picture?: string | null;
}


export abstract class UserData {
  constructor() {}

  abstract getUser(): Observable<User>;
}
