import { inject } from 'aurelia-framework';
import { Router } from 'aurelia-router';
import { EventAggregator } from 'aurelia-event-aggregator';
import { AuthorizeStep } from './common/authorizeStep';

@inject(EventAggregator)
export class App {
  router: Router;
  eventSubscriptions: any[] = [];
  userLoggedIn: boolean;

  constructor(eventAggregator: EventAggregator) {
    this.eventSubscriptions.push(eventAggregator.subscribe("userLoggedIn", () => { this.userLoggedIn = true; }));
  }

  configureRouter(config, router) {
    let self = this;
    config.title = "Docker Demo";
    config.addPipelineStep('authorize', AuthorizeStep);
    config.map([
      { route: '', moduleId: './home/home', name: 'home', title: 'Home', auth: true },
      { route: 'register', moduleId: './register/register', name: 'register', title: 'Register' },
      { route: 'login', moduleId: './login/login', name: 'login', title: 'Login' }
    ]);

    config.mapUnknownRoutes({ route: '', moduleId: './home/home', name:'home', title: 'Home', auth: true });
    self.router = router;
  }

  activate() {
    this.userLoggedIn = sessionStorage.getItem("accessToken") != null;
  }

  dettached() {
    this.eventSubscriptions.forEach(sub => sub.dispose())
  }

  logout() {
    sessionStorage.removeItem("accessToken");
    this.userLoggedIn = false;
    this.router.navigateToRoute("login");
  }
}
