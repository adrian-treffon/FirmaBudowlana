import { Component, OnInit } from '@angular/core';
<<<<<<< HEAD
=======
import { Order } from '../_models/order';
>>>>>>> adcae90761bb37eeb6e22c490764c1fd90e6ae80
import { AdminService } from '../_services/admin.service';
import { AlertifyService } from '../_services/alertify.service';
import { Router, ActivatedRoute, NavigationExtras } from '@angular/router';
import { DatePipe } from '@angular/common';
<<<<<<< HEAD
import { OrderWithUser } from '../_models/orderWithUser';
=======
>>>>>>> adcae90761bb37eeb6e22c490764c1fd90e6ae80

@Component({
  selector: 'app-orders-all-list',
  templateUrl: './orders-all-list.component.html',
  styleUrls: ['./orders-all-list.component.css']
})
export class OrdersAllListComponent implements OnInit {
<<<<<<< HEAD
  orderList: OrderWithUser[];
=======
  orderList: Order[];
>>>>>>> adcae90761bb37eeb6e22c490764c1fd90e6ae80

  constructor(private adminService: AdminService, private alertify: AlertifyService, private router: Router,
              private datePipe: DatePipe, private route: ActivatedRoute) { }

  ngOnInit() {
    this.loadOrders();
  }

  loadOrders() {
    this.route.data.subscribe(data => {
      this.orderList = data.orders;
<<<<<<< HEAD
      console.log(this.orderList);
=======
>>>>>>> adcae90761bb37eeb6e22c490764c1fd90e6ae80
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

<<<<<<< HEAD
  editOrder(order: OrderWithUser) {
=======
  editOrder(order: Order) {
>>>>>>> adcae90761bb37eeb6e22c490764c1fd90e6ae80
    const navigationExtras: NavigationExtras = {
      queryParams: {
        order: JSON.stringify(order)
      }
    };

    this.router.navigate(['edit-validated-order'], navigationExtras);
  }
<<<<<<< HEAD

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
=======
>>>>>>> adcae90761bb37eeb6e22c490764c1fd90e6ae80
}
