import { Component, OnInit } from '@angular/core';
import { Team } from '../_models/team';
import { AdminService } from '../_services/admin.service';
import { AlertifyService } from '../_services/alertify.service';

@Component({
  selector: 'app-team-list',
  templateUrl: './team-list.component.html',
  styleUrls: ['./team-list.component.css']
})
export class TeamListComponent implements OnInit {
  teamList: Team[];

  constructor(private adminService: AdminService, private alertify: AlertifyService) { }

  ngOnInit() {
    this.loadTeams();
  }

  loadTeams() {
    this.adminService.getTeams().subscribe((teamsTemp: Team[]) => {
        this.teamList = teamsTemp;
        console.log(this.teamList);
    }, error => {
      this.alertify.error('Nie udało się załadować listy zespołów: ' + error);
    });
  }
}
