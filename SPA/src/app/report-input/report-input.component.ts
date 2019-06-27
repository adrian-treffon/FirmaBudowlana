import { Component, OnInit } from '@angular/core';
import { ReportParams } from '../_models/report-params';
import { AdminService } from '../_services/admin.service';
import { ActivatedRoute, Router, NavigationExtras } from '@angular/router';
import { AlertifyService } from '../_services/alertify.service';
import { Team } from '../_models/team';
import { Worker } from '../_models/worker';
import { FormBuilder, FormGroup } from '@angular/forms';
import { DatePipe } from '@angular/common';
import { OrderWithUser } from '../_models/orderWithUser';

@Component({
  selector: 'app-report-input',
  templateUrl: './report-input.component.html',
  styleUrls: ['./report-input.component.css']
})
export class ReportInputComponent implements OnInit {
  isListMode = false;
  reportParams: ReportParams = new Object();
  reportInputOrderForm: FormGroup;
  teams: Team[] = new Array();
  workers: Worker[] = new Array();
  ordersReport: OrderWithUser[];
  rangeStartDate: Date;
  rangeEndDate: Date;
  firstAvaliableDate: Date;

  workersToSendList: Worker[] = new Array();
  selectedWorker: Worker;
  teamsToSendList: Team[] = new Array();
  selectedTeam: Team;

  workersDropdownList = [];
  workersSelectedItems = [];
  workersDropdownSettings = {};

  teamsDropdownList = [];
  teamsSelectedItems = [];
  teamsDropdownSettings = {};

  constructor(private adminService: AdminService, private route: ActivatedRoute, private datePipe: DatePipe,
              private alertify: AlertifyService, private router: Router, private fb: FormBuilder) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.teams = data.result.departments;
      this.workers = data.result.types;

      this.firstAvaliableDate = new Date('2019-06-01');
      new Date(this.firstAvaliableDate).getTime();
    });
    this.createReportInputOrderForm();

    for (let i = 0; i < this.workers.length; i++) {
      this.workersDropdownList.push({ item_id: i, item_text:
        this.workers[i].firstName + ' ' + this.workers[i].lastName + ', Specjalizacja: ' +
        this.workers[i].specialization});
    }

    this.workersDropdownSettings = {
      singleSelection: false,
      idField: 'item_id',
      textField: 'item_text',
      selectAllText: 'Zaznacz wszystkich',
      unSelectAllText: 'Odznacz wszystkich',
      searchPlaceholderText: 'Szukaj...',
      itemsShowLimit: 3,
      allowSearchFilter: true
    };

    for (let i = 0; i < this.teams.length; i++) {
      this.teamsDropdownList.push({ item_id: i, item_text:
        this.teams[i].description});
    }

    this.teamsDropdownSettings = {
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

  createReportInputOrderForm() {
    this.reportInputOrderForm = this.fb.group({
    range: ['']
  });
  }

  sendReportInput() {
    if (this.reportInputOrderForm.valid) {
      this.reportParams.startDate = this.reportInputOrderForm.value.range[0];
      this.reportParams.endDate = this.reportInputOrderForm.value.range[1];
      this.reportParams.teams = this.teamsToSendList;
      this.reportParams.workers = this.workersToSendList;

      if (this.reportParams.teams.length > 0 && this.reportParams.workers.length > 0) {
        this.alertify.alert('Jedna z list (lista zespołów albo lista pracowników) musi być pusta!', () => {
          return;
        });
      } else {
        this.adminService.getReport(this.reportParams).subscribe((data: any) => {
          this.isListMode = true;
          this.ordersReport = data;
        }, error => {
          this.alertify.error('Bład: ' + error);
        });
      }
    }
  }

  workersOnItemSelect(item: any) {
    this.workersToSendList.push(this.workers[item.item_id]);
  }
  workersOnSelectAll(items: any) {
    delete this.workersToSendList;
    this.workersToSendList = this.workers;
  }
  workersOnItemDeSelect(item: any) {
    const tempUnselectedWorker = this.workers[item.item_id];
    this.workersToSendList.splice(this.workersToSendList.findIndex(x => x.workerID === tempUnselectedWorker.workerID), 1);
  }
  teamsOnItemSelect(item: any) {
    this.teamsToSendList.push(this.teams[item.item_id]);
  }
  teamsOnSelectAll(items: any) {
    delete this.teamsToSendList;
    this.teamsToSendList = this.teams;
  }
  teamsOnItemDeSelect(item: any) {
    const tempUnselectedTeam = this.teams[item.item_id];
    this.teamsToSendList.splice(this.teamsToSendList.findIndex(x => x.teamID === tempUnselectedTeam.teamID), 1);
  }

  getEndDate(i: number) {
    if (this.ordersReport[i].validated === false) {
      this.ordersReport[i].endDate = null;
    }
    if (this.ordersReport[i].endDate === null || typeof this.ordersReport[i].endDate === 'undefined') {
      return 'Nie zaproponowano';
    } else {
      return this.datePipe.transform(this.ordersReport[i].endDate, 'dd-MM-yyyy');
    }
  }

  getCost(i: number) {
    if (this.ordersReport[i].cost === null || typeof this.ordersReport[i].cost === 'undefined' || this.ordersReport[i].cost === 0) {
      return 'Nie zaproponowano';
    } else {
      return this.ordersReport[i].cost + ' zł';
    }
  }

  getPaid(i: number) {
    if (this.ordersReport[i].paid === true) {
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

  isPaid(i: number) {
    if (this.ordersReport[i].paid === true) {
      return true;
    } else {
      return false;
    }
  }

  getButtonText(i: number) {
    if (this.ordersReport[i].paid === true) {
      return 'Opłacone';
    } else {
      return 'Edytuj';
    }
  }
}
