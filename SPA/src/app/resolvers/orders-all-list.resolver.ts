import { Injectable } from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { AlertifyService } from '../_services/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Order } from '../_models/order';
import { AdminService } from '../_services/admin.service';

@Injectable()
export class OrdersAllResolver implements Resolve<Order[]> {
    constructor(private adminService: AdminService, private router: Router, private alertify: AlertifyService) {}

    resolve(route: ActivatedRouteSnapshot): Observable<Order[]> {
        return this.adminService.getAllOrders().pipe(
            catchError(error => {
                this.alertify.error('Problem z uzyskiwaniem listy zamówień!');
                this.router.navigate(['/admin-menu']);
                return of(null);
            })
        );
    }
}
