import { ChangeDetectorRef, Component, inject, OnInit } from '@angular/core';
import { EmployeeModel } from '../../models/Employee.model';
import { FormsModule } from "@angular/forms";
import { EmployeeService } from '../../services/employee-service';
import { Observable } from 'rxjs';
import { DesignationListModel } from '../../models/Department.model';
import { Master } from '../../services/master';
import { AsyncPipe } from '@angular/common';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-employee-form',
  standalone: true,
  imports: [FormsModule, AsyncPipe],
  templateUrl: './employee-form.html',
  styleUrl: './employee-form.css',
})
export class EmployeeForm implements OnInit {

  newEmployeeObj: EmployeeModel = new EmployeeModel();

  empService = inject(EmployeeService);
  masterSrv = inject(Master);
  activatedRoute = inject(ActivatedRoute);
  cdr = inject(ChangeDetectorRef);

  loggedEmpData: EmployeeModel = new EmployeeModel();

  constructor() {
    const localData = localStorage.getItem("empLoginUser")
    if (localData != null) {
      this.loggedEmpData = JSON.parse(localData)
    }
  }

  $designationList: Observable<DesignationListModel[]> =
    this.masterSrv.getAllDesignation();

  ngOnInit(): void {
    // debugger;
    this.activatedRoute.params.subscribe(params => {
      const id = +params['id'];
      if (id && id !== 0) {
        this.empService.getEmpById(id).subscribe({
          next: (result) => {
            this.newEmployeeObj = { ...result };
            this.cdr.detectChanges();
          }
        });
      } else {
        this.newEmployeeObj = new EmployeeModel();
      }
    });
  }

  onSaveEmp() {

    if (!this.newEmployeeObj.name || this.newEmployeeObj.name.trim() === "") {
      alert("Name is required");
      return;
    }

    if (!this.newEmployeeObj.email || this.newEmployeeObj.email.trim() === "") {
      alert("Email is required");
      return;
    }

    const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

    if (!emailPattern.test(this.newEmployeeObj.email)) {
      alert("Enter a valid email address");
      return;
    }

    if (!this.newEmployeeObj.contactNo || this.newEmployeeObj.contactNo.trim() === "") {
      alert("Contact number is required");
      return;
    }

    if (this.newEmployeeObj.contactNo.length !== 10) {
      alert("Contact number must be 10 digits");
      return;
    }

    if (!this.newEmployeeObj.pincode || this.newEmployeeObj.pincode.trim() === "") {
      alert("Pincode is required");
      return;
    }

    if (this.newEmployeeObj.pincode.length !== 6) {
      alert("Pincode must be 6 digits");
      return;
    }

    if (this.newEmployeeObj.designationId === 0) {
      alert("Please select designation");
      return;
    }

    if (!this.newEmployeeObj.role || this.newEmployeeObj.role.trim() === "") {
      alert("Role is required");
      return;
    }

    // CONTROL FLOW
    if (this.newEmployeeObj.employeeId === 0) {

      // CREATE EMPLOYEE
      debugger;
      this.empService.saveEmployee(this.newEmployeeObj).subscribe({
        next: () => {
          alert("Employee Created Successfully");
          this.newEmployeeObj = new EmployeeModel();
        },
        error: (error) => {
          debugger;
          if (error.status === 400) {
            alert(error.error);
          } else {
            alert("Something went wrong");
          }
        }
      });

    } else {

      // UPDATE EMPLOYEE
      this.empService.updateEmployee(this.newEmployeeObj.employeeId, this.newEmployeeObj).subscribe({
        next: () => {
          alert("Employee Updated Successfully");
        },
        error: (error) => {
          if (error.status === 400) {
            alert(error.error);
          } else {
            alert("Something went wrong");
          }
        }
      });

    }

  }

}