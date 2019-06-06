import { Component, OnInit, Output } from '@angular/core';
import { EventEmitter } from '@angular/core';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { User } from '../_models/user';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  user: User;
  @Output() cancelRegister = new EventEmitter();
  registerForm: FormGroup;

  constructor(private fb: FormBuilder, private authServide: AuthService, private router: Router, private alertify: AlertifyService) { }

  ngOnInit() {
    this.createRegisterForm();
  }

  createRegisterForm() {
    this.registerForm = this.fb.group({
      email: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', [Validators.required, Validators.minLength(6)]],
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      address: ['', Validators.required]
    }, {validator: this.passwordMatchValidator});
  }

  passwordMatchValidator(formGroup: FormGroup) {
    const password = formGroup.get('password').value;
    const passwordConfirm = formGroup.get('confirmPassword').value;
    if (password === passwordConfirm) {
      return null;
    } else {
      return {mismatch: true};
    }
  }

  register() {
    if (this.registerForm.valid) {
      this.user = Object.assign({}, this.registerForm.value);
      this.authServide.register(this.user).subscribe(() => {
      this.alertify.success('Zarejestrowano sie!');
      }, error => {
        this.alertify.error('Nie udało się zarejestrować' + error);
      }, () => {
        this.authServide.login(this.user).subscribe(() => {
          this.router.navigate(['/orders']);
        });
      });
    }
  }

  cancel() {
    this.cancelRegister.emit(false);
  }
}
