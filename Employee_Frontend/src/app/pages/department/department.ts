import { Component, inject, OnInit, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Master } from '../../services/master';
import { DepartmentModel } from '../../models/Department.model';

@Component({
  selector: 'app-department',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './department.html',
  styleUrl: './department.css'
})
export class Department implements OnInit {

  newDepObj: DepartmentModel = new DepartmentModel();

  masterService = inject(Master);

  deptList = signal<DepartmentModel[]>([]);

  ngOnInit(): void {
    this.getAllDepartments();
  }

  onSaveDept() {
    this.masterService.saveDept(this.newDepObj).subscribe(({
      next: (result: any) => {
        alert("Depertment Created Successfully")
        this.getAllDepartments();

        this.newDepObj = {
          departmentId: 0,
          departmentName: '',
          isActive: false
        };
      },
      error: (error) => {
        alert(error.error)
      }
    }))
  }

  onUpdateDept() {
    this.masterService.UpdateDept(this.newDepObj).subscribe(({
      next: (result: any) => {
        debugger;
        alert("Depertment Updated Successfully")
        this.getAllDepartments();

        this.newDepObj = {
          departmentId: 0,
          departmentName: '',
          isActive: false
        };
      },
      error: (error) => {
        debugger;
        alert("Depertment must be Unique")
      }
    }))
  }

  onDelete(id: number) {
    const isDelete = confirm("Are you sure want to Delete")
    if (isDelete) {
      this.masterService.DeleteDeptById(id).subscribe(({
        next: (result: any) => {
          debugger;
          alert(result.message)
          this.getAllDepartments();
        },
        error: (error) => {
          debugger;
          alert(error.error)
        }
      }))
    }
  }


  onEdit(data: DepartmentModel) {
    const strData = JSON.stringify(data);
    const parseData = JSON.parse(strData);
    this.newDepObj = parseData;
  }

  onReset() {
    this.newDepObj = new DepartmentModel();
  }

  getAllDepartments() {
    this.masterService.getAllDept().subscribe({
      next: (result: any) => {
        console.log("API Result:", result);
        this.deptList.set(result);
      }
    });
  }

}