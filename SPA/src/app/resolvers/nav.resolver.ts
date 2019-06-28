import {Injectable} from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable()
export class NavResolver implements Resolve<any> {
    constructor(private authService: AuthService, private router: Router, private alertify: AlertifyService) {}

    resolve(route: ActivatedRouteSnapshot): Observable<any> {
        return this.authService.decodedToken(
            catchError(error => {
                this.alertify.error('Problem z uzyskiwaniem danych');
                return of(null);
            })
        );
    }
}
