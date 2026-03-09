import { HttpClient, HttpHeaders } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { EmployeeModel, IEmployeeListModel } from '../models/Employee.model';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class EmployeeService {

  apiUrl: string = "https://localhost:7117/api/";
  http = inject(HttpClient);

  getHeaders() {
    const user = JSON.parse(localStorage.getItem('empLoginUser') || '{}');

    return {
      headers: new HttpHeaders({
        Authorization: `Bearer ${user.token}`
      })
    };
  }

  saveEmployee(obj: EmployeeModel) {
    return this.http.post(this.apiUrl + "EmployeeMaster", obj, this.getHeaders());
  }

  getAllEmployee(): Observable<IEmployeeListModel[]> {
    return this.http.get<IEmployeeListModel[]>(
      this.apiUrl + "EmployeeMaster",
      this.getHeaders()
    );
  }

  getEmpById(id:number): Observable<EmployeeModel>{
    return this.http.get<EmployeeModel>(
      this.apiUrl + "EmployeeMaster/" + id,
      this.getHeaders()
    );
  }

  updateEmployee(id: number, obj: EmployeeModel) {
    return this.http.put(
      this.apiUrl + "EmployeeMaster/" + id,
      obj,
      this.getHeaders()
    );
  }
}