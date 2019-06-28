import { Injectable } from '@angular/core';
import { Team } from '../_models/team';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { AdminService } from '../_services/admin.service';
import { AlertifyService } from '../_services/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable()
export class ValidateOrderResolver implements Resolve<Team[]> {
    constructor(private adminService: AdminService, private router: Router, private alertify: AlertifyService) {}

    resolve(route: ActivatedRouteSnapshot): Observable<Team[]> {
        return this.adminService.getTeams().pipe(
            catchError(error => {
                this.alertify.error('Problem z uzyskiwaniem danych');
                this.router.navigate(['/admin-menu']);
                return of(null);
            })
        );
    }
}
