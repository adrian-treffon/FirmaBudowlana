import { Component, OnInit } from '@angular/core';
import { Team } from '../_models/team';
import { Worker } from '../_models/worker';
import { ActivatedRoute, Router } from '@angular/router';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { AdminService } from '../_services/admin.service';
import { AlertifyService } from '../_services/alertify.service';

@Component({
  selector: 'app-edit-team',
  templateUrl: './edit-team.component.html',
  styleUrls: ['./edit-team.component.css']
})
export class EditTeamComponent implements OnInit {
  team: Team;
  teamToSend: Team;
  editTeamForm: FormGroup;
  allWorkersList: Worker[];
  workersToSendList: Worker[] = new Array();
  selectedWorker: Worker;

  dropdownList = [];
  selectedItems = [];
  dropdownSettings = {};

  constructor(private fb: FormBuilder, private adminService: AdminService, private route: ActivatedRoute,
              private alertify: AlertifyService, private router: Router) { }

  ngOnInit() {
    this.route.queryParams.subscribe(params => {
      this.team = JSON.parse(params.team);
      this.workersToSendList = this.team.workers;
    });

    this.route.data.subscribe(data => {
      this.allWorkersList = data.workers;
    });

    for (let i = 0; i < this.allWorkersList.length; i++) {
      this.dropdownList.push({ item_id: i, item_text:
        this.allWorkersList[i].firstName + ' ' + this.allWorkersList[i].lastName + ', Specjalizacja: ' +
        this.allWorkersList[i].specialization});
    }

    // When there will be two workers with exactly same first and second names and specialisation this won't work,
    // find better way
    for (let i = 0; i < this.team.workers.length; i++) {
      this.selectedItems.push({
        item_id: this.dropdownList.find(x => x.item_text === this.team.workers[i].firstName
        + ' ' + this.team.workers[i].lastName + ', Specjalizacja: ' + this.team.workers[i].specialization).item_id,
        item_text: this.team.workers[i].firstName + ' ' + this.team.workers[i].lastName + ', Specjalizacja: ' +
        this.team.workers[i].specialization});
    }


    this.dropdownSettings = {
      singleSelection: false,
      idField: 'item_id',
      textField: 'item_text',
      selectAllText: 'Zaznacz wszystkich',
      unSelectAllText: 'Odznacz wszystkich',
      searchPlaceholderText: 'Szukaj...',
      itemsShowLimit: 3,
      allowSearchFilter: true
    };

    this.createEditTeamForm();
  }

  onItemSelect(item: any) {
    this.workersToSendList.push(this.allWorkersList[item.item_id]);
  }
  onSelectAll(items: any) {
    delete this.workersToSendList;
    this.workersToSendList = this.allWorkersList;
  }
  onItemDeSelect(item: any) {
    const tempUnselectedWorker = this.allWorkersList[item.item_id];
    this.workersToSendList.splice(this.workersToSendList.findIndex(x => x.workerID === tempUnselectedWorker.workerID), 1);
  }

  loadWorkers() {
    this.adminService.getWorkers().subscribe((workersTemp: Worker[]) => {
      this.allWorkersList = workersTemp;
    }, error => {
      this.alertify.error('Nie udało się załadować listy pracowników: ' + error);
    });
  }

  createEditTeamForm() {
     this.editTeamForm = this.fb.group({
      description: [this.team.description, Validators.required],
    });
  }

  editTeam() {
    if (this.editTeamForm.valid) {
      this.teamToSend = Object.assign({}, this.team, this.editTeamForm.value);
      this.teamToSend.workers = this.workersToSendList;
      this.adminService.editTeam(this.teamToSend).subscribe(() => {
      this.alertify.success('Edytowano zespół');
      }, error => {
        this.alertify.error(error);
      }, () => {
          this.router.navigate(['/team-list']);
      });
    }
  }

  cancel() {
    this.router.navigate(['/team-list']);
  }
}
