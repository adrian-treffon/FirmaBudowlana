import { Injectable } from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { AlertifyService } from '../_services/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { AdminService } from '../_services/admin.service';
import { OrderWithUser } from '../_models/orderWithUser';

@Injectable()
export class OrdersAllResolver implements Resolve<OrderWithUser[]> {
    constructor(private adminService: AdminService, private router: Router, private alertify: AlertifyService) {}

    resolve(route: ActivatedRouteSnapshot): Observable<OrderWithUser[]> {
        return this.adminService.getAllOrdersWithUser().pipe(
            catchError(error => {
                this.alertify.error('Problem z uzyskiwaniem listy zamówień!');
                this.router.navigate(['/admin-menu']);
                return of(null);
            })
        );
    }
}
