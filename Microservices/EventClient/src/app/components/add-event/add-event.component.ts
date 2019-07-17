import { EventService } from './../../services/event.service';
import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-add-event',
  templateUrl: './add-event.component.html',
  styleUrls: ['./add-event.component.css']
})
export class AddEventComponent implements OnInit {

  form: FormGroup;

  constructor(
    private fb: FormBuilder,
    private eventService: EventService
  ) { }

  ngOnInit() {
    this.form = this.fb.group({
      title: ['', Validators.required],
      speaker: ['', Validators.required],
      startDate: ['', Validators.required],
      endDate: ['', Validators.required],
      startTime: ['', Validators.required],
      endTime: ['', Validators.required],
      host: ['', Validators.required],
      registrationUrl: ['', Validators.required]
    });
  }

  addEvent() {
    if (this.form.valid) {
      this.eventService.addEvent(this.form.value)
        .subscribe(
          result => {
            console.log(result);
            alert('Successfully registered');
          },
          err => {
            alert('error');
            console.log(err);
          }
        );
    } else {
      alert('Invalid form');
    }
  }

}
