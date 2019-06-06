import {Injectable} from '@angular/core';
import { Worker } from '../_models/worker';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { AdminService } from '../_services/admin.service';
import { AlertifyService } from '../_services/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable()
export class NewTeamResolver implements Resolve<Worker[]> {
    constructor(private adminService: AdminService, private router: Router, private alertify: AlertifyService) {}

    resolve(route: ActivatedRouteSnapshot): Observable<Worker[]> {
        console.log('Hello from resolver');
        return this.adminService.getWorkers().pipe(
            catchError(error => {
                this.alertify.error('Problem z uzyskiwaniem danych');
                this.router.navigate(['/admin-menu']);
                return of(null);
            })
        );
    }
}
