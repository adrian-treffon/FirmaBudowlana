import { Component, OnInit } from '@angular/core';
import { Order } from '../_models/order';
import { AlertifyService } from '../_services/alertify.service';
import { AuthService } from '../_services/auth.service';

@Component({
  selector: 'app-orders',
  templateUrl: './orders.component.html',
  styleUrls: ['./orders.component.css']
})
export class OrdersComponent implements OnInit {
  userOrderList: Order[];

  constructor(public authService: AuthService, private alertify: AlertifyService) { }

  ngOnInit() {
    this.loadUserOrderList();
  }

  loadUserOrderList() {
    this.authService.getUserOrderList().subscribe((ordersTemp: Order[]) => {
      this.userOrderList = ordersTemp;
  }, error => {
    this.alertify.error('Nie udało się załadować listy zleceń: ' + error);
  });
  }

<<<<<<< HEAD
  isValidatedText(isValidatedBool: boolean) {
    if (isValidatedBool === true) {
=======
  isPaidText(isPaidBool: boolean) {
    if (isPaidBool === true) {
>>>>>>> adcae90761bb37eeb6e22c490764c1fd90e6ae80
      return 'Tak';
    } else {
        return 'Nie';
      }
    }
  }

