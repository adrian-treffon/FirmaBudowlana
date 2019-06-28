import { Component, OnInit, Input } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker/public_api';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AdminService } from '../_services/admin.service';
import { AlertifyService } from '../_services/alertify.service';
import { Team } from '../_models/team';
import { Order } from '../_models/order';

@Component({
  selector: 'app-validate-order',
  templateUrl: './validate-order.component.html',
  styleUrls: ['./validate-order.component.css']
})
export class ValidateOrderComponent implements OnInit {
  teams: Team[] = new Array();
  orderId: string = null;
  order: Order = null;
  validateOrderForm: FormGroup;
  allTeamsList: Team[];
  teamsToAllocate: Team[] = new Array();
  startDate;

  dropdownList = [];
  selectedItems = [];
  dropdownSettings = {};

  constructor(private fb: FormBuilder, private adminService: AdminService, private route: ActivatedRoute,
              private alertify: AlertifyService, private router: Router) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.teams = data.teams;
    });
    this.route.params.subscribe(params => {
      this.orderId = params.id;
      this.adminService.getOrder(this.orderId).subscribe((orderTemp: Order) => {
        this.order = orderTemp;
        this.startDate = new Date(this.order.startDate);
        new Date(this.startDate).getTime();
      }, error => {
        this.alertify.error('Nie udało się załadować zamówienia do edycji: ' + error);
      });
    });

    this.createValidateOrderForm();
    for (let i = 0; i < this.teams.length; i++) {
      this.dropdownList.push({ item_id: i, item_text:
        this.teams[i].description});
    }

    this.selectedItems = [];
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

  createValidateOrderForm() {
      this.validateOrderForm = this.fb.group({
      cost: ['', Validators.required],
      endDate: ['', Validators.required]
    });
  }

  validateOrder() {
    if (this.validateOrderForm.valid) {
      const tempOrderPart = Object.assign({}, this.validateOrderForm.value);
      this.order = Object.assign(this.order, tempOrderPart);
      this.order.teams = this.teamsToAllocate;

      this.adminService.validateOrder(this.order).subscribe(()  => {
        this.alertify.success('Zatwierdzono zamówienie');
      }, error => {
        this.alertify.error('Błąd podczas zatwierdzania zamówienia: ' + error);
      }, () => {
        this.router.navigate(['/order-list']);
      });
    }
  }

  cancel() {
    this.router.navigate(['/order-list']);
  }
}
