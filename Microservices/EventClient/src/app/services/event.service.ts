import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class EventService {

  private readonly BASE_URL: string = 'http://localhost:62408/api/events';

  constructor(private http: HttpClient) { }

  getAllEvents(): Observable<any> {
    return this.http.get(this.BASE_URL, {
      headers: {
        'Content-type': 'application/json',
        Accept: 'application/json'
      }
    });
  }

  addEvent(event: any): Observable<any> {
    const token = localStorage.getItem('auth_token');

    return this.http.post(this.BASE_URL, event, {
      headers: {
        'Content-type': 'application/json',
        Accept: 'application/json',
        Authorization: `Bearer ${token}`
      }
    });
  }
}
