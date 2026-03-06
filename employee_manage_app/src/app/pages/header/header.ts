import { CommonModule, JsonPipe } from '@angular/common';
import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink, RouterModule } from '@angular/router';
import { EmployeeModel } from '../../models/Employee.model';

@Component({
  selector: 'app-header',
  imports: [RouterModule,RouterLink],
  templateUrl: './header.html',
  styleUrl: './header.css',
})
export class Header {
  isCollapsed: boolean = false;
  router = inject(Router)

  loggedEmpData: EmployeeModel = new EmployeeModel();

  constructor(){
    const localData = localStorage.getItem("empLoginUser")
    if(localData !=  null){
      this.loggedEmpData = JSON.parse(localData)
    }
  }

  toggleSidebar(): void {
    this.isCollapsed = !this.isCollapsed;
  }

  onLogOut(){
    localStorage.removeItem('empLoginUser')
    this.router.navigateByUrl("/login")
  }
}
