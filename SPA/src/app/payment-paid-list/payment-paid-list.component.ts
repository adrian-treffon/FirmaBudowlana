import { Component, OnInit } from '@angular/core';
import { AdminService } from '../_services/admin.service';
import { AlertifyService } from '../_services/alertify.service';
import { Router } from '@angular/router';
import { Payment } from '../_models/payment';

@Component({
  selector: 'app-payment-paid-list',
  templateUrl: './payment-paid-list.component.html',
  styleUrls: ['./payment-paid-list.component.css']
})
export class PaymentPaidListComponent implements OnInit {
  payments: Payment[];

  constructor(private adminService: AdminService, private alertify: AlertifyService, private router: Router) { }

  ngOnInit() {
    this.loadPayments();
  }

  loadPayments() {
    this.adminService.getPaidPayments().subscribe((paymentsTemp: Payment[]) => {
      this.payments = paymentsTemp;
  }, error => {
    this.alertify.error('Nie udało się załadować listy płatności: ' + error);
  });
  }

}
