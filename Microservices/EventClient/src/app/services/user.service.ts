import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  private readonly BASE_URL: string = 'http://localhost:62800/api/identity';

  constructor(private http: HttpClient) { }

  registerUser(user: any): Observable<any> {
    return this.http.post(`${this.BASE_URL}/register`, user, {
      headers: {
        'Content-type': 'application/json',
        Accept: 'application/json'
      }
    });
  }

  getJwtToken(userLogin: any): Observable<any> {
    return this.http.post(`${this.BASE_URL}/token`, userLogin, {
      headers: {
        'Content-type': 'application/json',
        Accept: 'application/json'
      }
    });
  }

  // loginUser(): Observable<any> {
  //   return this.http.post(`${this.BASE_URL}/register`, user, {
  //     headers: {
  //       'Content-type': 'application/json',
  //       Accept: 'application/json'
  //     }
  //   });
  // }
}
