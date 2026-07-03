import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { TourLog } from '../models/tour-log';

@Injectable({
  providedIn: 'root'
})
export class TourLogService {

  private apiUrl = 'http://localhost:5270/api/tourlogs';
  
  constructor(private http: HttpClient) { }

  getLogsByTour(tourId: number) {
    return this.http.get<TourLog[]>(`${this.apiUrl}/${tourId}`);
  }
  createLog(log: any) {
    return this.http.post(this.apiUrl, log);
  }
  deleteLog(id: number) {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }

  updateLog(id: number, log: any) {
    return this.http.put(`${this.apiUrl}/${id}`, log);
  }
}
