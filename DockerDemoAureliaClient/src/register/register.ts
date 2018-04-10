import { inject } from 'aurelia-framework';
import { Router } from 'aurelia-router';
import { Server } from '../common/server';

@inject(Server, Router)
export class Register {
    username: string;
    password: string;
    confirmPassword: string;
    agreeToTerms: boolean;

    server: Server;
    router: Router;

    constructor(server: Server, router: Router) {
        this.server = server;
        this.router = router;
    }

    register() {
        let modelToSend = {
            username: this.username,
            password: this.password,
            confirmPassword: this.confirmPassword,
            agreeToTermsAndConditions: this.agreeToTerms
        };

        this.server.post("account/register", modelToSend).then(data => {
            this.router.navigateToRoute("login");
        });
    }
}