import { Component, OnInit } from '@angular/core';
import { AlertifyService } from '../_services/alertify.service';
import { AdminService } from '../_services/admin.service';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Worker } from '../_models/worker';

@Component({
  selector: 'app-edit-worker',
  templateUrl: './edit-worker.component.html',
  styleUrls: ['./edit-worker.component.css']
})
export class EditWorkerComponent implements OnInit {
  worker: Worker;
  workerToSend: Worker;
  editWorkerForm: FormGroup;


  constructor(private adminService: AdminService, private route: ActivatedRoute,
              private alertify: AlertifyService, private router: Router, private fb: FormBuilder) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.worker = data.worker;
<<<<<<< HEAD
=======
      console.log(this.worker);
>>>>>>> adcae90761bb37eeb6e22c490764c1fd90e6ae80
    });

    this.createEditWorkerForm();
  }

  createEditWorkerForm() {
    this.editWorkerForm = this.fb.group({
      firstName: [this.worker.firstName, Validators.required],
      lastName: [this.worker.lastName, Validators.required],
      specialization: [this.worker.specialization, Validators.required],
      manHour: [this.worker.manHour, Validators.required]
    });
  }

  editWorker() {
    if (this.editWorkerForm.valid) {
      this.workerToSend = Object.assign({}, this.worker, this.editWorkerForm.value);
<<<<<<< HEAD
=======
      console.log(this.workerToSend);
>>>>>>> adcae90761bb37eeb6e22c490764c1fd90e6ae80
      this.adminService.editWorker(this.workerToSend).subscribe(() => {
      this.alertify.success('Edytowano pracownika!');
      }, error => {
        this.alertify.error('Błąd podczas edytowania pracownika' + error);
      }, () => {
          this.router.navigate(['/worker-list']);
      });
    }
  }

  cancel() {
    this.router.navigate(['/worker-list']);
  }
}
