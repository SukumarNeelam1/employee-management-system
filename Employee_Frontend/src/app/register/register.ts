import { HttpClient } from '@angular/common/http';
import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';

@Component({
  selector: 'app-register',
  imports: [FormsModule,RouterModule],
  templateUrl: './register.html',
  styleUrl: './register.css'
})
export class Register {

  registerObj: any = {
    name: '',
    email: '',
    password: ''
  };

  confirmPassword: string = '';

  http = inject(HttpClient);
  router = inject(Router);

  onRegister() {

    if (this.registerObj.password !== this.confirmPassword) {
      alert("Passwords do not match");
      return;
    }

    this.http.post("https://localhost:7117/api/Auth/register", this.registerObj)
      .subscribe({
        next: (res: any) => {

          alert("Registration successful");

          this.router.navigateByUrl("login");

        },

        error: (err: any) => {
          alert(err.error);
        }
      });
  }
}