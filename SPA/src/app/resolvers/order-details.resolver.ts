import { Injectable } from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { AlertifyService } from '../_services/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { AuthService } from '../_services/auth.service';
import { Order } from '../_models/order';

@Injectable()
export class OrderDetailsResolver implements Resolve<Order> {
    constructor(private authService: AuthService, private router: Router, private alertify: AlertifyService) {}

    resolve(route: ActivatedRouteSnapshot): Observable<Order> {
        // console.log('From order-detail resolver, route.data.id = ' + route.params.id);
        return this.authService.getUserOrder(route.params.id).pipe(
            catchError(error => {
                this.alertify.error('Problem z uzyskiwaniem danych');
                this.router.navigate(['/admin-menu']);
                return of(null);
            })
        );
    }
}
