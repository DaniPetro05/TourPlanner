import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class GeocodingService {

  constructor(private http: HttpClient) { }

  search(location: string) {
    const url = `https://nominatim.openstreetmap.org/search?format=json&q=${location}`;
    return this.http.get<any[]>(url);
  }
}
