import * as Q from 'q';

export class Server {
    isBusy: boolean;
    tokenKey: string = "accessToken";
    rootPath: string = "api/"

    post = (url, data) => {
        this.isBusy = true;
        return Q.when($.ajax(this.rootPath + url, 
            {
                type: 'POST',
                data: data,
                headers: this.setHeaders()
            })
            .always(() => this.isBusy = false)
            .fail(error => console.log(error)));
    }

    setHeaders() {
        let token = sessionStorage.getItem(this.tokenKey);
        let headers: any = {};
        if (token) {
            headers.Authorization = 'Bearer ' + token;
        }

        return headers;
    };
}