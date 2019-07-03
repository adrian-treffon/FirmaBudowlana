import { Component, OnInit } from '@angular/core';
import { AdminService } from '../_services/admin.service';
import { AlertifyService } from '../_services/alertify.service';
import { Router } from '@angular/router';
import { OrderWithUser } from '../_models/orderWithUser';

@Component({
  selector: 'app-order-list',
  templateUrl: './order-list.component.html',
  styleUrls: ['./order-list.component.css']
})
export class OrderListComponent implements OnInit {
  orderList: OrderWithUser[];
  selectedOrder: OrderWithUser;
  editMode = false;

  constructor(private adminService: AdminService, private alertify: AlertifyService, private router: Router) { }

  ngOnInit() {
    this.loadOrders();
  }

  loadOrders() {
    this.adminService.getUnvalidatedOrdersWithUser().subscribe((ordersTemp: OrderWithUser[]) => {
        this.orderList = ordersTemp;
    }, error => {
      this.alertify.error('Nie udało się załadować listy zleceń: ' + error);
    });
  }

  onEditMode(inputOrder: OrderWithUser) {
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
