var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
define('app',["require", "exports", "aurelia-framework", "aurelia-event-aggregator", "./common/authorizeStep"], function (require, exports, aurelia_framework_1, aurelia_event_aggregator_1, authorizeStep_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    var App = (function () {
        function App(eventAggregator) {
            var _this = this;
            this.eventSubscriptions = [];
            this.eventSubscriptions.push(eventAggregator.subscribe("userLoggedIn", function () { _this.userLoggedIn = true; }));
        }
        App.prototype.configureRouter = function (config, router) {
            var self = this;
            config.title = "Docker Demo";
            config.addPipelineStep('authorize', authorizeStep_1.AuthorizeStep);
            config.map([
                { route: '', moduleId: './home/home', name: 'home', title: 'Home', auth: true },
                { route: 'login', moduleId: './login/login', name: 'login', title: 'Login' }
            ]);
            config.mapUnknownRoutes({ route: '', moduleId: './home/home', name: 'home', title: 'Home', auth: true });
            self.router = router;
        };
        App.prototype.activate = function () {
            this.userLoggedIn = sessionStorage.getItem("accessToken") != null;
        };
        App.prototype.dettached = function () {
            this.eventSubscriptions.forEach(function (sub) { return sub.dispose(); });
        };
        App.prototype.logout = function () {
            sessionStorage.removeItem("accessToken");
            this.userLoggedIn = false;
            this.router.navigateToRoute("login");
        };
        App = __decorate([
            aurelia_framework_1.inject(aurelia_event_aggregator_1.EventAggregator),
            __metadata("design:paramtypes", [aurelia_event_aggregator_1.EventAggregator])
        ], App);
        return App;
    }());
    exports.App = App;
});



define('environment',["require", "exports"], function (require, exports) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.default = {
        debug: true,
        testing: true
    };
});



define('main',["require", "exports", "./environment"], function (require, exports, environment_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    function configure(aurelia) {
        aurelia.use
            .standardConfiguration()
            .feature('resources');
        if (environment_1.default.debug) {
            aurelia.use.developmentLogging();
        }
        if (environment_1.default.testing) {
            aurelia.use.plugin('aurelia-testing');
        }
        aurelia.start().then(function () { return aurelia.setRoot(); });
    }
    exports.configure = configure;
});



define('common/authorizeStep',["require", "exports", "aurelia-router"], function (require, exports, aurelia_router_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    var AuthorizeStep = (function () {
        function AuthorizeStep() {
        }
        AuthorizeStep.prototype.run = function (routingContext, next) {
            var isLoggedIn = sessionStorage.getItem('accessToken') != null;
            var loginRoute = 'login';
            if (routingContext.getAllInstructions().some(function (i) { return i.config.auth; })) {
                if (!isLoggedIn) {
                    return next.cancel(new aurelia_router_1.Redirect(loginRoute));
                }
            }
            return next();
        };
        return AuthorizeStep;
    }());
    exports.AuthorizeStep = AuthorizeStep;
});



define('common/server',["require", "exports", "q"], function (require, exports, Q) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    var Server = (function () {
        function Server() {
            var _this = this;
            this.tokenKey = "accessToken";
            this.rootPath = "api/";
            this.post = function (url, data) {
                _this.isBusy = true;
                return Q.when($.ajax(_this.rootPath + url, {
                    type: 'POST',
                    data: data,
                    headers: _this.setHeaders()
                })
                    .always(function () { return _this.isBusy = false; })
                    .fail(function (error) { return console.log(error); }));
            };
        }
        Server.prototype.setHeaders = function () {
            var token = sessionStorage.getItem(this.tokenKey);
            var headers = {};
            if (token) {
                headers.Authorization = 'Bearer ' + token;
            }
            return headers;
        };
        ;
        return Server;
    }());
    exports.Server = Server;
});



define('home/home',["require", "exports"], function (require, exports) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    var Home = (function () {
        function Home() {
        }
        return Home;
    }());
    exports.Home = Home;
});



