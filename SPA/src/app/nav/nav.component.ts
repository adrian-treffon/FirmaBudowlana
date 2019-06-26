import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { NewOrderComponent } from '../newOrder/newOrder.component';

@Component({
  providers: [NewOrderComponent],
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {};
  subscription: Subscription;
  roleLocal: string;

  constructor(public authService: AuthService, private alertify: AlertifyService, private router: Router, 
              private newOrderComp: NewOrderComponent) {
  }

  ngOnInit() {}

  goHome() {
    if (this.authService.isAdmin) {
      this.router.navigate(['admin-menu']);
    } else {
      this.router.navigate(['order-list']);
    }
  }

  login() {
    this.authService.login(this.model).subscribe(next => {
      this.alertify.success('Zalogowano się');
    }, error => {
      this.alertify.error(error);
    }, () => {
     //this.navigate();
    });
  }

  loggedIn() {
    return this.authService.loggedIn();
  }

  navigate() {
    if (this.authService.isAdmin) {
      this.router.navigate(['/admin-menu']);
    }
    if (this.authService.isUser) {
      this.router.navigate(['/newOrder']);
    }
  }

  logout() {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    this.authService.decodedToken = null;
    this.authService.currentUser = null;
    this.alertify.message('Wylogowano');
    this.roleLocal = 'User';
    this.router.navigate(['/home']);
  }
}
