import { Component, OnInit } from '@angular/core';
import { Order } from '../_models/order';
import { AdminService } from '../_services/admin.service';
import { AlertifyService } from '../_services/alertify.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-order-list',
  templateUrl: './order-list.component.html',
  styleUrls: ['./order-list.component.css']
})
export class OrderListComponent implements OnInit {
  orderList: Order[];
  selectedOrder: Order;
  editMode = false;

  constructor(private adminService: AdminService, private alertify: AlertifyService, private router: Router) { }

  ngOnInit() {
    this.loadOrders();
  }

  loadOrders() {
    this.adminService.getUnvalidatedOrders().subscribe((ordersTemp: Order[]) => {
        this.orderList = ordersTemp;
        console.log(this.orderList);
    }, error => {
      this.alertify.error('Nie udało się załadować listy zleceń: ' + error);
    });
  }

  onEditMode(inputOrder: Order) {
    this.selectedOrder = inputOrder;
    this.editMode = true;
  }
}
