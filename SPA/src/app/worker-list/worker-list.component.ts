import { Component, OnInit } from '@angular/core';
import { AdminService } from '../_services/admin.service';
import { AlertifyService } from '../_services/alertify.service';

import { Worker } from '../_models/worker';

@Component({
  selector: 'app-worker-list',
  templateUrl: './worker-list.component.html',
  styleUrls: ['./worker-list.component.css']
})
export class WorkerListComponent implements OnInit {
  workers: Worker[];
  // selectedId: string;
  // isDetailMode = false;


  constructor(private adminService: AdminService, private alertify: AlertifyService) { }

  ngOnInit() {
    this.loadWorkers();
  }

  loadWorkers() {
    this.adminService.getWorkers().subscribe((workersTemp: Worker[]) => {
      this.workers = workersTemp;
      console.log(this.workers);
    }, error => {
      this.alertify.error('Nie udało się załadować listy pracowników: ' + error);
    });
  }

  // setSelectedId(inputId: string) {
  //   this.selectedId = inputId;
  //   console.log(this.selectedId);
  //   this.isDetailMode = true;
  // }
}
