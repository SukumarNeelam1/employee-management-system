import { HttpClient } from '@angular/common/http';
import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';

@Component({
  selector: 'app-login',
  imports: [FormsModule, RouterModule],
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login {

  loginObj: any = {
    email: '',
    password: ''
  };

  http = inject(HttpClient);
  router = inject(Router);

  onLogin() {

    this.http.post("https://localhost:7117/api/Auth/login", this.loginObj)
      .subscribe({
        next: (result: any) => {

          localStorage.setItem('empLoginUser', JSON.stringify(result));

          if (result.role === "HR") {
            this.router.navigateByUrl("dashboard");
          }
          else {
            this.router.navigateByUrl("new-employee/" + result.employeeId);
          }
        },

        error: (error: any) => {
          alert(error.error);
        }
      });
  }
}