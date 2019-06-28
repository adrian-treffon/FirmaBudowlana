import { Component, OnInit } from '@angular/core';
import { AdminService } from '../_services/admin.service';
import { AlertifyService } from '../_services/alertify.service';
import { Router } from '@angular/router';
import { Order } from '../_models/order';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-payment-unpaid-list',
  templateUrl: './payment-unpaid-list.component.html',
  styleUrls: ['./payment-unpaid-list.component.css']
})
export class PaymentUnpaidListComponent implements OnInit {
  orderList: Order[];

  constructor(private adminService: AdminService, private alertify: AlertifyService, private router: Router,
              private datePipe: DatePipe) { }

  ngOnInit() {
    this.loadOrders();
  }

  loadOrders() {
    this.adminService.getUnpaidPayments().subscribe((ordersTemp: Order[]) => {
        this.orderList = ordersTemp;
    }, error => {
      this.alertify.error('Nie udało się załadować listy zleceń do opłacenia: ' + error);
    });
  }

  getEndDate(i: number) {
    if (this.orderList[i].validated === false) {
      this.orderList[i].endDate = null;
    }
    if (this.orderList[i].endDate === null || typeof this.orderList[i].endDate === 'undefined') {
      return 'Nie zaproponowano';
    } else {
      return this.datePipe.transform(this.orderList[i].endDate, 'dd-MM-yyyy');
    }
  }

  pay(i: number) {
    this.adminService.addPayment(this.orderList[i]).subscribe(() => {
      this.alertify.success('Opłacono pracowników za podane zlecenie!');
      }, error => {
        this.alertify.error('Błąd podczas opłacania pracowników' + error);
      }, () => {
          this.ngOnInit();
      });
  }
}
