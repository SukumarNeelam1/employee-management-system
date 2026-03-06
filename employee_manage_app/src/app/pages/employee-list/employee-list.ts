import { Component, inject, OnInit, signal } from '@angular/core';
import { IEmployeeListModel } from '../../models/Employee.model';
import { EmployeeService } from '../../services/employee-service';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-employee-list',
  imports: [RouterLink],
  templateUrl: './employee-list.html',
  styleUrl: './employee-list.css',
})
export class EmployeeList implements OnInit{
  employeeList = signal<IEmployeeListModel[]>([])
  empSrv = inject(EmployeeService)

  // Pageload
  ngOnInit(): void {
    this.getAllEmp()
  }

  getAllEmp(){
  this.empSrv.getAllEmployee().subscribe({
    next: (res: IEmployeeListModel[]) => {
      this.employeeList.set(res);
    },
    error: (err) => {
      console.error(err);
    }
  });
}
}
