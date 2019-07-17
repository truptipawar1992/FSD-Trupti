import { EventService } from './../../services/event.service';
import { Observable } from 'rxjs';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  public events: Observable<any>;

  constructor(private eventService: EventService) { }

  ngOnInit() {
    this.events = this.eventService.getAllEvents();
  }

}
