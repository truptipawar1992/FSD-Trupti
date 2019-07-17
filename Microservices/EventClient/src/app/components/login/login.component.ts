import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  form: FormGroup;

  constructor(
    private fb: FormBuilder,
    private userService: UserService
  ) { }

  ngOnInit() {
    this.form = this.fb.group({
      email: ['', Validators.required],
      password: ['', Validators.required]
    });
  }

  login() {
    if (this.form.valid) {
      this.userService.getJwtToken(this.form.value)
      .subscribe(
        result => {
          localStorage.setItem('auth_token', result.token);
          alert('Login success');
        },
        err => {
          alert('Login failed');
          console.log(err);
        }
      );
    } else {
      alert('Invalid form');
    }
  }


}
