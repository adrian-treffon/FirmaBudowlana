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
    }, error => {
      this.alertify.error('Nie udało się załadować listy zespołów: ' + error);
    });
  }

  deleteTeam(id: string) {
    this.alertify.confirm('Czy na pewno chcesz usunąć ten zespół?', () => {
      this.adminService.deleteTeam(id).subscribe(() => {
        this.alertify.success('Zespół został usunięty');
        this.ngOnInit();
      }, error => {
        this.alertify.error('Błąd podczas usuwania zespołu!');
      });
    });
  }
}
