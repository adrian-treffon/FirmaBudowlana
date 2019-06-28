import { Component, OnInit, Input } from '@angular/core';
import { Team } from '../_models/team';
import { Worker } from '../_models/worker';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { AdminService } from '../_services/admin.service';
import { Router, ActivatedRoute } from '@angular/router';
import { AlertifyService } from '../_services/alertify.service';

@Component({
  selector: 'app-new-team',
  templateUrl: './new-team.component.html',
  styleUrls: ['./new-team.component.css']
})
export class NewTeamComponent implements OnInit {
  team: Team;
  newTeamForm: FormGroup;
  allWorkersList: Worker[];
  workersToSendList: Worker[] = new Array();
  selectedWorker: Worker;

  dropdownList = [];
  selectedItems = [];
  dropdownSettings = {};

  constructor(private fb: FormBuilder, private adminService: AdminService, private route: ActivatedRoute,
              private alertify: AlertifyService, private router: Router) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.allWorkersList = data.workers;
    });
    this.createNewTeamForm();

    for (let i = 0; i < this.allWorkersList.length; i++) {
      this.dropdownList.push({ item_id: i, item_text:
        this.allWorkersList[i].firstName + ' ' + this.allWorkersList[i].lastName + ', Specjalizacja: ' +
        this.allWorkersList[i].specialization});
    }

    this.selectedItems = [];
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

  createNewTeamForm() {
     this.newTeamForm = this.fb.group({
      description: ['', Validators.required],
    });
  }

  addTeam() {
    if (this.newTeamForm.valid) {
      this.team = Object.assign({}, this.newTeamForm.value);
      this.team.workers = this.workersToSendList;
      this.adminService.addNewTeam(this.team).subscribe(() => {
      this.alertify.success('Dodano nowy zespół');
      }, error => {
        this.alertify.error('Błąd podczas dodawania nowego zespołu' + error);
      }, () => {
          this.router.navigate(['/team-list']);
      });
    }
  }

  cancel() {
    this.router.navigate(['/worker-list']);
  }
}
