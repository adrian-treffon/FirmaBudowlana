import { Component, OnInit } from '@angular/core';
import { AdminService } from '../_services/admin.service';
import { AlertifyService } from '../_services/alertify.service';
import { Router, ActivatedRoute, NavigationExtras } from '@angular/router';
import { DatePipe } from '@angular/common';
import { OrderWithUser } from '../_models/orderWithUser';

@Component({
  selector: 'app-orders-all-list',
  templateUrl: './orders-all-list.component.html',
  styleUrls: ['./orders-all-list.component.css']
})
export class OrdersAllListComponent implements OnInit {
  orderList: OrderWithUser[];

  constructor(private adminService: AdminService, private alertify: AlertifyService, private router: Router,
              private datePipe: DatePipe, private route: ActivatedRoute) { }

  ngOnInit() {
    this.loadOrders();
  }

  loadOrders() {
    this.route.data.subscribe(data => {
      this.orderList = data.orders;
      console.log(this.orderList);
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

  getCost(i: number) {
    if (this.orderList[i].cost === null || typeof this.orderList[i].cost === 'undefined' || this.orderList[i].cost === 0) {
      return 'Nie zaproponowano';
    } else {
      return this.orderList[i].cost + ' zł';
    }
  }

  getPaid(i: number) {
    if (this.orderList[i].paid === true) {
      return 'Tak';
    } else {
        return 'Nie';
      }
  }

  editOrder(order: OrderWithUser) {
    const navigationExtras: NavigationExtras = {
      queryParams: {
        order: JSON.stringify(order)
      }
    };

    this.router.navigate(['edit-validated-order'], navigationExtras);
  }

  isPaid(order: OrderWithUser)
  {
    if (order.paid === true) {
      return true;
    } else {
      return false;
    }
  }

  getButtonText(i: number) {
    if (this.orderList[i].paid) {
      return 'Opłacone';
    } else {
      return 'Edytuj';
    }
  }
}
