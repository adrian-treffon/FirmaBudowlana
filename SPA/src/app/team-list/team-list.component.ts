import { Component, OnInit } from '@angular/core';
import { Team } from '../_models/team';
import { AdminService } from '../_services/admin.service';
import { AlertifyService } from '../_services/alertify.service';
import { NavigationExtras, Router } from '@angular/router';

@Component({
  selector: 'app-team-list',
  templateUrl: './team-list.component.html',
  styleUrls: ['./team-list.component.css']
})
export class TeamListComponent implements OnInit {
  teamList: Team[];

  constructor(private adminService: AdminService, private alertify: AlertifyService, private router: Router) { }

  ngOnInit() {
    this.loadTeams();
  }

  loadTeams() {
    this.adminService.getTeams().subscribe((teamsTemp: Team[]) => {
        this.teamList = teamsTemp;
    }, error => {
      this.alertify.error('Nie udało się załadować listy zespołów: ' + error);
    });
  }

  deleteTeam(id: string) {
    this.alertify.confirm('Czy na pewno chcesz rozwiązać ten zespół?', () => {
      this.adminService.deleteTeam(id).subscribe(() => {
        this.alertify.success('Zespół został rozwiązany');
        this.ngOnInit();
      }, error => {
        this.alertify.error(error);
      });
    });
  }

  editTeam(team: Team) {
    const navigationExtras: NavigationExtras = {
      queryParams: {
        team: JSON.stringify(team)
      }
    };

    this.router.navigate(['edit-team'], navigationExtras);
  }
}
