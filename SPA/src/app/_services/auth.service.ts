import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { User } from '../_models/user';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Order } from '../_models/order';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable()
export class AuthService {
    baseUrl: any = 'http://localhost:5000/';
    jwtHelper = new JwtHelperService();
    decodedToken: any;
    role = new BehaviorSubject<string>('User');
    currentRole = this.role.asObservable();
    id: string;
    currentUser: User;

    constructor(private http: HttpClient) { }

    changeCurrentRole(roleNext: string) {
        this.role.next(roleNext);
    }

    login(model: any) {
        return this.http.post(this.baseUrl + 'account/login', model).pipe(
            map((response: any) => {
            const user = response;
            if (user) {
                localStorage.setItem('token', user.token);
                localStorage.setItem('user', JSON.stringify(user.user));
                this.decodedToken = this.jwtHelper.decodeToken(user.token);
                this.changeCurrentRole(this.decodedToken.role);
                this.currentUser = user.user;
                this.id = this.decodedToken.nameid;
            }
            })
        );
    }

    register(user: User) {
        return this.http.post(this.baseUrl + 'account/register', user);
    }

    newOrder(order: Order) {
        return this.http.post(this.baseUrl + 'order/addByClient', order);
    }

    getUserOrderList(): Observable<Order[]> {
        return this.http.get<Order[]>(this.baseUrl + 'account/userOrders/' + this.getUserId());
    }

    getUserOrder(id: string): Observable<Order> {
        return this.http.get<Order>(this.baseUrl + 'account/orderDetails/' + this.getUserId() + '/' + id);
      }

    loggedIn() {
        const token = localStorage.getItem('token');
        return !this.jwtHelper.isTokenExpired(token);
    }

    getName() {
        const token = localStorage.getItem('token');
        const tempToken = this.jwtHelper.decodeToken(token);
        return tempToken.unique_name;
    }

    isAdmin() {
        const token = localStorage.getItem('token');
        if (token === null || typeof token === 'undefined') {
            return false;
        }
        const tempToken = this.jwtHelper.decodeToken(token);
        if (tempToken === null || typeof tempToken === 'undefined') {
            return false;
        }

        if (tempToken.role === 'Admin') {
            return true;
        } else {
            return false;
        }
    }

    getUserId() {
        const token = localStorage.getItem('token');
        const tempToken = this.jwtHelper.decodeToken(token);
        return tempToken.nameid;
    }

    isUser() {
        const token = localStorage.getItem('token');
        if (token === null || typeof token === 'undefined') {
            return false;
        }
        const tempToken = this.jwtHelper.decodeToken(token);

        if (tempToken === null || typeof tempToken === 'undefined') {
            return false;
        }
        if (tempToken.role === 'User') {
            return true;
        } else {
            return false;
        }
    }
}
