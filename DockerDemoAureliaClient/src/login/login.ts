import { inject } from 'aurelia-framework';
import { Router } from 'aurelia-router';
import { EventAggregator } from 'aurelia-event-aggregator';
import { Server } from '../common/server';

@inject(Server, Router, EventAggregator)
export class Login {
    username: string;
    password: string;
    server: Server;
    router: Router;
    eventAggregator: EventAggregator;

    constructor(server: Server, router: Router, eventAggregator: EventAggregator) {
        this.server = server;
        this.router = router;
        this.eventAggregator = eventAggregator;
    }

    login() {
        let modelToSend = {
            username: this.username,
            password: this.password
        };

        this.server.post("account/sign-in", modelToSend).then(data => {
            sessionStorage.setItem(this.server.tokenKey, data.access_token);
            this.router.navigateToRoute("home");
            this.eventAggregator.publish("userLoggedIn");
        });
    }
}