


import { Routes } from '@angular/router';
import { DashboardComponent } from '../../../_component/dashboard/dashboard.component';
import { StudentrecordComponent } from '../../../_component/studentrecord/studentrecord.component';
import { StudentrecordAddComponent } from '../../../_component/studentrecord/studentrecord-add.component';
import { StudentrecordEditComponent } from '../../../_component/studentrecord/studentrecord-edit.component';

import { AuthGuard } from '../../../_guards/auth.guard';

export const APPLAYOUT_ROUTES: Routes = [
	{
		path: 'dashboard', component: DashboardComponent, canActivate: [AuthGuard]
	},
	{
		path: 'studentrecord', component: StudentrecordComponent, canActivate: [AuthGuard],
		//children: [
		//	{ path: 'add', component: StudentrecordAddComponent, canActivate: [AuthGuard] },
		//	{ path: 'edit/:id', component: StudentrecordEditComponent, canActivate: [AuthGuard] }
		//]
	},
	{
		path: 'addstudent', component: StudentrecordAddComponent
	},
	{
		path: 'editstudent/:id', component: StudentrecordEditComponent
	},
];
