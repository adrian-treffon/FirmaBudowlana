import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { User } from '../_models/user';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable()
export class AuthService {
    baseUrl: any = 'http://localhost:5000/account/';
    jwtHelper = new JwtHelperService();
    decodedToken: any;
    role: string;
    currentUser: User;

    constructor(private http: HttpClient) { }

    login(model: any) {
        return this.http.post(this.baseUrl + 'login', model).pipe(
            map((response: any) => {
            const user = response;
            if (user) {
                localStorage.setItem('token', user.token);
                this.decodedToken = this.jwtHelper.decodeToken(user.token);
                this.role = this.decodedToken.role;
            }
            })
        );
    }

    register(user: User) {
        return this.http.post(this.baseUrl + 'register', user);
    }

    loggedIn() {
        const token = localStorage.getItem('token');
        return !this.jwtHelper.isTokenExpired(token);
      }
}
