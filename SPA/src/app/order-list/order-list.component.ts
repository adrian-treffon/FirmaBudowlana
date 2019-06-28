import { Component, OnInit } from '@angular/core';
<<<<<<< HEAD
import { AdminService } from '../_services/admin.service';
import { AlertifyService } from '../_services/alertify.service';
import { Router } from '@angular/router';
import { OrderWithUser } from '../_models/orderWithUser';
=======
import { Order } from '../_models/order';
import { AdminService } from '../_services/admin.service';
import { AlertifyService } from '../_services/alertify.service';
import { Router } from '@angular/router';
>>>>>>> adcae90761bb37eeb6e22c490764c1fd90e6ae80

@Component({
  selector: 'app-order-list',
  templateUrl: './order-list.component.html',
  styleUrls: ['./order-list.component.css']
})
export class OrderListComponent implements OnInit {
<<<<<<< HEAD
  orderList: OrderWithUser[];
  selectedOrder: OrderWithUser;
=======
  orderList: Order[];
  selectedOrder: Order;
>>>>>>> adcae90761bb37eeb6e22c490764c1fd90e6ae80
  editMode = false;

  constructor(private adminService: AdminService, private alertify: AlertifyService, private router: Router) { }

  ngOnInit() {
    this.loadOrders();
  }

  loadOrders() {
<<<<<<< HEAD
    this.adminService.getUnvalidatedOrdersWithUser().subscribe((ordersTemp: OrderWithUser[]) => {
=======
    this.adminService.getUnvalidatedOrders().subscribe((ordersTemp: Order[]) => {
>>>>>>> adcae90761bb37eeb6e22c490764c1fd90e6ae80
        this.orderList = ordersTemp;
    }, error => {
      this.alertify.error('Nie udało się załadować listy zleceń: ' + error);
    });
  }

<<<<<<< HEAD
  onEditMode(inputOrder: OrderWithUser) {
=======
  onEditMode(inputOrder: Order) {
>>>>>>> adcae90761bb37eeb6e22c490764c1fd90e6ae80
    this.selectedOrder = inputOrder;
    this.editMode = true;
  }

  deleteOrder(id: string) {
    this.alertify.confirm('Czy na pewno chcesz usunąć to zamówienie?', () => {
      this.adminService.deleteOrder(id).subscribe(() => {
        this.alertify.success('Zlecenie zostało usunięte');
        this.ngOnInit();
      }, error => {
        this.alertify.error('Błąd podczas usuwania zlecenia!');
      });
    });
  }
}
