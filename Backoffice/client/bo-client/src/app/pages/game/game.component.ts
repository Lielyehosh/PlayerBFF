import {Component, OnDestroy, OnInit} from '@angular/core';
import {Subject, Subscription} from 'rxjs';
import {Observable} from 'rxjs';
import {webSocket, WebSocketSubject} from "rxjs/webSocket";
import {AuthStore} from "../../core/stores/auth.store";
import {takeUntil} from "rxjs/operators";
import {HttpClient} from "@angular/common/http";
import {Settings} from "../../core/services/models/settings";

@Component({
  selector: 'app-game',
  templateUrl: './game.component.html',
  styleUrls: ['./game.component.scss']
})
export class GameComponent implements OnInit, OnDestroy {
  private destroy$: Subject<void> = new Subject<void>();
  private webSocketSubject: WebSocketSubject<unknown>;
  msgInput: string = "";
  messages: any = [];
  username?: string;
  users: any = ["Aharon", "Sharon"];

  constructor(protected authStore: AuthStore, protected http: HttpClient) {
    this.username = this.authStore.getAuthPayload().username;
    console.log(this.username);
    // this.webSocketSubject = webSocket(`ws://localhost:4649/chat?name=${this.username}`);
    this.webSocketSubject = webSocket({
      url: `wss://localhost:5001/ws/player/join`,
    });
    // `wss://localhost:5001/ws/player/join`);
  }

  ngOnInit(): void {
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

  sendMessage() {
    this.webSocketSubject.next( {Message: this.msgInput})
  }

  playMove() {
    // this.webSocketSubject.next( {Player: this.authStore.getAuthPayload().username, Move: this.msgInput})
    return this.http.post<any>('https://localhost:5001/ws/player/move', {
      Move: this.msgInput
    }).pipe(takeUntil(this.destroy$))
      .subscribe(res => {
        this.messages.push(res);
      });
  }

  onUnsubscribeBtnClicked() {
    this.webSocketSubject.unsubscribe();
  }

  private onMessageReceived(msg: any) {
    this.messages.push(`Move ${msg.Result}`);
    console.log(msg.data);
  }

  private onErrorReceived(err: any) {
    console.log(err);
  }

  private onComplete() {
    console.log("complete");
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
