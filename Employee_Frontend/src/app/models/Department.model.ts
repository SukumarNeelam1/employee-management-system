export class DepartmentModel {
    departmentId: number;
    departmentName: string;
    isActive: boolean;

    constructor() {
        this.departmentId = 0;
        this.departmentName = "";
        this.isActive = false;
    }
}

// Post Api Call 
export interface DesignationModel {
    designationId: number;
    departmentId: number;
    designationName: string;
}

// Get Api Call
export interface DesignationListModel {
    designationId: number;
    departmentId: number;
    designationName: string;
    departmentName: string;
}
