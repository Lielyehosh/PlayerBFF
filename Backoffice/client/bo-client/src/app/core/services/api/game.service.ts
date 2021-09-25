import { Injectable } from '@angular/core';
import {AuthStore} from "../../stores/auth.store";
import {webSocket, WebSocketSubject} from "rxjs/webSocket";
import {takeUntil} from "rxjs/operators";
import {Subject} from "rxjs";
import {HttpClient} from "@angular/common/http";

@Injectable({
  providedIn: 'root'
})
export class GameService {
  private username?: string;
  private webSocketSubject: WebSocketSubject<unknown>;
  private destroy$: Subject<void> = new Subject<void>();
  messages: any = [];

  constructor(protected authStore: AuthStore,
              private http: HttpClient) {
    this.username = this.authStore.getAuthPayload().username;
    this.webSocketSubject = webSocket(`wss://localhost:5001/ws/player/join`);
    this.connect();
  }

  connect() {
    this.webSocketSubject
      .pipe(takeUntil(this.destroy$))
      .subscribe(
        msg => this.onMessageReceived(msg),   // Called whenever there is a message from the server.
        err => this.onErrorReceived(err),                          // Called if at any point WebSocket API signals some kind of error.
        () => this.onComplete()                 // Called when connection is closed (for whatever reason).
      )
  }

  private onMessageReceived(msg: any) {
    this.messages.push(`${msg.Sender}: ${msg.Message}`);
    console.log(msg.data);
  }

  private onErrorReceived(err: any) {
    console.log(err);
  }

  private onComplete() {
    console.log("complete");
  }
}
