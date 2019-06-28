import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { ActivatedRoute, Router } from '@angular/router';
import { AlertifyService } from '../_services/alertify.service';
import { Order } from '../_models/order';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-order-details',
  templateUrl: './order-details.component.html',
  styleUrls: ['./order-details.component.css']
})


export class OrderDetailsComponent implements OnInit {
  orderID: string;
  order: Order;
  constructor(public authService: AuthService, private route: ActivatedRoute,
              private alertify: AlertifyService, private router: Router, private datePipe: DatePipe) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.order = data.order;
    });
    this.route.params.subscribe(data => {
    this.orderID = data.id;
    }, error => {
      this.alertify.error('Nie udało się załadować zamówienia do edycji: ' + error);
    });
  }

  getEndDate() {
    if (this.order.validated === false) {
      this.order.endDate = null;
    }
    if (this.order.endDate === null || typeof this.order.endDate === 'undefined') {
      return 'Nie zaproponowano';
    } else {
      return this.datePipe.transform(this.order.endDate, 'dd-MM-yyyy');
    }
  }

  getCost() {
    if (this.order.cost === null || typeof this.order.cost === 'undefined' || this.order.cost === 0) {
      return 'Nie zaproponowano';
    } else {
      return this.order.cost + ' zł';
    }
  }

  getValidated() {
    if (this.order.validated === true) {
      return 'Tak';
    } else {
        return 'Nie';
    }
  }

  getPaid() {
    if (this.order.paid === true) {
      return 'Tak';
    } else {
        return 'Nie';
      }
  }
}
