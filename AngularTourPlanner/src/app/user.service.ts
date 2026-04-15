import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { inject } from '@angular/core';
import { environment } from '../environments/environment';
import { Observable } from 'rxjs';

export interface User {
  id?: number;
  username: string;
  password: string;
  email: string;
}

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor() { }
  private http = inject(HttpClient);
  private apiUrl = environment.apiURL + '/user';

  public get(): Observable<User> {
    return this.http.get<User>(this.apiUrl);
  }
}
