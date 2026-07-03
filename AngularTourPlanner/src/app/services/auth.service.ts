import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { tap } from 'rxjs/operators';

/*export interface LoginResponse {
  token: string;
}*/

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private http: HttpClient) { }

  login(username: string, password: string) {
    return this.http.post<any>('http://localhost:5270/api/auth/login', {
      username,
      password
    }).pipe(tap(response => {
        localStorage.setItem('token', response.token);
      })
    );
  }

  register(username: string, email: string, password: string) {
    return this.http.post(
      'http://localhost:5270/api/auth/register',
      {
        username,
        email,
        password
      }
    );
  }

  logout() {
    localStorage.removeItem('token');
  }

  isLoggedIn(): boolean {
    return !!localStorage.getItem('token');
  }
}
