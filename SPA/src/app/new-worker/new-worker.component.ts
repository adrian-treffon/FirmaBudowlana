import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AlertifyService } from '../_services/alertify.service';
import { AdminService } from '../_services/admin.service';
import { Worker } from '../_models/worker';

@Component({
  selector: 'app-new-worker',
  templateUrl: './new-worker.component.html',
  styleUrls: ['./new-worker.component.css']
})
export class NewWorkerComponent implements OnInit {
  worker: Worker;
  newWorkerForm: FormGroup;

  constructor(private fb: FormBuilder, private adminService: AdminService, private router: Router, private alertify: AlertifyService) { }

  ngOnInit() {
    this.createNewWorkerForm();
  }

  createNewWorkerForm() {
    this.newWorkerForm = this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      specialization: ['', Validators.required],
      manHour: ['', Validators.required]
    });
  }

  addWorker() {
    if (this.newWorkerForm.valid) {
      this.worker = Object.assign({}, this.newWorkerForm.value);
      this.adminService.addNewWorker(this.worker).subscribe(() => {
      this.alertify.success('Dodano nowego pracownika!');
      }, error => {
        this.alertify.error('Błąd podczas dodawania nowego pracownika' + error);
      }, () => {
          this.router.navigate(['/worker-list']);
      });
    }
  }

  cancel() {
    this.router.navigate(['/worker-list']);
  }

}
