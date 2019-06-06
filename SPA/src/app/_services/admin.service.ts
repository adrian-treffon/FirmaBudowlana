import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { Worker } from '../_models/worker';
import { Team } from '../_models/team';
import { Order } from '../_models/order';



@Injectable({
  providedIn: 'root'
})
export class AdminService {
  baseUrl: any = 'http://localhost:5000/';

  constructor(private httpClient: HttpClient) { }

  getWorkers(): Observable<Worker[]> {
    return this.httpClient.get<Worker[]>(this.baseUrl + 'comparison/workers');
  }

  addNewWorker(worker: Worker) {
    return this.httpClient.post(this.baseUrl + 'add/worker', worker);
  }

  addNewTeam(team: Team) {
    return this.httpClient.post(this.baseUrl + 'add/team', team);
  }

  getTeams(): Observable<Team[]> {
    return this.httpClient.get<Team[]>(this.baseUrl + 'comparison/teams');
  }

  getUnvalidatedOrders(): Observable<Order[]> {
    return this.httpClient.get<Order[]>(this.baseUrl + 'order/showInvalidated');
  }
}
