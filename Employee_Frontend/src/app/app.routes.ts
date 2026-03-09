import { Routes } from '@angular/router';
import { Login } from './pages/login/login';
import { Header } from './pages/header/header';
import { Dashboard } from './pages/dashboard/dashboard';
import { EmployeeForm } from './pages/employee-form/employee-form';
import { Department } from './pages/department/department';
import { EmployeeList } from './pages/employee-list/employee-list';
import { Designation } from './pages/designation/designation';
import { Register } from './register/register';

export const routes: Routes = [{
    path: '',
    redirectTo: 'login',
    pathMatch: 'full'
},
{
    path: 'login',
    component: Login
},
{
    path: 'register',
    component: Register
},
{
    path: '',
    component: Header,
    children: [
        {
            path: 'dashboard',
            component: Dashboard
        },
        {
            path: 'new-employee/:id',
            component: EmployeeForm
        },
        {
            path: 'employees',
            component: EmployeeList
        },
        {
            path: 'department',
            component: Department
        },
        {
            path: 'designation',
            component: Designation
        }
    ]
}

];
