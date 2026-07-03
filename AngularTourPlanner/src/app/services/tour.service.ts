import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Tour } from '../tour';

@Injectable({
  providedIn: 'root'
})
export class TourService {

  private apiUrl = 'http://localhost:5270/api/tours';

  constructor(private http: HttpClient) { }

  getTours(): Observable<Tour[]> {
    return this.http.get<Tour[]>(this.apiUrl);
  }

  getTourById(id: number): Observable<Tour> {
    return this.http.get<Tour>(`${this.apiUrl}/${id}`);
  }

  createTour(tour: any): Observable<Tour> {
    return this.http.post<Tour>(this.apiUrl, tour);
  }

  deleteTour(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  updateTour(id: number, tour: any): Observable<Tour> {
    return this.http.put<Tour>(`${this.apiUrl}/${id}`, tour);
  }

  getTransportTypes() {
    //return this.http.get<string[]>(`${this.apiUrl}/transport`);
    return this.http.get<string[]>('http://localhost:5270/api/transport');
  }
}
