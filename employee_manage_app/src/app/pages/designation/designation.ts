import { Component, OnInit, inject, signal } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { Master } from '../../services/master';
import { Observable } from 'rxjs';
import { DepartmentModel, DesignationListModel } from '../../models/Department.model';
import { AsyncPipe, CommonModule, NgClass } from '@angular/common';
import { ChangeDetectorRef } from '@angular/core';

@Component({
  selector: 'app-designation',
  standalone: true,
  imports: [ReactiveFormsModule, NgClass, CommonModule],
  templateUrl: './designation.html',
  styleUrls: ['./designation.css']
})
export class Designation {

  masterSrv = inject(Master);
  fb = inject(FormBuilder);
  cdr = inject(ChangeDetectorRef);

  designationForm!: FormGroup;

  designationList: DesignationListModel[] = [];
  departmentList: DepartmentModel[] = [];

  editingId: number = 0;

  ngOnInit(): void {
    this.createForm();
    this.loadDepartments();
    this.loadDesignations();
  }

  // ======================
  // FORM CREATION
  // ======================

  createForm() {

    this.designationForm = this.fb.group({
      designationId: [0],
      departmentId: [null, Validators.required],
      designationName: ['', Validators.required]
    });
  }

  // ======================
  // LOAD DEPARTMENTS
  // ======================

  loadDepartments() {
    this.masterSrv.getAllDept().subscribe((res: any) => {
      this.departmentList = res;
    });
  }

  // ======================
  // LOAD DESIGNATIONS
  // ======================

  loadDesignations() {

    this.masterSrv.getAllDesignation()
      .subscribe((res: DesignationListModel[]) => {

        this.designationList = [...res]; // force new reference
        this.cdr.detectChanges();

      });

  }

  // ======================
  // SAVE
  // ======================

  onSave() {

    if (this.designationForm.invalid) return;

    const data = this.designationForm.value;

    if (data.designationId != 0) {

      this.masterSrv.UpdateDesignation(data.designationId, data)
        .subscribe(() => {

          this.loadDesignations();
          this.onReset();
          this.cdr.detectChanges();

        });

    } else {

      this.masterSrv.saveDesignation(data)
        .subscribe(() => {

          this.loadDesignations();
          this.onReset();
          this.cdr.detectChanges();

        });

    }

  }
  // ======================
  // EDIT
  // ======================

  onEdit(item: any) {

    this.designationForm.patchValue({
      designationId: item.designationId,
      departmentId: item.departmentId,
      designationName: item.designationName
    });

  }

  // ======================
  // UPDATE
  // ======================

  // onUpdate() {

  //   this.masterSrv.UpdateDesignation(this.designationForm.value)
  //     .subscribe(() => {

  //       alert("Designation Updated");

  //       this.loadDesignations();
  //       this.onReset();

  //     });

  // }

  // ======================
  // DELETE
  // ======================

  onDelete(id: number) {

    if (!confirm("Delete this designation?")) return;

    this.masterSrv.DeleteDesignationById(id)
      .subscribe(() => {

        alert("Deleted");

        this.loadDesignations();
        this.cdr.detectChanges();

      });
  }

  // ======================
  // RESET
  // ======================

  onReset() {

    this.editingId = 0;

    this.designationForm.reset();

    this.designationForm.patchValue({
      designationId: 0,
      departmentId: null,
      designationName: ''
    });

  }

}