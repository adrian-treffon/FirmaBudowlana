import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { Worker } from '../_models/worker';
import { Team } from '../_models/team';
import { Order } from '../_models/order';
import { Payment } from '../_models/payment';
import { ReportParams } from '../_models/report-params';
import { OrderWithUser } from '../_models/orderWithUser';



@Injectable({
  providedIn: 'root'
})
export class AdminService {
  baseUrl: any = 'http://localhost:5000/';

  constructor(private httpClient: HttpClient) { }

  getWorkers(): Observable<Worker[]> {
    return this.httpClient.get<Worker[]>(this.baseUrl + 'comparison/workers');
  }

  getAllWorkers(): Observable<Worker[]> {
    return this.httpClient.get<Worker[]>(this.baseUrl + 'comparison/AllWorkers');
  }

  getUnpaidPayments(): Observable<Order[]> {
    return this.httpClient.get<Order[]>(this.baseUrl + 'add/payment');
  }

  getPaidPayments(): Observable<Payment[]> {
    return this.httpClient.get<Payment[]>(this.baseUrl + 'comparison/payments');
  }

  addPayment(order: Order) {
    return this.httpClient.post(this.baseUrl + 'add/payment', order);
  }

  addNewWorker(worker: Worker) {
    return this.httpClient.post(this.baseUrl + 'add/worker', worker);
  }

  addNewTeam(team: Team) {
    return this.httpClient.post(this.baseUrl + 'add/team', team);
  }

  validateOrder(order: Order) {
    return this.httpClient.post(this.baseUrl + 'order/validate', order);
  }

  editWorker(worker: Worker) {
    return this.httpClient.post(this.baseUrl + 'edit/worker', worker);
  }

  editTeam(team: Team) {
    return this.httpClient.post(this.baseUrl + 'edit/team', team);
  }

  editOrder(order: Order) {
    return this.httpClient.post(this.baseUrl + 'edit/order', order);
  }

  getReport(reportParams: ReportParams) {
    return this.httpClient.post(this.baseUrl + 'comparison/report', reportParams);
  }

  getAllTeams(): Observable<Team[]> {
    return this.httpClient.get<Team[]>(this.baseUrl + 'comparison/AllTeams');
  }

  getTeams(): Observable<Team[]> {
    return this.httpClient.get<Team[]>(this.baseUrl + 'comparison/teams');
  }

  getUnvalidatedOrders(): Observable<Order[]> {
    return this.httpClient.get<Order[]>(this.baseUrl + 'order/showInvalidated');
  }

  getUnvalidatedOrdersWithUser(): Observable<OrderWithUser[]> {
    return this.httpClient.get<OrderWithUser[]>(this.baseUrl + 'order/showInvalidated');
  }

  getAllOrders(): Observable<Order[]> {
    return this.httpClient.get<Order[]>(this.baseUrl + 'comparison/orders');
  }

  getAllOrdersWithUser(): Observable<OrderWithUser[]> {
    return this.httpClient.get<OrderWithUser[]>(this.baseUrl + 'comparison/orders');
  }

  getOrder(id: string): Observable<Order> {
    return this.httpClient.get<Order>(this.baseUrl + 'order/validate/' + id);
  }

  deleteOrder(id: string): Observable<any> {
    return this.httpClient.get<any>(this.baseUrl + 'delete/order/' + id);
  }

  deleteTeam(id: string): Observable<any> {
    return this.httpClient.get<any>(this.baseUrl + 'delete/team/' + id);
  }

  getWorker(id: string): Observable<Worker> {
    return this.httpClient.get<Worker>(this.baseUrl + 'comparison/workers/' + id);
  }

  deleteWorker(id: string): Observable<any> {
    return this.httpClient.get<any>(this.baseUrl + 'delete/worker/' + id);
  }
}
