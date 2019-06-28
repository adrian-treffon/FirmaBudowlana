import { Injectable } from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { AlertifyService } from '../_services/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
<<<<<<< HEAD
import { AdminService } from '../_services/admin.service';
import { OrderWithUser } from '../_models/orderWithUser';

@Injectable()
export class OrdersAllResolver implements Resolve<OrderWithUser[]> {
    constructor(private adminService: AdminService, private router: Router, private alertify: AlertifyService) {}

    resolve(route: ActivatedRouteSnapshot): Observable<OrderWithUser[]> {
        return this.adminService.getAllOrdersWithUser().pipe(
=======
import { Order } from '../_models/order';
import { AdminService } from '../_services/admin.service';

@Injectable()
export class OrdersAllResolver implements Resolve<Order[]> {
    constructor(private adminService: AdminService, private router: Router, private alertify: AlertifyService) {}

    resolve(route: ActivatedRouteSnapshot): Observable<Order[]> {
        // console.log('From order-detail resolver, route.data.id = ' + route.params.id);
        return this.adminService.getAllOrders().pipe(
>>>>>>> adcae90761bb37eeb6e22c490764c1fd90e6ae80
            catchError(error => {
                this.alertify.error('Problem z uzyskiwaniem listy zamówień!');
                this.router.navigate(['/admin-menu']);
                return of(null);
            })
        );
    }
}
