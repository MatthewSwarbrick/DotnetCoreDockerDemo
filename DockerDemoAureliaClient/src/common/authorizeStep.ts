import { Redirect } from 'aurelia-router';

export class AuthorizeStep {
  run(routingContext, next) {
    var isLoggedIn = sessionStorage.getItem('accessToken') != null;
    var loginRoute = 'login';

    if (routingContext.getAllInstructions().some(i => i.config.auth)) {
      if (!isLoggedIn) {
        return next.cancel(new Redirect(loginRoute));
      }
    }

    return next();
  }
}