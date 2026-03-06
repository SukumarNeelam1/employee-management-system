import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { DepartmentModel, DesignationModel, DesignationListModel } from '../models/Department.model';
import { Observable } from 'rxjs';
// import { DesignationListModel } from '../../models/Department.model';

@Injectable({
  providedIn: 'root',
})
export class Master {

  apiUrl: string = "https://localhost:7117/api/"
  http = inject(HttpClient);

  getAllDept() {
    return this.http.get(this.apiUrl + "DepartmentMaster/GetAllDepartments")
  }

  // Post api call
  saveDept(obj: DepartmentModel) {
    return this.http.post(this.apiUrl + "DepartmentMaster/AddDepartment", obj)
  }

  // Put api call
  UpdateDept(obj: DepartmentModel) {
    return this.http.put(this.apiUrl + "DepartmentMaster/UpdateDepartment", obj)
  }

  // Delete api call using id
  DeleteDeptById(id: number) {
    return this.http.delete(this.apiUrl + "DepartmentMaster/DeleteDepartment/" + id)
  }

  getAllDesignation(): Observable<DesignationListModel[]> {
    return this.http.get<DesignationListModel[]>(this.apiUrl + "Designation/")
  }

  saveDesignation(obj: DesignationModel) {
    return this.http.post(this.apiUrl + "Designation/", obj)
  }

  UpdateDesignation(id: number, obj: any) {
    return this.http.put(this.apiUrl + "Designation/" + id, obj);
  }

  DeleteDesignationById(id: number) {
    return this.http.delete(this.apiUrl + "Designation/" + id)
  }
}
