import { Component, inject } from '@angular/core';
import { Router, RouterLink, RouterModule } from '@angular/router';
import { EmployeeModel } from '../../models/Employee.model';

@Component({
  selector: 'app-header',
  imports: [RouterModule, RouterLink],
  templateUrl: './header.html',
  styleUrl: './header.css',
})
export class Header {

  isCollapsed: boolean = false;
  isMobileOpen: boolean = false;

  router = inject(Router)

  loggedEmpData: EmployeeModel = new EmployeeModel();
  isHR: boolean = false;

  constructor(){
    const localData = localStorage.getItem("empLoginUser")
    if(localData != null){
      this.loggedEmpData = JSON.parse(localData)

      if(this.loggedEmpData.role == "HR"){
        this.isHR = true;
      }
    }
  }

  toggleSidebar(): void {

    if(window.innerWidth < 768){
      this.isMobileOpen = !this.isMobileOpen;
    } else {
      this.isCollapsed = !this.isCollapsed;
    }

  }

  // ⭐ NEW FUNCTION
  closeMobileSidebar(){
    if(window.innerWidth < 768){
      this.isMobileOpen = false;
    }
  }

  onLogOut(){
    localStorage.removeItem('empLoginUser')
    this.router.navigateByUrl("/login")
  }

}