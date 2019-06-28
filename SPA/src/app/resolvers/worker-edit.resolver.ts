import { Injectable } from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { AlertifyService } from '../_services/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Worker } from '../_models/worker';
import { AdminService } from '../_services/admin.service';

@Injectable()
export class WorkerEditResolver implements Resolve<Worker> {
    constructor(private adminService: AdminService, private router: Router, private alertify: AlertifyService) {}

    resolve(route: ActivatedRouteSnapshot): Observable<Worker> {
        return this.adminService.getWorker(route.params.id).pipe(
            catchError(error => {
                this.alertify.error('Problem z uzyskiwaniem danych');
                this.router.navigate(['/worker-list']);
                return of(null);
            })
        );
    }
}
