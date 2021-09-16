import {Injectable} from "@angular/core";
import {Router} from "@angular/router";
import {NbAuthService} from "@nebular/auth";
import {BehaviorSubject} from "rxjs";
import {setHeaderUser} from "../../theme/components/header/header.component";
import { User } from "../data/user";

let currentUser: any = null;

@Injectable({
  providedIn: 'root'
})

export class AuthStore {
  private lang: string = "en";

  constructor(nbAuthService: NbAuthService, router: Router) {
    const onAuthChange = () => {
      const payload = this.getAuthPayload();
      if (!payload) return;
      setHeaderUser({name: payload.username, picture: null});
      currentUser = {name: payload.username, label: payload.email};
    };
    this.authPayloadSubject.subscribe(onAuthChange);
    nbAuthService.onTokenChange()
      .subscribe((token) => {
        debugger;
        this.setAuthToken(token ? token.getValue() : null, token ? token.getPayload() : null);
      });
  }

  private authTokenSubject = new BehaviorSubject<string | null>(null);
  private authPayloadSubject = new BehaviorSubject<JwtPayload>({});

  public get currentUser() {
    return currentUser;
  }

  public getAuthToken() {
    return this.authTokenSubject.getValue();
  }

  public getAuthPayload() {
    return this.authPayloadSubject.getValue();
  }

  public get authPayloadObs() {
    return this.authPayloadSubject.asObservable();
  }

  public setAuthToken(token: string | null, payload: JwtPayload) {
    this.authTokenSubject.next(token);
    this.authPayloadSubject.next(payload);
    if (token) {
      localStorage.setItem('auth_t', token);
      localStorage.setItem('auth_p', JSON.stringify(payload));
    } else {
      localStorage.removeItem('auth_t');
      localStorage.removeItem('auth_p');
      localStorage.removeItem('auth_app_token');
    }

  }
}

interface JwtPayload {
  username?: string;
  email?: string;
}

(window as any).CurrentUser = () => currentUser;
