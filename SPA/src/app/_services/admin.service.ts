import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { Worker } from '../_models/worker';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  baseUrl: any = 'http://localhost:5000/';

constructor(private httpClient: HttpClient) { }

getWorkers(): Observable<Worker[]> {
  return this.httpClient.get<Worker[]>(this.baseUrl + 'comparison/workers');
}
}
