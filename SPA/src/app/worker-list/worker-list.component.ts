import { Component, OnInit } from '@angular/core';
import { AdminService } from '../_services/admin.service';
import { AlertifyService } from '../_services/alertify.service';

import { Worker } from '../_models/worker';
import { Team } from '../_models/team';


@Component({
  selector: 'app-worker-list',
  templateUrl: './worker-list.component.html',
  styleUrls: ['./worker-list.component.css']
})
export class WorkerListComponent implements OnInit {
  workers: Worker[];
  teams: Team[];

  constructor(private adminService: AdminService, private alertify: AlertifyService) { }

  ngOnInit() {
    this.loadWorkers();
  }

  loadWorkers() {
    this.adminService.getWorkers().subscribe((workersTemp: Worker[]) => {
      this.workers = workersTemp;
    }, error => {
      this.alertify.error('Nie udało się załadować listy pracowników: ' + error);
    });
  }

  deleteWorker(id: string) {
    this.alertify.confirm('Czy na pewno chcesz zwolnić tego pracownika?', () => {
      this.adminService.deleteWorker(id).subscribe(() => {
        this.alertify.success('Pracownik został zwolniony');
        this.ngOnInit();
      }, error => {
        this.alertify.error(error);
      });
    });
  }
}
