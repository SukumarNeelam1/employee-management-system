import { HttpClient } from '@angular/common/http';
import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { errorContext } from 'rxjs/internal/util/errorContext';

@Component({
  selector: 'app-login',
  imports: [FormsModule],
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login {
  loginObj: any = {
    email: '',
    contactNo: ''
  };

  http = inject(HttpClient);
  router = inject(Router)

  onLogin() {
    this.http.post("https://localhost:7117/api/EmployeeMaster/login", this.loginObj)
      .subscribe({
        next: (result: any) => {
          localStorage.setItem('empLoginUser', JSON.stringify(result.data));
          if (result.data.role === "HR") {
            this.router.navigateByUrl("dashboard");
          }
          else {
            this.router.navigateByUrl("new-employee/" + result.data.employeeId);
          }
        },
        error: (error: any) => {
          alert(error.error.message);
        }
      });
  }
}
