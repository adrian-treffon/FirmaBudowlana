import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  registerMode: any = false;

  constructor(public authService: AuthService) { }

  ngOnInit() {
<<<<<<< HEAD
=======
    console.log(this.authService.isUser());
>>>>>>> adcae90761bb37eeb6e22c490764c1fd90e6ae80
  }

  activateRegisterMode() {
    this.registerMode = true;
  }

  cancelRegisterMode(registerMode: boolean) {
    this.registerMode = registerMode;
  }

}
