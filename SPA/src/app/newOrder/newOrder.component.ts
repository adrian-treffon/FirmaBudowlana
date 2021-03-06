import { Component, OnInit } from '@angular/core';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker/public_api';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Order } from '../_models/order';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';
import { Location } from '@angular/common';

@Component({
  selector: 'app-newOrder',
  templateUrl: './newOrder.component.html',
  styleUrls: ['./newOrder.component.css']
})
export class NewOrderComponent implements OnInit {
  bsConfig: Partial<BsDatepickerConfig>;
  newOrderForm: FormGroup;
  order: Order;
  isSubmited = false;
  now = new Date();
  constructor(private fb: FormBuilder, private router: Router, private authServide: AuthService,
              private alertify: AlertifyService, private location: Location) {}

  ngOnInit() {
    this.isSubmited = false;
    this.createNewOrderForm();
    new Date(this.now).getTime();
  }
  createNewOrderForm() {
    this.newOrderForm = this.fb.group({
      description: ['', Validators.required],
      startDate: ['', Validators.required]
    });
  }

  reloadEverything() {
    this.isSubmited = false;
    this.newOrderForm.reset();
  }

  submitOrder() {
    if (this.newOrderForm.valid) {
      this.order = Object.assign({}, this.newOrderForm.value);
      this.order.userID = this.authServide.id;
      this.authServide.newOrder(this.order).subscribe(() => {
        this.alertify.success('Złożono zamówienie!');
        }, error => {
          this.alertify.error('Nie udało się złożyć zamówienia' + error);
        });
      this.isSubmited = true;
    }
  }
  cancel() {
    this.router.navigate(['/home']);
  }
}