var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
define('login/login',["require", "exports", "aurelia-framework", "aurelia-router", "aurelia-event-aggregator", "../common/server"], function (require, exports, aurelia_framework_1, aurelia_router_1, aurelia_event_aggregator_1, server_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    var Login = (function () {
        function Login(server, router, eventAggregator) {
            this.server = server;
            this.router = router;
            this.eventAggregator = eventAggregator;
        }
        Login.prototype.login = function () {
            var _this = this;
            var modelToSend = {
                username: this.username,
                password: this.password
            };
            this.server.post("account/sign-in", modelToSend).then(function (data) {
                sessionStorage.setItem(_this.server.tokenKey, data.access_token);
                _this.router.navigateToRoute("home");
                _this.eventAggregator.publish("userLoggedIn");
            });
        };
        Login = __decorate([
            aurelia_framework_1.inject(server_1.Server, aurelia_router_1.Router, aurelia_event_aggregator_1.EventAggregator),
            __metadata("design:paramtypes", [server_1.Server, aurelia_router_1.Router, aurelia_event_aggregator_1.EventAggregator])
        ], Login);
        return Login;
    }());
    exports.Login = Login;
});



define('resources/index',["require", "exports"], function (require, exports) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    function configure(config) {
    }
    exports.configure = configure;
});



define('text!app.html', ['module'], function(module) { module.exports = "<template><require from=\"./less/app.css\"></require><require from=\"bootstrap/css/bootstrap.css\"></require><nav class=\"navbar navbar-light bg-faded d-flex flex-row justify-content-between\"><a class=\"navbar-brand\" route-href=\"home\"><img src=\"assets/moby.png\" height=\"45\" class=\"d-inline-block align-top logo\" alt=\"\"> Docker Demo</a><ul class=\"navbar-nav flex-row menu-list\"><li if.bind=\"!userLoggedIn\" class=\"nav-item\"><a class=\"nav-link pointer\" route-href=\"login\">Login</a></li><li if.bind=\"!userLoggedIn\" class=\"nav-item\"><a class=\"nav-link pointer\" route-href=\"\">Register</a></li><li if.bind=\"userLoggedIn\" class=\"nav-item\"><a class=\"nav-link pointer\" click.delegate=\"logout()\">Logout</a></li></ul></nav><div class=\"container\"><router-view></router-view></div></template>"; });
define('text!home/home.html', ['module'], function(module) { module.exports = "<template>HOME</template>"; });
define('text!less/app.css', ['module'], function(module) { module.exports = ".logo {\n  margin-right: 16px;\n}\n.pointer {\n  cursor: pointer;\n}\n.menu-list li {\n  margin-right: 10px;\n}\n"; });
define('text!login/login.html', ['module'], function(module) { module.exports = "<template><div class=\"container mt-5 w-75\"><div class=\"card card-outline-danger mb-3 text-center\"><div class=\"card-block\"><h3 class=\"card-title\">Log into Docker Demo!</h3><hr><form><div class=\"input-group\"><span class=\"input-group-addon\"><i class=\"fa fa-user\"></i></span> <input type=\"text\" class=\"form-control\" placeholder=\"Username\" value.bind=\"username\"></div><br><div class=\"input-group\"><span class=\"input-group-addon\"><i class=\"fa fa-lock\"></i></span> <input type=\"password\" class=\"form-control\" placeholder=\"Password\" value.bind=\"password\"></div><hr><button type=\"submit\" class=\"btn btn-lg btn-outline-danger pointer\" click.delegate=\"login()\">Log in</button></form></div><div class=\"card-block\"><a class=\"card-link text-muted float-left\" href=\"#\"><i class=\"fa fa-lock\"></i> Forgot your password?</a> <a class=\"card-link text-muted float-right\" href=\"#\">Create an account</a></div></div></div></template>"; });
//# sourceMappingURL=app-bundle.js.map