import { Router } from 'aurelia-router';
import { AuthorizeStep } from './common/authorizeStep';

export class App {
  router: Router;

  configureRouter(config, router) {
    let self = this;
    config.title = "DockerDemo";
    config.addPipelineStep('authorize', AuthorizeStep);
    config.map([
      { route: '', moduleId: './home/home', name: 'home', title: 'Home', auth: true },
      { route: 'login', moduleId: './login/login', name: 'login', title: 'Login' }
    ]);

    config.mapUnknownRoutes({ route: '', moduleId: './home/home', name:'home', title: 'Home', auth: true });
    self.router = router;
  }
}
