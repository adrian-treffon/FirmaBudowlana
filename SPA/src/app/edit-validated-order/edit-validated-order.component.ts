import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AdminService } from '../_services/admin.service';
import { ActivatedRoute, Router } from '@angular/router';
import { AlertifyService } from '../_services/alertify.service';
import { Order } from '../_models/order';
import { Team } from '../_models/team';

@Component({
  selector: 'app-edit-validated-order',
  templateUrl: './edit-validated-order.component.html',
  styleUrls: ['./edit-validated-order.component.css']
})
export class EditValidatedOrderComponent implements OnInit {
  teams: Team[] = new Array();
  orderId: string = null;
  order: Order;
  orderToSend: Order;
  editOrderForm: FormGroup;
  allTeamsList: Team[];
  teamsToAllocate: Team[] = new Array();
  startDate;
  endDate;

  dropdownList = [];
  selectedItems = [];
  dropdownSettings = {};

  constructor(private fb: FormBuilder, private adminService: AdminService, private route: ActivatedRoute,
              private alertify: AlertifyService, private router: Router) { }

  ngOnInit() {
    this.route.queryParams.subscribe(params => {
      this.order = JSON.parse(params.order);
      this.teamsToAllocate = this.order.teams;
      this.startDate = new Date(this.order.startDate);
      new Date(this.startDate).getTime();
      this.endDate = new Date(this.order.endDate);
      new Date(this.endDate).getTime();
    });

    this.route.data.subscribe(data => {
      this.teams = data.teams;
    });

    for (let i = 0; i < this.teams.length; i++) {
      this.dropdownList.push({ item_id: i, item_text:
        this.teams[i].description});
    }

    for (let i = 0; i < this.order.teams.length; i++) {
      this.selectedItems.push({
        item_id: this.dropdownList.find(x => x.item_text === this.order.teams[i].description).item_id,
        item_text: this.order.teams[i].description});
    }

    this.dropdownSettings = {
      singleSelection: false,
      idField: 'item_id',
      textField: 'item_text',
      selectAllText: 'Zaznacz wszystkie zespoły',
      unSelectAllText: 'Odznacz wszystkie zespoły',
      searchPlaceholderText: 'Szukaj...',
      itemsShowLimit: 3,
      allowSearchFilter: true
    };
    this.createEditValidateOrderForm();
  }

  onItemSelect(item: any) {
    this.teamsToAllocate.push(this.teams[item.item_id]);
  }
  onSelectAll(items: any) {
    delete this.teamsToAllocate;
    this.teamsToAllocate = this.teams;
  }
  onItemDeSelect(item: any) {
    const tempUnselectedTeam = this.teams[item.item_id];
    this.teamsToAllocate.splice(this.teamsToAllocate.findIndex(x => x.teamID === tempUnselectedTeam.teamID), 1);
  }

  createEditValidateOrderForm() {
      this.editOrderForm = this.fb.group({
      cost: [this.order.cost, Validators.required],
      endDate: [this.endDate, Validators.required]
    });
  }

  editValidatedOrder() {
    if (this.editOrderForm.valid) {
      this.orderToSend = Object.assign({}, this.order, this.editOrderForm.value);
      this.orderToSend.teams = this.teamsToAllocate;

      this.adminService.editOrder(this.orderToSend).subscribe(()  => {
        this.alertify.success('Zatwierdzono zamówienie');
      }, error => {
        this.alertify.error('Błąd podczas zatwierdzania zamówienia: ' + error);
      }, () => {
        this.router.navigate(['/order-all-list']);
      });
    }
  }

  cancel() {
    this.router.navigate(['/order-all-list']);
  }
}
