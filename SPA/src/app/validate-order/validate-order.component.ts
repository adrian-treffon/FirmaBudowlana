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
  orderFromList: Order = null;
  bsConfig: Partial<BsDatepickerConfig>;
  validateOrderForm: FormGroup;
  allTeamsList: Team[];
  teamsToAllocate: Team[] = new Array();

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
      console.log(params);
      this.orderFromList = params.order;
    });

    console.log(this.teams);
    this.createValidateOrderForm();
    this.dropdownList = [
      {item_id: 1, item_text: 'Test bo nie ma zespołów XD'}
    ];
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

  }

  cancel() {
    this.router.navigate(['/order-list']);
  }
}
