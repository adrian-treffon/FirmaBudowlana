import { Injectable } from '@angular/core';
import { Worker } from '../_models/worker';
import { Team } from '../_models/team';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { AdminService } from '../_services/admin.service';
import { AlertifyService } from '../_services/alertify.service';
import { Observable, of, forkJoin, Subject } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { map } from 'rxjs/operators';

@Injectable()
export class ReportInputResolver implements Resolve<any> {
    constructor(private adminService: AdminService, private router: Router, private alertify: AlertifyService) {}

    resolve(route: ActivatedRouteSnapshot): Observable<any> {
        return forkJoin([
                this.adminService.getAllWorkers(),
                this.adminService.getAllTeams()
                .pipe(catchError(error => {
                    return of(null);
                }))
        ]).pipe(map(result => {
            return {
                types: result[0],
                departments: result[1]
            };
        }));
    }
}
